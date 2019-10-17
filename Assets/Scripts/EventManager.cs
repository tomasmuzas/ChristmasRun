using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void ObjectCollision();
    public static event ObjectCollision OnObjectCollision;

    public static void PublishObjectCollision()
    {
        OnObjectCollision?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
