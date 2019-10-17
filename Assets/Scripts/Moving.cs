using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    private Rigidbody _body;

    private bool _stop = false;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnObjectCollision += StopMoving;
        _body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_stop)
        {
            _body.velocity = new Vector3(0, 0, -GameManager.GameSpeed * 100);
        }
    }

    void OnDisable()
    {
        EventManager.OnObjectCollision -= StopMoving;
    }

    void StopMoving()
    {
        _stop = true;
        _body.velocity = new Vector3(0, 0, 0);
    }
}
