﻿
using UnityEngine;
[CreateAssetMenu(fileName = "DungeonGenerationData.asset", menuName = "DungeonGenerationData/Dungeon Data")]
public class DungeonGenerationData : ScriptableObject
{
    public int numberOfCrawlers;

    public int iterationMin;
    public int iterationMax;
    // Start is called before the first frame update
}
