using UnityEngine;
using System;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Dungeon Structure", menuName = "Dungeon Generation/Structure Object")]
public class StructureObject : ScriptableObject
{

    [Header("Manually set Variables")]
    public int minWeight;
    public int maxWeight;


    [Space(30)]

    public int height;
    public int width;
    public Vector2Int center;

    [Space(10)]

    public List<Vector2Int> enemySpawnPoints;
    public List<EntrancePointData> entrancesData;

    [Space(10)]

    public List<Vector2Int> lightSourceList;

    [Space(10)]

    public List<TilePositionData> tileData;


    [Serializable]
    public class TilePositionData
    {
        public Vector2Int tilePosition;
        public TilePositionData(Vector2Int pos)
        {
            tilePosition = pos;
        }
    }

    [Serializable]
    public class EntrancePointData
    {
        public enum Direction
        {
            North, South, East, West
        }
        public Direction direction;
        public Vector2Int position;

        public EntrancePointData(Direction dir, Vector2Int pos)
        {
            direction = dir;
            position = pos;
        }
    }

    


}



