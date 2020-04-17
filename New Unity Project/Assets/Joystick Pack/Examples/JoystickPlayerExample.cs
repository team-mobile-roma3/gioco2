using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public FloatingJoystick floatingJoystick;
    public GameObject player;
    public bool joyMove;
    public void FixedUpdate()
    {
        float horizontal, vertical;
        if (joyMove)
        {
            horizontal = floatingJoystick.Horizontal;
            vertical = floatingJoystick.Vertical;    
        }
        else
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical =Input.GetAxis("Vertical");
        }
        
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(horizontal * speed, vertical * speed);
    }
}