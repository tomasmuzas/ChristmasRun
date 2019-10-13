using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float InitialGameSpeed;
    public float SpawnSpeed;
    public static float GameSpeed;
    public GameObject myPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameSpeed = InitialGameSpeed;
        InvokeRepeating("SpawnObject", SpawnSpeed, SpawnSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnObject()
    {
        Instantiate(myPrefab, new Vector3(0, 0, 2.631f), Quaternion.identity);
    }
}
