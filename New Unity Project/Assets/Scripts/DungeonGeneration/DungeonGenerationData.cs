using UnityEngine;

[CreateAssetMenu(fileName = "DungeonGenerationData.asset", menuName = "DungeonGenerationData/Dungeon Data")]
public class DungeonGenerationData : ScriptableObject
{
    public Vector2Int pos;
    public int numberOfCrawlers;
    public int iterationMin;
    public int iterationMax;
}
