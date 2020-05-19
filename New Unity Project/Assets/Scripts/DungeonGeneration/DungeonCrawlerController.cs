using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up = 0,
    left = 1,
    down = 2,
    right = 3
};


public class DungeonCrawlerController : MonoBehaviour
{
    
    private  List<Vector2Int> positionsVisited;
    private static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction, Vector2Int>
    {
        {Direction.up, Vector2Int.up},
        {Direction.left, Vector2Int.left},
        {Direction.down, Vector2Int.down},
        {Direction.right, Vector2Int.right}
    };

    public  List<Vector2Int> GenerateDungeon(DungeonGenerationData dungeonData, Vector2Int startpos)
    {
        positionsVisited = new List<Vector2Int>();
        List<DungeonCrawler> dungeonCrawlers = new List<DungeonCrawler>();

        for(int i = 0; i < dungeonData.numberOfCrawlers; i++)
        {
            dungeonCrawlers.Add(new DungeonCrawler(startpos));
        }

        int iterations = Random.Range(dungeonData.iterationMin, dungeonData.iterationMax);
        
        for(int i = 0; i < iterations; i++)
        {
            foreach(DungeonCrawler dungeonCrawler in dungeonCrawlers)
            {
                Vector2Int newPos = dungeonCrawler.Move(directionMovementMap);
                Debug.Log(" ho visitato " + newPos);
                if(newPos ==  startpos)
                    Debug.Log(" ho visitato " + newPos+ "ma non la aggiungo");
                else 
                positionsVisited.Add(newPos);
            }
        }
        Debug.Log("fine visita ");
        return positionsVisited;
     
    }
}
