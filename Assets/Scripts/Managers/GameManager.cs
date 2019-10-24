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
    public float InitialGameSpeed;
    public float InitialSpawnSpeed;
    public float DifficultyConstant;
    public static float GameSpeed;
    public static float SpawnSpeed;
    [SerializeField]
    public List<Spawnable> SpawnablePrefabs;
    public GameObject GameOverPanel;
    public GameObject MainCharacter;
    public static GameManager Instance { get; private set; } // static singleton

    private static readonly Random Rnd = new Random();
    private readonly EventHandler<CollisionHappenedEvent> _collisionHandler = new EventHandler<CollisionHappenedEvent>();
    private readonly EventHandler<GiftCollectedEvent> _giftHandler = new EventHandler<GiftCollectedEvent>();

    private int _points;
    public Text PointsText;
    private int _gifts;
    public Text GiftsText;

    private bool GameRunning => Time.timeScale > 0;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        // Cache references to all desired variables
        MainCharacter = FindObjectOfType<MainCharacter>().gameObject;
        _collisionHandler.EventAction += HandleCollisionHappened;
        _giftHandler.EventAction += HandleGiftCollected;
        GameSpeed = InitialGameSpeed;
        SpawnSpeed = InitialSpawnSpeed;
        StartCoroutine(AddPoints());
        GiftsText.text = _gifts.ToString();
        StartCoroutine(SpawnObject());
    }

    void Update()
    {
        SetPointsText();
        IncreaseDifficulty();
    }

    IEnumerator SpawnObject()
    {
        while (Time.timeScale > 0)
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
            Debug.Log(SpawnSpeed);
            yield return new WaitForSeconds(SpawnSpeed);
        }
    }

    private void HandleCollisionHappened(CollisionHappenedEvent @event)
    {
        if (@event.CollidedWith.GetComponent<Valuable>() != null)
        {
            var value = @event.CollidedWith.GetComponent<Valuable>().Value;
            Destroy(@event.CollidedWith.gameObject);
            EventManager.PublishEvent(new GiftCollectedEvent { Value = value});
        }
        else if(@event.CollidedWith.GetComponent<Destructable>())
        {
            EventManager.PublishEvent(new GameOverEvent());
            CancelInvoke(nameof(SpawnObject));
            CancelInvoke(nameof(AddPoints));
            GameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    void HandleGiftCollected(GiftCollectedEvent @event)
    {
        _gifts += @event.Value;
        GiftsText.text = _gifts.ToString();
    }

    void SetPointsText()
    {
        PointsText.text = _points.ToString();
    }

    IEnumerator AddPoints()
    {
        while (GameRunning)
        {
            _points++;
            yield return new WaitForSeconds(0.01f / GameSpeed);
        }
        
    }

    public void RestartGame()
    {
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
