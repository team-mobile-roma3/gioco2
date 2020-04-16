using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public FloatingJoystick floatingJoystick;
    public GameObject player;
/*
    public void FixedUpdate()
    {
        float horizontal = floatingJoystick.Horizontal;
        float vertical = floatingJoystick.Vertical;
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(horizontal*speed,vertical*speed);
    }
*/}