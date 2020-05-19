using System;
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


    // Start is called before the first frame update
    public void Drop(Vector2 pos)
    {
        totalWeight = 0;
        Debug.Log("total w " + totalWeight);
        foreach (var spawnable in items)
        {
            Debug.Log("2total w " + totalWeight);
            totalWeight += spawnable.weight;
        }

        float pick = UnityEngine.Random.value * totalWeight;
        int chosenIndex = 0;
        float cumulativeWeight = items[0].weight;
        Debug.Log("pick " + pick + "index " + chosenIndex);
        while (pick > cumulativeWeight && chosenIndex < items.Count - 1)
        {
            Debug.Log("2pick " + pick + "index " + chosenIndex);
            chosenIndex++;
            cumulativeWeight += items[chosenIndex].weight;
        }
        
         if(chosenIndex != (items.Count -1))
            i = Instantiate(items[chosenIndex].gameObject, pos, Quaternion.identity) as GameObject;
     

    }

}
