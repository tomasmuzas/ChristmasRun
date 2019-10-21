using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Events
{
    public class GiftCollectedEvent : IEvent
    {
        public int Value { get; set; }
    }
}
