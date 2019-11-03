using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Events;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField]
    public List<PowerUpMap> PowerUpMap;
    private readonly EventHandler<CollisionHappenedEvent> _collisionHandler = new EventHandler<CollisionHappenedEvent>();

    // Start is called before the first frame update
    void Start()
    {
        _collisionHandler.EventAction += HandleCollisionHappened;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleCollisionHappened(CollisionHappenedEvent @event)
    {
        var collidedObjectName = @event.CollidedWith.gameObject.name;
        if (@event.Object.GetComponent<MainCharacter>() && PowerUpMap.Any(x => collidedObjectName.StartsWith(x.PowerUpPickup.name)))
        {
            var powerUp = PowerUpMap.Single(x => collidedObjectName.StartsWith(x.PowerUpPickup.name));
            powerUp.PowerUpSpawn.Activate();
            DestroyPowerUpSpawn(@event);
        }
    }

    void DestroyPowerUpSpawn(CollisionHappenedEvent @event)
    {
        var smoothDestroy = @event.CollidedWith.GetComponent<SmoothDestroy>();
        if (smoothDestroy)
        {
            smoothDestroy.StartDestroy();
        }
        else
        {
            Destroy(@event.CollidedWith.gameObject);
        }
    }
}
