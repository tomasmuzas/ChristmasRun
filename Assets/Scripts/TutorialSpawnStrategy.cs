using System.Collections;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Events;
using Assets.Scripts.Managers;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    public class TutorialSpawnStrategy : MonoBehaviour, ISpawnStrategy
    {
        private bool _hasMovedRight;
        private bool _hasMovedLeft;
        private bool _hasJumped;
        private bool _hasCollectedValuable;
        private bool _wasObstacleSpawned;

        private readonly EventHandler<PlayerJumpedEvent> jumpEventHandler = new EventHandler<PlayerJumpedEvent>();
        private readonly EventHandler<PlayerMovedLeftEvent> moveLeftEventHandler = new EventHandler<PlayerMovedLeftEvent>();
        private readonly EventHandler<PlayerMovedRightEvent> moveRightEventHandler = new EventHandler<PlayerMovedRightEvent>();
        private readonly EventHandler<GiftCollectedEvent> giftCollectedHandler = new EventHandler<GiftCollectedEvent>();

        private readonly Random Rnd = new Random();

        public void StartSpawning()
        {
            EventManager.PublishEvent(new TutorialStartedEvent());
            CanvasManager.Instance.DisplayTutorialText("Swipe right to move right");
            moveLeftEventHandler.EventAction += HandleMoveLeft;
            moveRightEventHandler.EventAction += HandleMoveRight;
            jumpEventHandler.EventAction += HandleJump;
            giftCollectedHandler.EventAction += HandleGiftCollected;
        }

        private void HandleMoveRight(IEvent @event)
        {
            if (!_hasMovedRight)
            {
                CanvasManager.Instance.DisplayTutorialText("Swipe left to move left");
                _hasMovedRight = true;
            }
        }

        private void HandleMoveLeft(IEvent @event)
        {
            if (!_hasMovedLeft && _hasMovedRight)
            {
                CanvasManager.Instance.DisplayTutorialText("Swipe up to jump");
                _hasMovedLeft = true;
            }
        }

        private void HandleJump(IEvent @event)
        {
            if (!_hasJumped && _hasMovedRight && _hasMovedLeft)
            {
                CanvasManager.Instance.DisplayTutorialText("Collecting gifts or candies is good!\nCollect one!");
                _hasJumped = true;
                StartCoroutine(StartSpawningValuables());
            }
        }

        private void HandleGiftCollected(IEvent @event)
        {
            if (!_hasCollectedValuable && _hasJumped && _hasMovedRight && _hasMovedLeft)
            {
                _hasCollectedValuable = true;
                CanvasManager.Instance.DisplayTutorialText("Avoid trees, reindeers and snowballs!");
                SpawnObstacle();
            }
        }

        #region Helper methods

        private void SpawnObstacle()
        {
            var lane = (Lane)Rnd.Next(0, 3);
            var spawnable = SpawnManager.PickSpawnableBasedOnChance(SpawnableGroup.Obstacle, SpawnType.NonProbabilistic);
            SpawnManager.SpawnItem(lane, spawnable);
            _wasObstacleSpawned = true;
            StartCoroutine(WaitForNoCollision());
        }

        private IEnumerator WaitForNoCollision()
        {
            if (_wasObstacleSpawned && _hasCollectedValuable && _hasJumped && _hasMovedLeft && _hasMovedRight)
            {
                yield return new WaitForSeconds(5);
                CanvasManager.Instance.DisplayTutorialText("Great job! Now you are ready to play!");
                yield return new WaitForSeconds(2);
                CanvasManager.Instance.DisplayTutorialText(string.Empty);
                EventManager.PublishEvent(new TutorialFinishedEvent());
            }
        }

        private IEnumerator StartSpawningValuables()
        {
            while (!_hasCollectedValuable)
            {
                var valuableLane = (Lane)Rnd.Next(0, 3);
                var spawnable = SpawnManager.PickSpawnableBasedOnChance(SpawnableGroup.Valuable, SpawnType.NonProbabilistic);
                SpawnManager.SpawnItem(valuableLane, spawnable);

                yield return new WaitForSeconds(3);
            }
        }

        #endregion
    }
}
