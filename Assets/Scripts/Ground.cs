using UnityEngine;

public class Ground : MonoBehaviour
{
    public float SpeedMultiplier = 1;
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameRunning)
        {
            material.mainTextureOffset += new Vector2(0, -GameManager.GameSpeed * SpeedMultiplier);
        }
    }
}
