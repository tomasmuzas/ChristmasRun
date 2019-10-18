using UnityEngine;

public class Moving : MonoBehaviour
{
    private Rigidbody _body;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _body.velocity = new Vector3(0, 0, -GameManager.GameSpeed * 100);
    }
}
