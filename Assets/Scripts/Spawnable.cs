using System;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

[Serializable]
public class Spawnable : MonoBehaviour
{
    [SerializeField]
    public List<SpawnPosition> SpawnPositions;
}