using System;
using System.Collections;
using Assets.Scripts;
using UnityEngine;

public class Boy : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Lane lane;
    private bool _jumping;
    private bool _sliding;
    public float JumpHeight;
    public float MovementDelta;
    public float MovementTime;
    public AudioSource jump;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        lane = Lane.Middle;
    }

    // Update is called once per frame
    void Update()
    {
        _jumping = Math.Abs(rigidbody.velocity.y) > 0.01F;

        if (Input.GetKeyDown("right") && lane != Lane.Right && !_jumping && !_sliding)
        {
            rigidbody.velocity = new Vector3(MovementDelta / MovementTime, 0, 0);
            lane++;
            StartCoroutine(StopSlide());
        }
        if (Input.GetKeyDown("left") && lane != Lane.Left && !_jumping && !_sliding) 
        {
            rigidbody.velocity = new Vector3(-MovementDelta / MovementTime, 0, 0);
            lane--;
            StartCoroutine(StopSlide());
        }
        if (Input.GetKeyDown("up") && !_jumping)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, JumpHeight, 0);
            jump.Play();
        }
    }

    IEnumerator StopSlide()
    {
        _sliding = true;
        yield return new WaitForSeconds(MovementTime);
        rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
        _sliding = false;
    }
}
