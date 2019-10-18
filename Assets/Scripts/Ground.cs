using UnityEngine;

public class Ground : MonoBehaviour
{
    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            material.mainTextureOffset += new Vector2(0, -GameManager.GameSpeed);
        }
    }
}
