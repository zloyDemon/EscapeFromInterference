﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class DungGeneration : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("DungGeneration Awake");
        DungeonManager.Instance.LevelLoaded += () =>
        {
            Debug.Log("Level loaded");
            Debug.Log("Count: " + DungeonManager.Instance.AvailablePosition.Count);
        };
        
    }
}
