using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSItemSpawner : MonoBehaviour
{
    public GameObject item;
    // Start is called before the first frame update
    void Start()
    {
        item = Instantiate(item.gameObject, new Vector2(0, 220), Quaternion.identity) as GameObject;
    }


}
