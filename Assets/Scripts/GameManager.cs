using System.Collections.Generic;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Events;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public float InitialGameSpeed;
    public float SpawnSpeed;
    public static float GameSpeed;
    [SerializeField]
    public List<Spawnable> SpawnablePrefabs;
    public GameObject GameOverText;
    public GameObject MainCharacter;
    public static GameManager Instance { get; private set; } // static singleton

    private static readonly Random Rnd = new Random();
    private readonly EventHandler<CollisionHappenedEvent> _collisionHandler = new EventHandler<CollisionHappenedEvent>();

    private int Points;
    public Text PointsText;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        // Cache references to all desired variables
        MainCharacter = FindObjectOfType<MainCharacter>().gameObject;
        _collisionHandler.EventAction += HandleCollisionHappened;
        GameSpeed = InitialGameSpeed;
        InvokeRepeating(nameof(SpawnObject), SpawnSpeed, SpawnSpeed);
        InvokeRepeating("AddPoints", 0f, GameSpeed * 20);
    }

    void Update()
    {
        SetPointsText();   
    }
    void SpawnObject()
    {
        if (SpawnablePrefabs?.Count > 0)
        {
            var spawnablePrefab = SpawnablePrefabs[Rnd.Next(0, SpawnablePrefabs.Count)];
            if (spawnablePrefab.SpawnPositions?.Count > 0)
            {
                var location = spawnablePrefab.SpawnPositions[Rnd.Next(0, spawnablePrefab.SpawnPositions.Count)];
                Instantiate(spawnablePrefab, location.Position, Quaternion.identity);
            }
        }
    }

    private void HandleCollisionHappened(CollisionHappenedEvent @event)
    {
        CancelInvoke(nameof(SpawnObject));
        GameOverText.SetActive(true);
        Time.timeScale = 0;
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
