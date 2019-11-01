using System;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Events;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static float _startTime;
    public static float GameSpeed => (float)(1 + Math.Sqrt((Time.time - _startTime) / 100000));

    public GameObject MainCharacter;
    
    public static GameManager Instance { get; private set; } // static singleton

    private readonly Assets.Scripts.EventHandling.EventHandler<CollisionHappenedEvent> _collisionHandler = new Assets.Scripts.EventHandling.EventHandler<CollisionHappenedEvent>();

    public bool GameRunning => Time.timeScale > 0;

    void Awake()
    {
        _startTime = Time.time;
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    // Start is called before the first frame update
    void Start()
    {
        _collisionHandler.EventAction += HandleCollisionHappened;
    }

    public void StartGame()
    {
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
            Time.timeScale = 0;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
