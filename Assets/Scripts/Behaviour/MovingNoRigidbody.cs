using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingNoRigidbody : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameRunningAndStarted)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - GameManager.Instance.GameSpeed * Time.deltaTime);
        }
    }
}
