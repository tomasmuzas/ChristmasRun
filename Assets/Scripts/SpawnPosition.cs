using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class SpawnPosition
    {
        [SerializeField]
        public Lane Lane;
        [SerializeField]
        public Vector3 Position;
    }
}
