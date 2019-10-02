﻿using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References:")]
    public Logger logger;
    public Scatter scatter;
    public Target target;
    public GameObject hand;

    public GameObject snowball;

    //poisson disk spawning
    public float radius = 1;
    public Vector2 regionSize = new Vector2(15, 15);
    List<Vector2> spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = PoissonDiskSpawn.GenerateSpawns(radius, regionSize);
        foreach (Vector2 point in spawnPoints)
        {
            Instantiate(snowball, new Vector3(-7 + point.x, 0.2f, -3 + point.y), Quaternion.identity);
        }
    }

    public string GetTimeStamp()
    {
        return DateTime.UtcNow.ToString("HH:mm:ss:fff");
    }
}
