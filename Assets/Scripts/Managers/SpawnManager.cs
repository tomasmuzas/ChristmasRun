using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Behaviour;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Events;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Managers
{
    [Flags]
    public enum SpawnableGroup
    {
        Valuable = 1,
        Obstacle = 2,
        PowerUp = 4
    }

    public enum SpawnType
    {
        Probabilistic,
        NonProbabilistic
    }

    public class SpawnManager : MonoBehaviour
    {
        [SerializeField]
        public List<Spawnable> SpawnablePrefabs;
        [SerializeField]
        public List<GameObject> HousePrefabs;

        public static SpawnManager Instance { get; private set; } // static singleton

        private static List<Spawnable> valuableSpawnables;
        private static List<Spawnable> obstacleSpawnables;
        private static List<Spawnable> powerupSpawnables;

        private static readonly Random Rnd = new Random();

        public ISpawnStrategy spawnStrategy;

        private EventHandling.EventHandler<TutorialFinishedEvent> tutorialEventHandler = null;

        void Awake()
        {
            if (Instance == null) { Instance = this; }

            valuableSpawnables = SpawnablePrefabs
                .Where(s => s.GetComponent<Valuable>() != null)
                .ToList();
            obstacleSpawnables = SpawnablePrefabs
                .Where(s => s.GetComponent<Destructable>() != null)
                .ToList();
            powerupSpawnables = SpawnablePrefabs
                .Where(s => s.GetComponent<PowerUpPickup>() != null)
                .ToList();


            if (PlayerPrefs.GetInt(PlayerPrefKeys.Tutorial, 0) == 0)
            {
                spawnStrategy = CreateSpawnStrategy<TutorialSpawnStrategy>();
                tutorialEventHandler = new EventHandling.EventHandler<TutorialFinishedEvent>();
                tutorialEventHandler.EventAction += HandleTutorialFinished;
            }
            else
            {
                spawnStrategy = CreateSpawnStrategy<MainSpawnStrategy>();
            }
        }

        void Start()
        {
            StartCoroutine(SpawnHouses());
        }

        private void HandleTutorialFinished(IEvent @event)
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.Tutorial, 1);
            spawnStrategy = CreateSpawnStrategy<MainSpawnStrategy>();
            spawnStrategy.StartSpawning();
        }

        private ISpawnStrategy CreateSpawnStrategy<T>() where T: MonoBehaviour, ISpawnStrategy
        {
            // This is a very shady method. In order to use Coroutines, we need to have MonoBehaviour
            // MonoBehaviour can only be initialized properly via GameObject methods.
            // We are adding a component of the strategy, then getting it again...
            return new GameObject(typeof(T).Name).AddComponent<T>().GetComponent<T>();
        }

        public static Spawnable PickSpawnableBasedOnChance(SpawnableGroup spawnableGroup, SpawnType spawnType = SpawnType.Probabilistic)
        {
            var possibleSpawnables = new List<Spawnable>();

            if (spawnableGroup.HasFlag(SpawnableGroup.Valuable))
            {
                possibleSpawnables.AddRange(valuableSpawnables);
            }
            if (spawnableGroup.HasFlag(SpawnableGroup.PowerUp))
            {
                possibleSpawnables.AddRange(powerupSpawnables);
            }
            if (spawnableGroup.HasFlag(SpawnableGroup.Obstacle))
            {
                possibleSpawnables.AddRange(obstacleSpawnables);
            }

            possibleSpawnables = possibleSpawnables.OrderBy(s => s.SpawnChance).ToList();

            if (spawnType == SpawnType.NonProbabilistic)
            {
                return possibleSpawnables[Rnd.Next(0, possibleSpawnables.Count)];
            }

            var randomNumber = Rnd.NextDouble();

            var cumulative = 0.0;
            for (var i = 0; i < possibleSpawnables.Count; i++)
            {
                cumulative += possibleSpawnables[i].SpawnChance;
                if (randomNumber < cumulative)
                {
                    return possibleSpawnables[i];
                }
            }

            return null;
        }

        public static void SpawnItem(Lane lane, Spawnable spawnable)
        {
            if (spawnable != null)
            {
                var spawnPosition = spawnable.SpawnPositions.Single(p => p.Lane == lane);
                Instantiate(spawnable, spawnPosition.Position, spawnable.transform.rotation);
            }
        }

        private IEnumerator SpawnHouses()
        {
            yield return new WaitForSeconds(5);
            while (GameManager.Instance.GameRunning)
            {
                yield return new WaitForSeconds(1.4f / GameManager.Instance.GameSpeed);
                if (HousePrefabs?.Count > 0)
                {
                    var house = HousePrefabs[Rnd.Next(0, HousePrefabs.Count)];
                    Instantiate(house, new Vector3(-1.2f, 0.362f, 4.61f), Quaternion.Euler(0f, 103.3f, 0f));
                    house = HousePrefabs[Rnd.Next(0, HousePrefabs.Count)];
                    Instantiate(house, new Vector3(1.2f, 0.362f, 4.342001f), Quaternion.Euler(0f, 257.97f, 0f));
                }
            }
        }
    }
}
