using UnityEngine;
using UnityEngine.EventSystems;

public class ShootButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private static bool isShooting = false;

    public static bool IsShooting{ get => isShooting; } 

    public void OnPointerDown(PointerEventData data) => isShooting = true;

    public void OnPointerUp(PointerEventData data) => isShooting = false;
}
