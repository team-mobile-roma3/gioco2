using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;
    private List<List<Vector2Int>> dungeonRooms = new List<List<Vector2Int>>();
    private DungeonCrawlerController generator;

    private int temp;
    private int nRoomLoaded;


    private void Start()
    {
        generator = new DungeonCrawlerController();

        temp = 0;
        for (int i = 0; i < dungeonGenerationData.livelli; i++)
        {
            dungeonRooms.Add(generator.GenerateDungeon(dungeonGenerationData, new Vector2Int(temp, 0)));
            temp += 10;  
        }

        /*    for (int i = 0; i < dungeonRooms.Count; i++)
            {
                temp = 0;
                   SpawnRooms(dungeonRooms[i], temp, i+1);
                temp += 10;
            }

        */
        StartCoroutine(Wait(dungeonRooms));
    }

    private void SpawnRooms(IEnumerable<Vector2Int> levels, int temp, int k)
    {
       
        RoomController.instance.LoadRoom("Start"+(k), temp, 0);
        foreach (Vector2Int roomLocation in levels)
        {

            nRoomLoaded += RoomController.instance.LoadRoom(RoomController.instance.GetRandomRoomName(), roomLocation.x, roomLocation.y);
        }


    }
    IEnumerator Wait(List<List<Vector2Int>> levels)
    {
        temp = 0;

        for (int i = 0; i < levels.Count; i++)
        {
            
            SpawnRooms(dungeonRooms[i], temp, i + 1);
            temp += 10;
            yield return new  WaitForSeconds(0.2f);
        }
    
   
       
      
    }
}
