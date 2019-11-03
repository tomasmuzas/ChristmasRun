using Assets.Scripts.Events;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class Valuable : MonoBehaviour
    {
        void Start()
        {
            EventManager.PublishEvent(new ValuableSpawnedEvent(this));
        }

        public int Value;
    }
}
