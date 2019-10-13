using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boy : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Lane lane;
    private bool _jumping;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        lane = Lane.Middle;
    }

    // Update is called once per frame
    void Update()
    {
        _jumping = rigidbody.velocity != Vector3.zero;
        if (Input.GetKeyDown("right") && lane != Lane.Right && !_jumping)
        {
            transform.position += new Vector3(1, 0, 0);
            lane++;
        }
        if (Input.GetKeyDown("left") && lane != Lane.Left && !_jumping) 
        {
            transform.position += new Vector3(-1, 0, 0);
            lane--;
        }
        if (Input.GetKeyDown("up") && !_jumping)
        {
            rigidbody.velocity = new Vector3(0, 4, 0);
        }
    }
}
