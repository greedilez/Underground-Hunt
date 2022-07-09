using UnityEngine;

public class PreviewGun : MonoBehaviour
{
    [SerializeField] private int gunIndexToUnlock = 1;

    private GunData data;

    private void Awake(){
        InitializeFields();
    }

    private protected void InitializeFields(){
        data = FindObjectOfType<GunData>();
    }

    private void OnTriggerEnter(Collider col){
        if(col.tag == "Player"){
            GetGun();
            TakeGun();
            Destroy(gameObject);
        }
    }

    private protected void GetGun() => data.GunsAccess[gunIndexToUnlock] = true;

    private protected void TakeGun() => data.ChangeGun(gunIndexToUnlock);
}
