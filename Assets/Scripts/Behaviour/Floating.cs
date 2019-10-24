using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public float Amplitude = 0.5f;
    public float Frequency = 1f;

    private float _yPositionOffset;

    // Use this for initialization
    void Start()
    {
        // Store the starting position & rotation of the object
        _yPositionOffset = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Float up/down with a Sin()
        transform.position = new Vector3(
            transform.position.x,
            _yPositionOffset + Mathf.Sin(Time.fixedTime * Mathf.PI * Frequency) * Amplitude,
            transform.position.z);
    }
}
