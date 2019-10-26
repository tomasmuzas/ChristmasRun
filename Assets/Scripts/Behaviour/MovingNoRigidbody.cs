﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingNoRigidbody : MonoBehaviour
{
    private float _velocity;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _velocity = -GameManager.GameSpeed * 100;
        if (Time.deltaTime > 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + _velocity * Time.deltaTime);
        }
    }
}
