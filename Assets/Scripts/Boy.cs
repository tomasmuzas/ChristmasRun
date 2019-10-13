using Assets.Scripts;
using UnityEngine;

public class Boy : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Lane lane;
    private bool _jumping;
    public float JumpHeight;
    public float MovementDelta;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        lane = Lane.Middle;
    }

    // Update is called once per frame
    void Update()
    {
        _jumping = rigidbody.velocity != Vector3.zero;
        if (Input.GetKeyDown("right") && lane != Lane.Right && !_jumping)
        {
            transform.position += new Vector3(MovementDelta, 0, 0);
            lane++;
        }
        if (Input.GetKeyDown("left") && lane != Lane.Left && !_jumping) 
        {
            transform.position += new Vector3(-MovementDelta, 0, 0);
            lane--;
        }
        if (Input.GetKeyDown("up") && !_jumping)
        {
            rigidbody.velocity = new Vector3(0, JumpHeight, 0);
        }
    }
}
