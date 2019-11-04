using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Behaviour;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Events;
using UnityEngine;

public class MagnetPowerUp : PowerUpSpawn
{
    public float EffectRadius = 1.5f;
    public float EffectSpeed = 10;
    private EventHandler<ValuableSpawnedEvent> _valuableSpawnedHandler = new EventHandler<ValuableSpawnedEvent>();
    private List<Valuable> _valuablesNotAffected;
    private List<Valuable> _valuablesAffected = new List<Valuable>();

    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        _valuablesNotAffected = FindObjectsOfType<Valuable>().Cast<Valuable>().ToList();
        _valuableSpawnedHandler.EventAction += AddNewValuable;
        InvokeRepeating(nameof(CleanUpValuables), 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            GameManager.Instance.MainCharacter.transform.position.x,
            transform.position.y,
            transform.position.z);

        UpdateAffectedValuables();
        MoveAffectedValuablesTowardsThisObject();
    }

    private void UpdateAffectedValuables()
    {
        var valuablesThatChangedState = new List<Valuable>();
        foreach(var valuable in _valuablesNotAffected)
        {
            if (valuable != null)
            {
                if (Vector2DDistance(transform.position, valuable.transform.position) <= EffectRadius)
                {
                    _valuablesAffected.Add(valuable);
                    valuablesThatChangedState.Add(valuable);
                }
            }
        }
        valuablesThatChangedState.ForEach(x => _valuablesNotAffected.Remove(x));
    }

    private void MoveAffectedValuablesTowardsThisObject()
    {
        foreach(var valuable in _valuablesAffected)
        {
            if (valuable != null)
            {
                valuable.transform.position = Vector3.Lerp(valuable.transform.position, transform.position, EffectSpeed * Time.deltaTime);
            }
        }
    }

    //TODO: probably there is a better way to do this, just can't think of it
    private void CleanUpValuables()
    {
        _valuablesAffected = _valuablesAffected.Where(x => x != null).ToList();
        _valuablesNotAffected = _valuablesNotAffected.Where(x => x != null).ToList();
    }

    private void AddNewValuable(ValuableSpawnedEvent @event)
    {
        _valuablesNotAffected.Add(@event.Valuable);
    }

    void OnTriggerEnter(Collider other)
    {
        var valuable = other.GetComponent<Valuable>();
        if (valuable)
        {
            EventManager.PublishEvent(new CollisionHappenedEvent
            {
                Object = GameManager.Instance.MainCharacter.GetComponent<Collider>(),
                CollidedWith = other
            });
        }
    }

    public override GameObject Activate()
    {
        return Instantiate(gameObject);
    }

    private float Vector2DDistance(Vector3 v1, Vector3 v2)
    {
        float xDiff = v1.x - v2.x;
        float zDiff = v1.z - v2.z;
        return Mathf.Sqrt((xDiff * xDiff) + (zDiff * zDiff));
    }
}
