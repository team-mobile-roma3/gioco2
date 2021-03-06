﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour
{
    
    private GameObject player;    
    public float lastOffsetX;
    public float lastOffsetY;
    //public float legSpec;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, 
        new Vector2(player.transform.position.x - lastOffsetX, player.transform.position.y - lastOffsetY), player.GetComponent<PlayerController>().speed * Time.deltaTime*0.975f);
    }
}
