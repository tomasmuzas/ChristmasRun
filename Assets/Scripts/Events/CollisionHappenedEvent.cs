using Assets.Scripts.EventHandling;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public class CollisionHappenedEvent : IEvent
    {
        public Collider Object { get; set; }

        public Collider CollidedWith { get; set; }
    }
}
