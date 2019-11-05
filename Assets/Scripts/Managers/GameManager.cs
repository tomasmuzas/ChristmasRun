using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
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

    [SerializeField]
    public List<Skin> Skins;
    
    public static GameManager Instance { get; private set; } // static singleton

    private readonly Assets.Scripts.EventHandling.EventHandler<CollisionHappenedEvent> _collisionHandler = new Assets.Scripts.EventHandling.EventHandler<CollisionHappenedEvent>();

    public bool GameRunning => Time.timeScale > 0;
    public bool GameStarted;
    public bool GameRunningAndStarted => GameRunning && GameStarted;

    void Awake()
    {
        Time.timeScale = 1;
        _startTime = Time.time;
        if (Instance == null) { Instance = this; }

        var currentSkinName = PlayerPrefs.GetString("equipped_skin_name", "Boy");
        var currentSkin = Skins.Single(s => s.Name == currentSkinName);
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
        GameStarted = true;
        SpawnManager.Instance.StartSpawning();
    }

    public void OpenSkinStore()
    {
        SceneManager.LoadScene(1);
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
            GameStarted = false;
        }
    }

    public void RestartGame()
    {
        EventManager.DisposeAllHandlers();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
