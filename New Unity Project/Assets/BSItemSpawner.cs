using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSItemSpawner : MonoBehaviour
{
    public GameObject item;

    private Boolean cooldown = false;
    void Start()
    {
        
    }

    private void Update()
    {
        if (!cooldown && GameObject.Find("BSPotion(Clone)") == null)
        {

            item = Instantiate(item.gameObject, new Vector2(0, 220), Quaternion.identity) as GameObject;
            StartCoroutine(Spawn());
        }


    }

    private  IEnumerator Spawn()
    {
         cooldown = true;
        yield return new WaitForSeconds(10f);
        cooldown = false;
      
    }

}

