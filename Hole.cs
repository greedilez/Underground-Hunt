using UnityEngine;

public class Hole : MonoBehaviour
{
    private void Awake() => Destroy(gameObject, 10f);
}
