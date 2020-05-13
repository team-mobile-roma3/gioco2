using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> dungeonRooms;
    private List<Vector2Int> dungeonRooms2;
    private int temp;
  

    private void Start()
    {
        dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonGenerationData, dungeonGenerationData.pos);
        SpawnRooms(dungeonRooms);
       StartCoroutine(Wait());
        dungeonRooms2 = DungeonCrawlerController.GenerateDungeon(dungeonGenerationData, new Vector2Int(10,0));
       
        SpawnRooms2(dungeonRooms2);
    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms)
    {
        temp = 0;
        RoomController.instance.LoadRoom("Start", dungeonGenerationData.pos.x, dungeonGenerationData.pos.y);
        foreach(Vector2Int roomLocation in rooms)
        {
            temp += RoomController.instance.LoadRoom(RoomController.instance.GetRandomRoomName(), roomLocation.x, roomLocation.y);
        }

    }

    private void SpawnRooms2(IEnumerable<Vector2Int> rooms)
    {
       RoomController.instance.LoadRoom("Start2", 10, 0);
       

       foreach (Vector2Int roomLocation in rooms)
        {
            RoomController.instance.LoadRoom(RoomController.instance.GetRandomRoomName(), roomLocation.x, roomLocation.y);
        }
   
    }

    IEnumerator Wait()
    {
       
        yield return new WaitForSeconds(0.2f);

        
    }
}
