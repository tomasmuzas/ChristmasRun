using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Events;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject GroundDustParticles;
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
    }
}
