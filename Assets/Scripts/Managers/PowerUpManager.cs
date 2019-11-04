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
    private List<GameObject> _activePowerUpInstances = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _collisionHandler.EventAction += HandleCollisionHappened;
        InvokeRepeating(nameof(CleanUpInstances), 1, 1);
    }

    void HandleCollisionHappened(CollisionHappenedEvent @event)
    {
        var collidedObjectName = @event.CollidedWith.gameObject.name;
        if (@event.Object.GetComponent<MainCharacter>() && PowerUpMap.Any(x => collidedObjectName.StartsWith(x.PowerUpPickup.name)))
        {
            var powerUp = PowerUpMap.Single(x => collidedObjectName.StartsWith(x.PowerUpPickup.name));

            var alreadyActivePowerup = _activePowerUpInstances.FirstOrDefault(x => x != null && x.name.StartsWith(powerUp.PowerUpSpawn.name));
            Debug.Log(_activePowerUpInstances.Count);
            var alreadyActivePowerupDestroySelf = alreadyActivePowerup?.GetComponent<DestroySelf>();
            if (alreadyActivePowerup && alreadyActivePowerupDestroySelf)
            {
                alreadyActivePowerup.GetComponent<DestroySelf>().ResetDestructionTime();
            }
            else
            {
                var powerUpInstance = powerUp.PowerUpSpawn.Activate();
                _activePowerUpInstances.Add(powerUpInstance);
            }

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

    private void CleanUpInstances()
    {
        _activePowerUpInstances = _activePowerUpInstances.Where(x => x != null).ToList();
    }
}
