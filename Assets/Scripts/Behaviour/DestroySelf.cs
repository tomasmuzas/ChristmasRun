using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float DestroyAfter;
    // Start is called before the first frame update
    void Start()
    {
        var smoothDestroy = gameObject.GetComponent<SmoothDestroy>();
        if (smoothDestroy)
        {
            smoothDestroy.StartDestroy(DestroyAfter);
        }
        else
        {
            Destroy(gameObject, DestroyAfter);
        }
    }
}
