using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Managers;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    public class MainSpawnStrategy : MonoBehaviour, ISpawnStrategy
    {
        private Lane _valuableLane = Lane.Middle;
        private static readonly Random Rnd = new Random();

        public static float SpawnSpeed => InitialSpawnSpeed / GameManager.Instance.GameSpeed;
        public static float InitialSpawnSpeed = 0.7f;

        public void StartSpawning()
        { 
            Console.WriteLine(this);
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
                yield return new WaitForSeconds(3 / GameManager.Instance.GameSpeed);
            }
        }

        private IEnumerator SpawnLane(Lane lane)
        {
            while (GameManager.Instance.GameRunning)
            {
                var spawnable = SpawnManager.PickSpawnableBasedOnChance(lane == _valuableLane ? SpawnableGroup.Valuable : SpawnableGroup.Obstacle);
                SpawnManager.SpawnItem(lane, spawnable);

                yield return new WaitForSeconds(lane == _valuableLane ? SpawnSpeed : 1 / SpawnSpeed);
            }
        }
    }
}
