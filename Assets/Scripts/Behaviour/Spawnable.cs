using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    [Serializable]
    public class Spawnable : MonoBehaviour
    {
        [SerializeField]
        public List<SpawnPosition> SpawnPositions;
    }
}