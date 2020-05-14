﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnDeathSpawner : MonoBehaviour
{
    [Serializable]
    public struct Spawnable
    {
        public GameObject gameObject;
        public float weight;
    }

    public List<Spawnable> items = new List<Spawnable>();
    float totalWeight;

    private GameObject i;

    void Awake()
    {
        totalWeight = 0;
        foreach (var spawnable in items)
        {
            totalWeight += spawnable.weight;
        }
    }

    // Start is called before the first frame update
    public void Drop(Vector2 pos)
    {

        float pick = UnityEngine.Random.value * totalWeight;
        int chosenIndex = 0;
        float cumulativeWeight = items[0].weight;

        while (pick > cumulativeWeight && chosenIndex < items.Count - 1)
        {
            chosenIndex++;
            cumulativeWeight += items[chosenIndex].weight;
        }
        
        if (chosenIndex != items.Count)
        {
  
            i = Instantiate(items[chosenIndex].gameObject, pos, Quaternion.identity) as GameObject;
        }

    }

}
