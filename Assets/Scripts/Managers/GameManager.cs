using Assets.Scripts.Behaviour;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Events;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float InitialGameSpeed = 0.01f;
    public float DifficultyConstant = 20;
    public static float GameSpeed;
    public GameObject MainCharacter;
    
    public static GameManager Instance { get; private set; } // static singleton

    private readonly EventHandler<CollisionHappenedEvent> _collisionHandler = new EventHandler<CollisionHappenedEvent>();

    public bool GameRunning => Time.timeScale > 0;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        GameSpeed = InitialGameSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        _collisionHandler.EventAction += HandleCollisionHappened;
    }
    public void StartGame()
    {
        InvokeRepeating(nameof(IncreaseDifficulty), 0.5f, 0.5f);
        SpawnManager.Instance.StartSpawning();
    }
    //Used for diplaying framerate
    void OnGUI()
    {
        GUI.Label(new Rect(0f, 0f, 100f, 100f), (1.0f / Time.smoothDeltaTime).ToString());
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

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void IncreaseDifficulty()
    {
        GameSpeed = InitialGameSpeed * (1 + Time.time/DifficultyConstant);
        SpawnManager.SpawnSpeed = Time.time / DifficultyConstant > 1
            ? SpawnManager.InitialSpawnSpeed / (Time.time / DifficultyConstant)
            : SpawnManager.InitialSpawnSpeed;
    }
}
