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
    public enum SpawnableGroup
    {
        Valuable,
        Obstacle
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

        private static readonly Random Rnd = new Random();

        public ISpawnStrategy spawnStrategy;

        private EventHandler<TutorialFinishedEvent> tutorialEventHandler = null;

        void Awake()
        {
            if (Instance == null) { Instance = this; }

            valuableSpawnables = SpawnablePrefabs
                .Where(s => s.GetComponent<Valuable>() != null || s.GetComponent<PowerUpPickup>() != null)
                .OrderBy(s => s.SpawnChance)
                .ToList();
            obstacleSpawnables = SpawnablePrefabs
                .Where(s => s.GetComponent<Destructable>() != null)
                .OrderBy(s => s.SpawnChance)
                .ToList();


            if (PlayerPrefs.GetInt(PlayerPrefKeys.Tutorial, 0) == 0)
            {
                spawnStrategy = CreateSpawnStrategy<TutorialSpawnStrategy>();
                tutorialEventHandler = new EventHandler<TutorialFinishedEvent>();
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
            var possibleSpawnables = spawnableGroup == SpawnableGroup.Valuable ? valuableSpawnables : obstacleSpawnables;
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
            while (GameManager.Instance.GameRunning)
            {
                yield return new WaitForSeconds(0.8f / (GameManager.GameSpeed * 100));
                if (HousePrefabs?.Count > 0)
                {
                    var house = HousePrefabs[Rnd.Next(0, HousePrefabs.Count)];
                    Instantiate(house, new Vector3(-1.204675f, 1.139f, 4.5501f), Quaternion.Euler(0f, 13.479f, 0f));
                    house = HousePrefabs[Rnd.Next(0, HousePrefabs.Count)];
                    Instantiate(house, new Vector3(1.23f, 1.139f, 4.91f), Quaternion.Euler(0f, 160f, 0f));
                }
            }
        }
    }
}
