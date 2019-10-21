using System.Collections.Generic;
using Assets.Scripts.Behaviour;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Events;
using UnityEngine;
using Random = System.Random;
using Text = TMPro.TextMeshProUGUI;

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
    private readonly EventHandler<GiftCollectedEvent> _giftHandler = new EventHandler<GiftCollectedEvent>();

    private int Points = 0;
    public Text PointsText;
    private int Gifts = 0;
    public Text GiftsText;
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
        InvokeRepeating(nameof(SpawnObject), SpawnSpeed, SpawnSpeed);
        InvokeRepeating(nameof(AddPoints), 0f, GameSpeed * 20);
        GiftsText.text = Gifts.ToString();
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
            GameOverText.SetActive(true);
            Time.timeScale = 0;
        }
    }

    void HandleGiftCollected(GiftCollectedEvent @event)
    {
        Gifts += @event.Value;
        GiftsText.text = Gifts.ToString();
    }

    void SetPointsText()
    {
        PointsText.text = Points.ToString();
    }

    void AddPoints()
    {
        Points++;
    }
}
