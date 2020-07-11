using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType
    {
        left, right, top, bottom
    }

    public DoorType doorType;
    
    public GameObject doorCollider;

    private GameObject player;
    private float widthOffset = 2.0f;
    private bool notConnected = false;

    public bool getNotConnected()
    {
        return notConnected;
    }
    public void setNotConnected(bool val)
    {
        notConnected = val;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && Time.time > PlayerController.AbleTeleportDoor + 0.2f)
        {
            switch (doorType)
            {
                case DoorType.bottom:
                    if (this.GetComponentInChildren<SpriteRenderer>().enabled)
                    {
                        player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y - widthOffset);
                        PlayerController.AbleTeleportDoor = Time.time;
                        Debug.Log("passato sotto");
                    }

                    break;
                case DoorType.left:
                    if (this.GetComponentInChildren<SpriteRenderer>().enabled)
                    {  
                        player.transform.position = new Vector2(player.transform.position.x - widthOffset, player.transform.position.y);
                        PlayerController.AbleTeleportDoor = Time.time;
                        Debug.Log("passato sinistra");
                    }

                    break;
                case DoorType.right:
                    if (this.GetComponentInChildren<SpriteRenderer>().enabled)
                    {
                        player.transform.position = new Vector2(player.transform.position.x + widthOffset, player.transform.position.y);
                        PlayerController.AbleTeleportDoor = Time.time;
                        Debug.Log("passato destra");
                    }

                    break;
                case DoorType.top:
                    if (this.GetComponentInChildren<SpriteRenderer>().enabled )
                    {
                        player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + widthOffset);
                        PlayerController.AbleTeleportDoor = Time.time;
                        Debug.Log("passato sopra");
                    }

                    break;
            }
        }
    }
}
