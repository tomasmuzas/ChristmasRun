using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Behaviour;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Events;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject GroundDustParticles;
    public GameObject PickupEffectParticle;
    private bool _fisrtGroundCollisionHappened = false;
    private readonly EventHandler<CollisionHappenedEvent> _collisionHandler = new EventHandler<CollisionHappenedEvent>();

    // Start is called before the first frame update
    void Start()
    {
        _collisionHandler.EventAction += HandleCollisionHappened;
    }

    void HandleCollisionHappened(CollisionHappenedEvent @event)
    {
        if (@event.CollidedWith.GetComponent<Ground>() && @event.Object.GetComponent<MainCharacter>())
        {
            if (_fisrtGroundCollisionHappened)
            {
                var position = new Vector3(@event.Object.transform.position.x, @event.CollidedWith.transform.position.y,
                    @event.Object.transform.position.z - 0.1f);
                Instantiate(GroundDustParticles, position, Quaternion.identity);
            }
            else
            {
                _fisrtGroundCollisionHappened = true;
            }
        }

        if (@event.CollidedWith.GetComponent<Valuable>() != null && @event.Object.GetComponent<MainCharacter>())
        {
            var position = new Vector3(@event.CollidedWith.transform.position.x, @event.CollidedWith.transform.position.y,
                @event.CollidedWith.transform.position.z);
            Instantiate(PickupEffectParticle, position, Quaternion.identity);
        }
    }
}
