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
    private int _highScore;
    public Text HighScoreText;
    private int _totalGifts;
    public Text GiftsTotalText;
    private bool _highScoreReached;
    public GameObject HousePrefab;

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
        StartCoroutine(SpawnHouses());
        _highScore = PlayerPrefs.GetInt("highscore", _highScore);
        _totalGifts = PlayerPrefs.GetInt("totalgifts", _totalGifts);
        _highScoreReached = false;
        //HighScoreText.text = $"Highscore {_highScore}!";

    }

    void Update()
    {
        SetPointsText();
        IncreaseDifficulty();
        Debug.Log(GameSpeed);
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
            SetAllGifts();
            GameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    void SetAllGifts()
    {
        _totalGifts += _gifts;
        PlayerPrefs.SetInt("totalgifts", _totalGifts);
        GiftsTotalText.text = $"Total gifts: {_totalGifts}";
    }
    void HandleGiftCollected(GiftCollectedEvent @event)
    {
        _gifts += @event.Value;
        GiftsText.text = _gifts.ToString();
    }

    private IEnumerator SpawnHouses()
    {
        while (GameRunning)
        {
            Instantiate(HousePrefab, new Vector3(-1.204675f, 1.139f, 4.5501f), Quaternion.Euler(0f, 13.479f, 0f));
            Instantiate(HousePrefab, new Vector3(1.23f, 1.139f, 4.91f), Quaternion.Euler(0f, 160f, 0f));
            yield return new WaitForSeconds(0.8f / (GameSpeed * 100));
        }
    }

    void SetPointsText()
    {
        PointsText.text = _points.ToString();
        if(_points > _highScore)
        {
            _highScore = _points;
            PlayerPrefs.SetInt("highscore", _highScore);
            PlayerPrefs.Save();
            if(!_highScoreReached)
                StartCoroutine("SetHighScoreText");
        }
    }

    IEnumerator SetHighScoreText()
    {
        HighScoreText.text = "Highscore!";
        HighScoreText.enabled = true;
        yield return new WaitForSeconds(2f);
        HighScoreText.enabled = false;
        _highScoreReached = true;

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
