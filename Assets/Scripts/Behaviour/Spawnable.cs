using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    [Serializable]
    public class Spawnable : MonoBehaviour
    {
        public int AllowedInstancesPerSpawn;

        public float SpawnChance;

        [SerializeField]
        public List<SpawnPosition> SpawnPositions;
    }
}