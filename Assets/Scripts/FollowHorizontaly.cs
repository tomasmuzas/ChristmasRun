using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHorizontaly : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private GameObject _objectToFollow;
    public float FollowSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _objectToFollow = GameManager.Instance?.MainCharacter;
    }

    // Update is called once per frame
    void Update()
    {
        if (_objectToFollow != null && Math.Abs(_objectToFollow.transform.position.x - _rigidbody.position.x) > 0.05F)
        {
            var xSpeed = _objectToFollow.transform.position.x - _rigidbody.position.x > 0 ? FollowSpeed : -FollowSpeed;
            _rigidbody.velocity = new Vector3(xSpeed, _rigidbody.velocity.y, _rigidbody.velocity.z);
        }
    }
}
