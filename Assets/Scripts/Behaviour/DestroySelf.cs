using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float DestroyAfter;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(DestroyThisGameObject), DestroyAfter);
    }

    public void ResetDestructionTime()
    {
        CancelInvoke(nameof(DestroyThisGameObject));
        Invoke(nameof(DestroyThisGameObject), DestroyAfter);
    }

    public void DestroyThisGameObject()
    {
        var smoothDestroy = gameObject.GetComponent<SmoothDestroy>();
        if (smoothDestroy)
        {
            smoothDestroy.StartDestroy();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
