using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Behaviour;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;
using Text = TMPro.TextMeshProUGUI;

public class GameManager : MonoBehaviour
{
    public float InitialGameSpeed = 0.01f;
    public float InitialSpawnSpeed = 2;
    public float DifficultyConstant = 20;
    public static float GameSpeed;
    public static float SpawnSpeed;
    [SerializeField]
    public List<Spawnable> SpawnablePrefabs;
    public GameObject MainCharacter;
    public static GameManager Instance { get; private set; } // static singleton

    private static readonly Random Rnd = new Random();
    private readonly EventHandler<CollisionHappenedEvent> _collisionHandler = new EventHandler<CollisionHappenedEvent>();
    public List<GameObject> HousePrefabs;

    public bool GameRunning => Time.timeScale > 0;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        GameSpeed = InitialGameSpeed;
        SpawnSpeed = InitialSpawnSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        _collisionHandler.EventAction += HandleCollisionHappened;

    }
    public void StartGame()
    {
        StartCoroutine(SpawnObject());
        StartCoroutine(SpawnHouses());
        InvokeRepeating(nameof(IncreaseDifficulty), 0.5f, 0.5f);
    }
    //Used for diplaying framerate
    void OnGUI()
    {
        GUI.Label(new Rect(0f, 0f, 100f, 100f), (1.0f / Time.smoothDeltaTime).ToString());
    }

    IEnumerator SpawnObject()
    {
        while (GameRunning)
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
            yield return new WaitForSeconds(SpawnSpeed);
        }
    }

    private void HandleCollisionHappened(CollisionHappenedEvent @event)
    {
        if (@event.CollidedWith.GetComponent<Valuable>() != null)
        {
            var value = @event.CollidedWith.GetComponent<Valuable>().Value;
            var smoothDestroy = @event.CollidedWith.GetComponent<SmoothDestroy>();
            if (smoothDestroy)
            {
                smoothDestroy.StartDestroy();
            }
            else
            {
                Destroy(@event.CollidedWith.gameObject);
            }
            EventManager.PublishEvent(new GiftCollectedEvent { Value = value});
        }
        else if(@event.CollidedWith.GetComponent<Destructable>())
        {
            EventManager.PublishEvent(new GameOverEvent());
            CancelInvoke(nameof(IncreaseDifficulty));
            Time.timeScale = 0;
        }
    }

    private IEnumerator SpawnHouses()
    {
        while (GameRunning)
        {
            yield return new WaitForSeconds(0.8f / (GameSpeed * 100));
            if (HousePrefabs?.Count > 0)
            {
                var house = HousePrefabs[Rnd.Next(0, HousePrefabs.Count)];
                Instantiate(house, new Vector3(-1.204675f, 1.139f, 4.5501f), Quaternion.Euler(0f, 13.479f, 0f));
                house = HousePrefabs[Rnd.Next(0, HousePrefabs.Count)];
                Instantiate(house, new Vector3(1.23f, 1.139f, 4.91f), Quaternion.Euler(0f, 160f, 0f));
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void IncreaseDifficulty()
    {
        GameSpeed = InitialGameSpeed * (1 + Time.time/DifficultyConstant);
        SpawnSpeed = Time.time / DifficultyConstant > 1
            ? InitialSpawnSpeed / (Time.time / DifficultyConstant)
            : InitialSpawnSpeed;
    }
}
