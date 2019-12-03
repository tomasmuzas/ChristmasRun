using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Events;
using Assets.Scripts.Managers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float _startTime;
    private bool _isTutorial;
    private bool _gameStarted;

    private readonly Assets.Scripts.EventHandling.EventHandler<CollisionHappenedEvent> _collisionHandler = new Assets.Scripts.EventHandling.EventHandler<CollisionHappenedEvent>();
    private readonly Assets.Scripts.EventHandling.EventHandler<TutorialStartedEvent> _tutorialStartedHandler = new Assets.Scripts.EventHandling.EventHandler<TutorialStartedEvent>();
    private readonly Assets.Scripts.EventHandling.EventHandler<TutorialFinishedEvent> _tutorialFinishedHandler = new Assets.Scripts.EventHandling.EventHandler<TutorialFinishedEvent>();

    public GameObject MainCharacter;
    [SerializeField]
    public List<Skin> Skins;
    public float GameSpeedModifier;

    public bool ShouldCountScore => !_isTutorial && GameRunningAndStarted;
    public float GameSpeed => GameSpeedModifier * (Time.time - _startTime) + 1;
    
    public static GameManager Instance { get; private set; } // static singleton

    public bool GameRunning => Time.timeScale > 0;
    
    public bool GameRunningAndStarted => GameRunning && _gameStarted;

    void Awake()
    {
        Time.timeScale = 1;
        _startTime = Time.time;
        _tutorialStartedHandler.EventAction += e => _isTutorial = true;
        _tutorialFinishedHandler.EventAction += e => _isTutorial = false;
        if (Instance == null) { Instance = this; }

        var currentSkinName = PlayerPrefs.GetString(PlayerPrefKeys.EquippedSkin, SkinNames.Boy);
        var currentSkin = Skins.SingleOrDefault(s => s.Name == currentSkinName) ?? Skins.Single(s => s.Name == SkinNames.Boy);
        MainCharacter = Instantiate(
            currentSkin.Prefab, 
            new Vector3(0f, 1.19f, 0f),
            currentSkin.Prefab.transform.rotation);
    }

    // Start is called before the first frame update
    void Start()
    {
        _collisionHandler.EventAction += HandleCollisionHappened;
    }

    public void StartGame()
    {
        _gameStarted = true;
        SpawnManager.Instance.spawnStrategy.StartSpawning();
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
            Time.timeScale = 0;
            _gameStarted = false;
        }
    }
}
