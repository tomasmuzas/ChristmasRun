using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public float InitialGameSpeed;
    public float SpawnSpeed;
    public static float GameSpeed;
    public GameObject MyPrefab;
    public static Random Rnd = new Random();

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
        var spawnable = MyPrefab.GetComponent<Spawnable>();
        if (spawnable != null && spawnable.SpawnPositions != null && spawnable.SpawnPositions.Count > 0)
        {
            var location = spawnable.SpawnPositions[Rnd.Next(0, spawnable.SpawnPositions.Count)];
            Instantiate(MyPrefab, location.Position, Quaternion.identity);
        }
    }
}
