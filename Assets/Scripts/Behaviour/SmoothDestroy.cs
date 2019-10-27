using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothDestroy : MonoBehaviour
{
    public float DestroySpeed;

    private bool _destroy;
    private Vector3 _startScale;

    void Start()
    {
        _startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (_destroy)
        {
            if (transform.localScale.x >= _startScale.x * 0.1f)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, DestroySpeed * Time.deltaTime);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void StartDestroy()
    {
        _destroy = true;
    }
}
