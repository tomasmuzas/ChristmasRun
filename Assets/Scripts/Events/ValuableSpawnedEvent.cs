using Assets.Scripts.Behaviour;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Events
{
    public class ValuableSpawnedEvent : IEvent
    {
        public ValuableSpawnedEvent(Valuable valuable)
        {
            Valuable = valuable;
        }

        public Valuable Valuable { get; }
    }
}
