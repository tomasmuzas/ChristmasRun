using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boy : MonoBehaviour
{
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("right"))
        {
            transform.position += new Vector3(1, 0, 0);
        }
        if (Input.GetKeyDown("left"))
        {
            transform.position += new Vector3(-1, 0, 0);
        }
        if (Input.GetKeyDown("up"))
        {
            rigidbody.velocity = new Vector3(0, 4, 0);
        }
    }
}
