using System;
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

        public static float SpawnSpeed;
        public static float InitialSpawnSpeed = 0.8f;

        private static readonly Random Rnd = new Random();
        public static SpawnManager Instance { get; private set; } // static singleton

        void Awake()
        {
            if (Instance == null) { Instance = this; }
            else { Destroy(gameObject); }

            SpawnablePrefabs = SpawnablePrefabs.OrderBy(p => p.SpawnChance).ToList();
        }

        void Start()
        {
            SpawnSpeed = InitialSpawnSpeed;
            StartCoroutine(SpawnHouses());
        }

        private IEnumerator SpawnObject()
        {
            if (SpawnablePrefabs?.Count == 0)
            {
                yield break;
            }

            while (GameManager.Instance.GameRunning)
            {
                var spawnCounts = new Dictionary<Type, int>();
                foreach (var spawnablePrefab in SpawnablePrefabs)
                {
                    spawnCounts[spawnablePrefab.GetType()] = 0;
                }

                var numberOfItems = Rnd.Next(1, 3 + 1); // Random number between 1 and 3
                Debug.Log(numberOfItems);
                var currentLane = (Lane) Rnd.Next(0, numberOfItems == 3 ? 0 + 1 : 1 + 1); // Start spawning from either 1st or 2nd lane

                while (numberOfItems-- > 0)
                {
                    var spawnableItem = GetSpawnableItem(spawnCounts);
                    if (spawnableItem?.SpawnPositions?.Count > 0)
                    {
                        var location = spawnableItem.SpawnPositions.SingleOrDefault(s => s.Lane == currentLane);
                        if (location != null)
                        {
                            Instantiate(spawnableItem, location.Position, Quaternion.identity);
                            currentLane++;
                        }
                    }
                }

                yield return new WaitForSeconds(SpawnSpeed);
            }
        }

        private Spawnable GetSpawnableItem(Dictionary<Type, int> spawnCounts)
        {
            double diceRoll = Rnd.NextDouble();

            double cumulative = 0.0;
            for (int i = 0; i < SpawnablePrefabs.Count; i++)
            {
                cumulative += SpawnablePrefabs[i].SpawnChance;
                if (diceRoll < cumulative)
                {
                    var spawnedItem = SpawnablePrefabs[i];
                    if (spawnCounts[spawnedItem.GetType()] < spawnedItem.AllowedInstancesPerSpawn)
                    {
                        return spawnedItem;
                    }
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
            StartCoroutine(SpawnObject());
        }
    }
}
