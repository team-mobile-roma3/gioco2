﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;
    public Room currRoom;
    public float moveSpeedWhenRoomChange;
    public bool tutorial;



    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorial)
        {
            transform.position = Vector3.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0,0,-10) ,Time.deltaTime * 300);
         
        }
        else  UpdatePosition();
    }

    void UpdatePosition()
    {
        if(currRoom == null)
        {
            return;
        }
       

        Vector3 targetPos = GetCameraTargetPosition();

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeedWhenRoomChange);
    }

    Vector3 GetCameraTargetPosition()
    {
        if(currRoom == null)
        {
            return Vector3.zero;
        }

        Vector3 targetPos = currRoom.GetRoomCentre();
        targetPos.z = transform.position.z;

        return targetPos;
    }

    public bool IsSwitchingScene()
    {
        return transform.position.Equals( GetCameraTargetPosition()) == false;
    }
}
