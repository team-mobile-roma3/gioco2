using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class MoveTouch : MonoBehaviour
{

    public Transform player;


    public Transform circle;
    public Transform outerCircle;

    private Vector2 startingPoint;
    private int leftTouch = 99;


    // Update is called once per frame
    void Update()
    {
        int i = 0;
        while (i < Input.touchCount)
        {
            Touch t = Input.GetTouch(i);
            Vector2 touchPos = getTouchPosition(t.position); // * -1 for perspective cameras
            if (t.phase == TouchPhase.Began)
            {
                leftTouch = t.fingerId;
                startingPoint = touchPos;

            }
            else if (t.phase == TouchPhase.Moved )
            {
                Vector2 offset = touchPos - startingPoint;
                Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
                if (t.position.x > Screen.width / 2)
                {
                    player.GetComponent<PlayerController>().PlayerAttack(direction.x, direction.y);
                }
                else
                {
          //          player.GetComponent<PlayerController>().Move(direction.x, direction.y);
                  //  moveCharacter(direction);
                }
              

                circle.transform.position = new Vector2(outerCircle.transform.position.x + direction.x, outerCircle.transform.position.y + direction.y);

            }
            else if (t.phase == TouchPhase.Ended )
            {
       //         Debug.Log(startingPoint + " AO " + touchPos);
            /*    if (startingPoint == touchPos)
                {

                    if (t.position.x > Screen.width / 2)
                    {
                       
                        Boolean stance = GameController.Stance;
   //                     Debug.Log("cambio stance " + stance);
                        GameController.Stance = !stance;
                    }
                    else
                    {
   //                     Debug.Log(startingPoint.x + " " + Screen.width / 2);
                        Inventory.PotionUse();
                    }
                }*/
                leftTouch = 99;
                circle.transform.position = new Vector2(outerCircle.transform.position.x, outerCircle.transform.position.y);
            }
            ++i;
        }

    }
    Vector2 getTouchPosition(Vector2 touchPosition)
    {
        return GetComponent<Camera>().ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, transform.position.z));
    }

    void moveCharacter(Vector2 direction)
    {
        player.Translate(direction * GameController.MoveSpeed * Time.fixedDeltaTime);
    }
   
}
