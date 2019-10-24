using UnityEngine;

public class Rotating : MonoBehaviour
{
    public float RotationSpeed;
    public Vector3 RotationAxis;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(RotationAxis, RotationSpeed * Time.deltaTime);
    }
}