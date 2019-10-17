using UnityEngine;

public class Ground : MonoBehaviour
{
    public Material material;
    private bool _stop = false;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnObjectCollision += StopMoving;
        material = GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (!_stop)
        {
            material.mainTextureOffset += new Vector2(0, -GameManager.GameSpeed);
        }
    }

    void OnDisable()
    {
        EventManager.OnObjectCollision -= StopMoving;
    }

    void StopMoving()
    {
        _stop = true;
        material.mainTextureOffset = new Vector2(0, 0);
    }
}
