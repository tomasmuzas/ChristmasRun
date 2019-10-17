using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public float InitialGameSpeed;
    public float SpawnSpeed;
    public static float GameSpeed;
    public Spawnable SpawnablePrefab;
    public static Random Rnd = new Random();
    public Text PointsText;
    private int Points;
    public float PointsSpeed;

    // Start is called before the first frame update
    void Start()
    {
        GameSpeed = InitialGameSpeed;
        InvokeRepeating("SpawnObject", SpawnSpeed, SpawnSpeed);
        Points = 0;
        SetPointsText();
        InvokeRepeating("AddPoints", 0f, PointsSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        SetPointsText();
    }

    void SpawnObject()
    {
        if (SpawnablePrefab?.SpawnPositions?.Count > 0)
        {
            var location = SpawnablePrefab.SpawnPositions[Rnd.Next(0, SpawnablePrefab.SpawnPositions.Count)];
            Instantiate(SpawnablePrefab, location.Position, Quaternion.identity);
        }
    }

    void SetPointsText()
    {
        PointsText.text = $"Score: {Points}";
    }

    void AddPoints()
    {
        Points++;
    }
}
