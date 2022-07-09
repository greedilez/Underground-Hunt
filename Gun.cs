using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform extremeGunPoint;

    private ParticleSystem particle;

    [Range(0.05f, 5f)]
    [SerializeField] private float shootDelay = 0.5f;

    private bool isShooted = false;

    private Animator animator;

    private AudioSource source;

    [SerializeField] private AudioClip shootClip, reloadClip;

    private GameObject mainCamera;

    [SerializeField] private GameObject decal, bloodDecal;

    [SerializeField] private int currentPatrons = 35;

    [SerializeField] private int maxPatrons = 35;

    [SerializeField] private float reloadDelay = 2f;

    public int CurrentPatrons{ get => currentPatrons; }

    public int MaxPatrons{ get => maxPatrons; }

    private Person person;

    [SerializeField] private float decalSize = 1f;

    private bool isReloading = false;

    public bool IsReloading{ get => isReloading; }
    
    [SerializeField] private float zombieDamage = 10f;

    private GunData data;

    private void Start(){
        InitializeFields();
    }

    private protected void InitializeFields(){
        data = FindObjectOfType<GunData>();
        person = FindObjectOfType<Person>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        particle = GetComponentInChildren<ParticleSystem>();
    }

    private void FixedUpdate(){
        ShowReloadButtonOnZeroPatrons();
    }

    public void Shoot(){
        if(!isShooted){
            if(currentPatrons > 0){
                currentPatrons--;
                SendRaycast();
                source.PlayOneShot(shootClip);
                particle.Play(true);
                animator.SetTrigger("Shoot");
                StartCoroutine(ApplyDelay());
                isShooted = true;
            }
        }
    }

    private protected void ShowReloadButtonOnZeroPatrons(){
        if(currentPatrons > 0) person.ReloadButton.enabled = false;
        else person.ReloadButton.enabled = true;
    }

    public void Reload(){
        if(!isReloading){
            if(currentPatrons <= 0){
                isReloading = true;
                data.GunsMenu.SetActive(false);
                animator.SetTrigger("Reload");
                source.PlayOneShot(reloadClip);
                StartCoroutine(ReloadDelay());
            }
        }
    }

    private IEnumerator ReloadDelay(){
        yield return new WaitForSeconds(reloadDelay);{
            currentPatrons = maxPatrons;
            isReloading = false;
        }
    }

    private protected void SendRaycast(){
        RaycastHit hit;
        if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, Mathf.Infinity)){
            CreateDecal(hit);
            if(hit.collider.tag == "Zombie"){
                AttackZombie(hit);
            }
        }
    }

    private protected void AttackZombie(RaycastHit hit) => hit.collider.GetComponent<Zombie>().SetDamage(zombieDamage);

    private protected void CreateDecal(RaycastHit hit){
        if(hit.collider.tag != "Player" && hit.collider.tag != "Zombie"){
            InstantiateDecal(decal, hit);
        }
        else if(hit.collider.tag == "Zombie"){
            if(!hit.collider.GetComponent<Zombie>().IsZombieDead){
                InstantiateDecal(bloodDecal, hit);
            }
        }
    }

    private void InstantiateDecal(GameObject decal, RaycastHit hit){
        GameObject decalInstance = Instantiate(decal, hit.point + (hit.normal * 0.01f), Quaternion.LookRotation(hit.normal));
        decalInstance.transform.localScale = new Vector3(decalSize, decalSize, decalInstance.transform.localScale.z);
    }

    private IEnumerator ApplyDelay(){
        yield return new WaitForSeconds(shootDelay);{
            if(isShooted) isShooted = false;
        }
    }
}
