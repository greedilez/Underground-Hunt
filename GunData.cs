using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GunData : MonoBehaviour
{
    [SerializeField] private GameObject[] guns = new GameObject[4];

    [SerializeField] private bool[] gunsAccess = new bool[4];

    public bool[] GunsAccess{ get => gunsAccess; }

    [SerializeField] private Button[] gunsButtons = new Button[4];

    public delegate void methodContainer();

    public event methodContainer onGunChanged;

    [SerializeField] private GameObject gunsMenu;

    public GameObject GunsMenu{ get => gunsMenu; }

    private Person person;

    private void Awake(){
        ShowButtonOnGunAccess();
        person = FindObjectOfType<Person>();
    }

    public void OpenGunsMenu(){
        if(!person.CurrentGun.IsReloading){
            gunsMenu.SetActive(gunsMenu.activeSelf ? false : true);
            if(gunsMenu.activeSelf) ShowButtonOnGunAccess();
        }
    }

    public void ChangeGun(int gunIndex){
        if(gunsAccess[gunIndex]){
            if(!guns[gunIndex].activeSelf){ // Is gun not active
                for (int i = 0; i < guns.Length; i++)
                {
                    if(guns[i] != guns[gunIndex]) guns[i].SetActive(false);
                }
                guns[gunIndex].SetActive(true);
                if(onGunChanged != null) onGunChanged();
            }
            gunsMenu.SetActive(false);
        }
    }

    private protected void ShowButtonOnGunAccess(){
        for (int i = 0; i < gunsAccess.Length; i++)
        {
            if(gunsAccess[i]){
                gunsButtons[i].interactable = true;
            }
            else gunsButtons[i].interactable = false;
        }
    }
}
