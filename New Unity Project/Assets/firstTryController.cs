using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstTryController : MonoBehaviour
{
    public GameObject player;
    private Touch touch;
    private Vector2 begin, end; 
    void Update()
    {
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            switch (touch.phase) 
            {
                case TouchPhase.Began:
                begin = touch.position;
                break;

                case TouchPhase.Ended:
                end = touch.position;

                if (begin == end) 
                {

                }
                if (begin != end)
                {
                    player.GetComponent<Rigidbody2D>().velocity = end - begin;
                }
                break;
            }
        }
        
    }
}
