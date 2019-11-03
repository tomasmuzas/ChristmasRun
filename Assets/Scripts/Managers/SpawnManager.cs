using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Behaviour;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Managers
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField]
        public List<Spawnable> SpawnablePrefabs;
        [SerializeField]
        public List<GameObject> HousePrefabs;

        public static float SpawnSpeed => InitialSpawnSpeed / GameManager.GameSpeed;
        public static float InitialSpawnSpeed = 0.7f;

        private static readonly Random Rnd = new Random();
        public static SpawnManager Instance { get; private set; } // static singleton

        private List<Spawnable> valuableSpawnables;
        private List<Spawnable> obstacleSpawnables;
        private Lane _valuableLane;

        void Awake()
        {
            if (Instance == null) { Instance = this; }

            _valuableLane = Lane.Middle;
            valuableSpawnables = SpawnablePrefabs
                .Where(s => s.GetComponent<Valuable>() != null || s.GetComponent<PowerUpPickup>() != null)
                .OrderBy(s => s.SpawnChance)
                .ToList();
            obstacleSpawnables = SpawnablePrefabs
                .Where(s => s.GetComponent<Destructable>() != null)
                .OrderBy(s => s.SpawnChance)
                .ToList();
        }

        void Start()
        {
            StartCoroutine(SpawnHouses());
        }

        private IEnumerator SpawnLane(Lane lane)
        {
            if (SpawnablePrefabs?.Count == 0)
            {
                yield break;
            }

            while (GameManager.Instance.GameRunning)
            {
                var spawnable = PickSpawnableBasedOnChance(lane == _valuableLane ? valuableSpawnables : obstacleSpawnables);

                if (spawnable != null)
                {
                    var spawnPosition = spawnable.SpawnPositions.Single(p => p.Lane == lane);
                    Instantiate(spawnable, spawnPosition.Position, spawnable.transform.rotation);
                }

                yield return new WaitForSeconds(lane == _valuableLane ? SpawnSpeed : 1/SpawnSpeed);
            }
        }

        private Spawnable PickSpawnableBasedOnChance(List<Spawnable> possibleSpawnables)
        {
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

        public void StartSpawning()
        {
            StartCoroutine(SpawnLane(Lane.Left));
            StartCoroutine(SpawnLane(Lane.Middle));
            StartCoroutine(SpawnLane(Lane.Right));
            StartCoroutine(ChangeValuableLane());
        }

        private IEnumerator ChangeValuableLane()
        {
            while (GameManager.Instance.GameRunning)
            {
                _valuableLane = (Lane)Rnd.Next(0, 2 + 1);
                Debug.Log(_valuableLane);
                yield return new WaitForSeconds(3 / GameManager.GameSpeed);
            }
        }
    }
}
