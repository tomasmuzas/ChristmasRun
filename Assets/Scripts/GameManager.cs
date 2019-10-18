using Assets.Scripts.EventHandling;
using Assets.Scripts.Events;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public float InitialGameSpeed;
    public float SpawnSpeed;
    public static float GameSpeed;
    public Spawnable SpawnablePrefab;
    public GameObject GameOverText;
    private static readonly Random Rnd = new Random();
    private readonly EventHandler<CollisionHappenedEvent> _collisionHandler = new EventHandler<CollisionHappenedEvent>();

    // Start is called before the first frame update
    void Start()
    {
        _collisionHandler.EventAction += HandleCollisionHappened;
        GameSpeed = InitialGameSpeed;
        InvokeRepeating(nameof(SpawnObject), SpawnSpeed, SpawnSpeed);
    }

    void SpawnObject()
    {
        if (SpawnablePrefab?.SpawnPositions?.Count > 0)
        {
            var location = SpawnablePrefab.SpawnPositions[Rnd.Next(0, SpawnablePrefab.SpawnPositions.Count)];
            Instantiate(SpawnablePrefab, location.Position, Quaternion.identity);
        }
    }

    private void HandleCollisionHappened(CollisionHappenedEvent @event)
    {
        CancelInvoke(nameof(SpawnObject));
        GameOverText.SetActive(true);
        Time.timeScale = 0;
    }
}
