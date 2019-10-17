﻿using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public float InitialGameSpeed;
    public float SpawnSpeed;
    public static float GameSpeed;
    public Spawnable SpawnablePrefab;
    public static Random Rnd = new Random();

    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnObjectCollision += StopGame;
        GameSpeed = InitialGameSpeed;
        InvokeRepeating(nameof(SpawnObject), SpawnSpeed, SpawnSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable()
    {
        EventManager.OnObjectCollision -= StopGame;
    }

    void SpawnObject()
    {
        if (SpawnablePrefab?.SpawnPositions?.Count > 0)
        {
            var location = SpawnablePrefab.SpawnPositions[Rnd.Next(0, SpawnablePrefab.SpawnPositions.Count)];
            Instantiate(SpawnablePrefab, location.Position, Quaternion.identity);
        }
    }

    void StopGame()
    {
        CancelInvoke(nameof(SpawnObject));
    }
}
