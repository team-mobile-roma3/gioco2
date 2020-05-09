using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour
{
    
    private GameObject player;    
    public float lastOffsetX;
    public float lastOffsetY;
    
    public bool joyMove;
    public FloatingJoystick move;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        float horizontal, vertical;
        if (joyMove)
        {
            horizontal = move.Horizontal;
            vertical = move.Vertical;    
        }
        else
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical =Input.GetAxis("Vertical");
        }
        


        /*if(horizontal != 0 || vertical != 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position , player.GetComponent<PlayerController>().speed * Time.deltaTime* 0.9f);
        }
        else*/
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x - lastOffsetX,
                                                                                     player.transform.position.y - lastOffsetY),
                                                                                     player.GetComponent<PlayerController>().speed * Time.deltaTime*0.9f);
            
        }
    }
}
