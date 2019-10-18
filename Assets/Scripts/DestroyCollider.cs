using UnityEngine;

public class DestroyCollider : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        Destroy(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
