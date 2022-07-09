using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class Person : MonoBehaviour
{
    [SerializeField] private Joystick moveJoystick, cameraJoystick;

    [Range(0, 50)]
    [SerializeField] private float moveSpeed = 5f;

    [Range(0, 70)]
    [SerializeField] private float lookSpeed = 5f;

    [SerializeField] private float jumpForce = 10f;

    private Rigidbody rb;

    private bool isGrounded = false;

    private Animator animator;

    [SerializeField] private Transform gunPlace;

    [SerializeField] private GameObject mainCamera;

    [SerializeField] private TextMeshProUGUI ammunitionText;

    [SerializeField] private TextMeshProUGUI reloadButton;

    public TextMeshProUGUI ReloadButton{ get => reloadButton; set => reloadButton = value; }

    private Gun currentGun;

    public Gun CurrentGun{ get => currentGun; }

    private float xRot = 0;

    private AudioSource source;

    [SerializeField] private AudioClip firstStepClip, secondStepClip, gunInitializing;

    private bool didStep = false;

    [SerializeField] private float stepDelay = 1f;

    private GunData data;

    private void Awake(){
        InitializeFields();
        UpdateGunOnChange();
    }

    private protected void InitializeFields(){
        currentGun = FindObjectOfType<Gun>();
        data = GetComponent<GunData>();
        source = GetComponent<AudioSource>();
        source.PlayOneShot(gunInitializing);
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private protected void UpdateGunOnChange(){
        data.onGunChanged += () => {
            source.PlayOneShot(gunInitializing);
            currentGun = FindObjectOfType<Gun>();
        };
    }

    private void OnCollisionStay(Collision col){
        if(col.collider.tag == "Ground"){
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision col){
        if(col.collider.tag == "Ground"){
            isGrounded = false;
        }
    }

    private void FixedUpdate(){
        Look();
        CameraLook();
        ClampCameraRotation();
        ShootByTouch();
        Move();
        PlayStepAudio();
        Animate();
        UpdateAmmunitionText();
    }

    private protected void Move(){
        Vector3 translation = new Vector3(moveJoystick.Horizontal, 0, moveJoystick.Vertical);
        transform.Translate(translation * (Time.fixedDeltaTime * moveSpeed), Space.Self);
    }

    private protected void Animate(){
        float move = moveJoystick.Horizontal + moveJoystick.Vertical;
        if(move > 0.1f || move < -0.1f){
            animator.SetBool("Walk", true);
        }
        else animator.SetBool("Walk", false);
    }

    private protected void Look() => transform.Rotate(new Vector3(0, cameraJoystick.Horizontal, 0) * lookSpeed, Space.World);

    private protected void CameraLook() => mainCamera.transform.Rotate(new Vector3(-cameraJoystick.Vertical, 0, 0) * lookSpeed, Space.Self);

    private protected void ClampCameraRotation(){
        xRot += -cameraJoystick.Vertical * lookSpeed;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        mainCamera.transform.rotation = Quaternion.Euler(xRot, mainCamera.transform.eulerAngles.y, mainCamera.transform.eulerAngles.z);
    }

    public void Jump(){
        if(isGrounded){
            rb.velocity = new Vector3(0, jumpForce, 0);
        }
    }

    private protected void ShootByTouch(){
        if(ShootButton.IsShooting){
            currentGun.Shoot();
        }
    }

    public void ReloadCurrentGun() => currentGun.Reload();

    private protected void UpdateAmmunitionText() => ammunitionText.text = $"{currentGun.CurrentPatrons}/{currentGun.MaxPatrons}";

    private protected void PlayStepAudio(){
        float sum = moveJoystick.Vertical + moveJoystick.Horizontal;
        if(sum > 0.1f || sum < -0.1f){
            if(!didStep){
                didStep = true;
                int soundIndex = Random.Range(0, 4);
                source.PlayOneShot(StepSound(soundIndex));
                StartCoroutine(NextStepDelay());
            }
        }
    }

    private IEnumerator NextStepDelay(){
        yield return new WaitForSeconds(stepDelay);{
            if(didStep) didStep = false;
        }
    }

    private AudioClip StepSound(int soundIndex){
        switch(soundIndex){
            case <= 2:
            return firstStepClip;

            case > 2:
            return secondStepClip;
        }
    }
}
