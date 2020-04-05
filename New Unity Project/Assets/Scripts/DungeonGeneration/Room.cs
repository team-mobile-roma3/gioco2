using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int X;
    public int Y;

    public int Width;
    public int Height;
    void Start()
    {
        if(RoomController.instance == null)
        {

            Debug.Log("premuto play nella scena sbagliata");
            return;
        }

        RoomController.instance.RegisterRoom(this);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
    }
    public Vector3 GetRoomCentre()
    {

        return new Vector3(X * Width, Y * Height);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
