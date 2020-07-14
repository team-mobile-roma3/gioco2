using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSItemSpawner : MonoBehaviour
{
    public GameObject item;
    private GameObject toSpawn;
    public string name;

    private Boolean cooldown = false;
    void Start()
    {
        
    }

    private void Update()
    {
        if (!cooldown && GameObject.Find(name+"(Clone)") == null)
        {

            toSpawn = Instantiate(item.gameObject, transform.position, Quaternion.identity) as GameObject;
            StartCoroutine(Spawn());
        }


    }

    private  IEnumerator Spawn()
    {
         cooldown = true;
        while (GameObject.Find(name + "(Clone)") != null)
        {
            Debug.Log(this + " aspetto");
            yield return new WaitForSeconds(20f);
        }
 
        cooldown = false;
      
    }

}

