using UnityEngine;

public class Ground : MonoBehaviour
{
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameRunningAndStarted)
        {
            material.mainTextureOffset += new Vector2(0, -(GameManager.Instance.GameSpeed * Time.deltaTime));
        }
    }
}
