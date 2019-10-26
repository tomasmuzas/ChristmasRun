using System;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Events;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    public Swipe Swipe;
    private Rigidbody _rigidbody;
    private Lane _lane;
    private bool _jumping;
    private bool _sliding;
    public float JumpHeight;
    public float MovementDelta;
    public float MovementTime;
    public GameObject Plane;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _lane = Lane.Middle;
    }

    // Update is called once per frame
    void Update()
    {
        _jumping = Math.Abs(_rigidbody.velocity.y) > 0.01F;

        if ((Input.GetKeyDown("right") || Swipe.SwipeRight) && _lane != Lane.Right && !_jumping && !_sliding)
        {
            _rigidbody.velocity = new Vector3(MovementDelta / MovementTime, 0, 0);
            _lane++;
            StartCoroutine(StopSlide());
        }
        if ((Input.GetKeyDown("left") || Swipe.SwipeLeft) && _lane != Lane.Left && !_jumping && !_sliding)
        {
            _rigidbody.velocity = new Vector3(-MovementDelta / MovementTime, 0, 0);
            _lane--;
            StartCoroutine(StopSlide());
        }
        if ((Input.GetKeyDown("up") || Swipe.SwipeUp) && !_jumping && !_sliding)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, JumpHeight, 0);
            EventManager.PublishEvent(new PlayerJumpedEvent());
        }
    }

    IEnumerator StopSlide()
    {
        _sliding = true;
        yield return new WaitForSeconds(MovementTime);
        _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
        _rigidbody.MovePosition(new Vector3(-0.5f + (int)_lane * 0.5f, _rigidbody.position.y, _rigidbody.position.z));
        _sliding = false;
    }

    void OnTriggerEnter(Collider other)
    {
        EventManager.PublishEvent(new CollisionHappenedEvent
        {
            Object = GetComponent<Collider>(),
            CollidedWith = other
        });
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == Plane)
        {
            return;
        }
        EventManager.PublishEvent(new CollisionHappenedEvent
        {
            Object = GetComponent<Collider>(),
            CollidedWith = other.collider
        });
    }
}
