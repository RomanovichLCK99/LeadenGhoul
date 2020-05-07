using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class DungeonGenerator : MonoBehaviour
{
    public GenerationSettings generationSettings;

    #region HiddenVariables
    [HideInInspector] public RuleTile floorRuleTile = null;
    [HideInInspector] public RuleTile wallRuleTile = null;


    [HideInInspector] public RuleTile shadowRuleTile = null;
    [HideInInspector] public Tile randomErosion = null;

    [HideInInspector] public RuleTile ditherRuleTile = null;

    [HideInInspector] public Tile topWallTile = null;
    [HideInInspector] public Tile botWallTile = null;
    [HideInInspector] public Tile emptyTile = null;

    [HideInInspector] public int deviationRate = 5;
    [HideInInspector] public int maxRouteLength = 5;
    [HideInInspector] public int maxRoutes = 20;
    [HideInInspector] public int spacing = 20;
    [HideInInspector] public int extraCorridorRate = 50;
    [HideInInspector] public int minNumberOfRooms = 7;
    [HideInInspector] public int maxNumberOfRooms = 10;

    [HideInInspector] public StructureObject[] roomsArray;

    [HideInInspector] public GameObject[] customLights;

    [HideInInspector] public GameObject basicRoomPrefab;
    [HideInInspector] public GameObject basicDoorPrefab;

    #endregion

    #region TileMaps
    [Space(10)]

    [SerializeField] private Tilemap floorMap = null;
    [SerializeField] private Tilemap wallMap = null;
    [SerializeField] private Tilemap verticalWallMap = null;

    [Space(10)]

    [SerializeField] private Tilemap extrasDither = null;
    [SerializeField] private Tilemap extrasShadow = null;
    [SerializeField] private Tilemap extrasErosion = null;

    [Space(10)]

    [SerializeField] private Tilemap invisWalls = null;
    [SerializeField] private Tilemap bulletCollision = null;
    [SerializeField] private Tilemap darknessMap = null;

    


    [Space(10)]
    [SerializeField] private bool generateDungeon = true;
    [SerializeField] private bool fillDungeon = true;

    #endregion

    



    private Dictionary<Vector2Int, StructureObject> roomsData = new Dictionary<Vector2Int, StructureObject>();

    private StructureObject farthestRoom;
    private Vector2Int farthestPos;
    private float greatestDistance;

    private Vector2Int firstRoomPos = Vector2Int.zero;
    private StructureObject firstRoom;


    private int dirFromStartTaken = 0;

    private GameObject roomHolder;

    private int numberOfRooms = 0;
    int routeCount = 0;
    int roomCount = 0;

    BoundsInt floorBounds;
   
    private void Start()
    {
        generationSettings.Initialize(gameObject);

        roomHolder = new GameObject("Room Holder");
        roomHolder.transform.SetParent(transform);

        numberOfRooms = Random.Range(minNumberOfRooms, maxRouteLength);
        

        if (generateDungeon)
        {
            ClearAllMaps();

            int x = 0;
            int y = 0;
 

            firstRoom = LoadRoom(x, y, roomsArray[0]); // Make sure the first room is is empty in the middle!

            Vector2Int previousRoomPos = new Vector2Int(x, y);

            y += spacing;

            StructureObject secondRoom = LoadRoom(x, y, randomRoom(roomsArray));
            Vector2Int newRoomPos = new Vector2Int(x, y);

            farthestRoom = secondRoom;
            farthestPos = newRoomPos;
            greatestDistance = Vector2Int.Distance(previousRoomPos, newRoomPos);


            DrawCorridor(secondRoom, firstRoom, newRoomPos, previousRoomPos);
            CreateRooms(x, y, previousRoomPos, firstRoom, 0); // first Route will be 0

            StartCoroutine(ExtraCorridors());

        }


        floorBounds = floorMap.cellBounds;

        if (fillDungeon) {
            StartCoroutine(Fill());
        }
        
        
    }

    private void CreateRooms(int x, int y, Vector2Int previousRoomPos, StructureObject previousRoom, int routeLength)
    {    
        Vector2Int newPos;

        if (routeCount < maxRoutes)
        {
            routeCount++;
            while (++routeLength < maxRouteLength) // Recursive generation
            {
                Random.InitState(System.Environment.TickCount);

                bool routeUsed = false;
                int xOffset = x - previousRoomPos.x;
                int yOffset = y - previousRoomPos.y;

                previousRoomPos = new Vector2Int(x, y);

                if (roomCount >= numberOfRooms) return;

                //Go Straight
                if (Random.Range(1, 100) <= deviationRate)
                {
                    

                    if (routeUsed)
                    {                      
                        newPos = new Vector2Int(previousRoomPos.x + xOffset, previousRoomPos.y + yOffset);

                        if (GeneratedInRoom(newPos))
                        {
                            dirFromStartTaken++;
                            break;
                        }

                        StructureObject newRoom = LoadRoom(newPos.x, newPos.y, randomRoom(roomsArray));

                        CheckFarthestRoom(newPos, newRoom);

                        DrawCorridor(newRoom, previousRoom, newPos, previousRoomPos);

                        CreateRooms(newPos.x, newPos.y, previousRoomPos, previousRoom, Random.Range(routeLength, maxRouteLength));
                        previousRoom = newRoom;
                    }
                    else
                    {
                        x = previousRoomPos.x + xOffset;
                        y = previousRoomPos.y + yOffset;

                        newPos = new Vector2Int(x, y);

                        if (GeneratedInRoom(newPos))
                        {
                            newPos = farthestPos + new Vector2Int(xOffset, yOffset);
                            SetVariablesXY();

                            if (GeneratedInRoom(newPos))
                                return;
                        }

                        StructureObject newRoom = LoadRoom(newPos.x, newPos.y, randomRoom(roomsArray));

                        CheckFarthestRoom(newPos, newRoom);

                        DrawCorridor(newRoom, previousRoom, newPos, previousRoomPos);

                        previousRoom = newRoom;
                        routeUsed = true;
                    }

                }


                if (roomCount >= numberOfRooms) return;
                //Go left
                if (Random.Range(1, 100) <= deviationRate)
                {
                    if (routeUsed)
                    {
                        newPos = new Vector2Int(previousRoomPos.x - yOffset, previousRoomPos.y + xOffset);

                        if (GeneratedInRoom(newPos))
                        {
                            dirFromStartTaken++;
                            break;
                        }

                        StructureObject newRoom = LoadRoom(newPos.x, newPos.y, randomRoom(roomsArray));

                        CheckFarthestRoom(newPos, newRoom);

                        DrawCorridor(newRoom, previousRoom, newPos, previousRoomPos);

                        CreateRooms(newPos.x, newPos.y, previousRoomPos, previousRoom, Random.Range(routeLength, maxRouteLength));
                        previousRoom = newRoom;
                    }
                    else
                    {
                        y = previousRoomPos.y + xOffset;
                        x = previousRoomPos.x - yOffset;

                        newPos = new Vector2Int(x, y);

                        if (GeneratedInRoom(newPos))
                        {
                            newPos = farthestPos + new Vector2Int(-yOffset, xOffset);
                            SetVariablesXY();

                            if (GeneratedInRoom(newPos))
                                return;
                        }

                        StructureObject newRoom = LoadRoom(newPos.x, newPos.y, randomRoom(roomsArray));

                        CheckFarthestRoom(newPos, newRoom);

                        DrawCorridor(newRoom, previousRoom, newPos, previousRoomPos);

                        previousRoom = newRoom;
                        routeUsed = true;
                    }

                }

                if (roomCount >= numberOfRooms) return;
                //Go right
                if (Random.Range(1, 100) <= deviationRate)
                {
                    if (routeUsed)
                    {
                        newPos = new Vector2Int(previousRoomPos.x + yOffset, previousRoomPos.y - xOffset);

                        if (GeneratedInRoom(newPos))
                        {
                            dirFromStartTaken++;
                            return;
                        }

                        StructureObject newRoom = LoadRoom(newPos.x, newPos.y, randomRoom(roomsArray));

                        CheckFarthestRoom(newPos, newRoom);

                        DrawCorridor(newRoom, previousRoom, newPos, previousRoomPos);

                        CreateRooms(newPos.x, newPos.y, previousRoomPos, previousRoom, Random.Range(routeLength, maxRouteLength));
                        previousRoom = newRoom;
                    }
                    else
                    {
                        y = previousRoomPos.y - xOffset;
                        x = previousRoomPos.x + yOffset;

                        newPos = new Vector2Int(x, y);

                        if (GeneratedInRoom(newPos))
                        {
                            newPos = farthestPos + new Vector2Int(yOffset, -xOffset);
                            SetVariablesXY();

                            if (GeneratedInRoom(newPos))
                                return;
                        }

                        StructureObject newRoom = LoadRoom(newPos.x, newPos.y, randomRoom(roomsArray));

                        CheckFarthestRoom(newPos, newRoom);

                        DrawCorridor(newRoom, previousRoom, newPos, previousRoomPos);
                        previousRoom = newRoom;
                        routeUsed = true;
                    }

                }

                if (roomCount >= numberOfRooms) return;

                if (!routeUsed)
                {
                    x = previousRoomPos.x + xOffset;
                    y = previousRoomPos.y + yOffset;

                    newPos = new Vector2Int(x, y);

                    if (GeneratedInRoom(newPos))
                    {
                        newPos = farthestPos + new Vector2Int(xOffset, yOffset);
                        SetVariablesXY();

                        if (GeneratedInRoom(newPos))
                            return;
                    }

                    StructureObject newRoom = LoadRoom(newPos.x, newPos.y, randomRoom(roomsArray));

                    CheckFarthestRoom(newPos, newRoom);

                    DrawCorridor(newRoom, previousRoom, newPos, previousRoomPos);
                    previousRoom = newRoom;
                }

            } 

            x = 0;
            y = 0;
            previousRoomPos = new Vector2Int(x, y);
            StructureObject secondRoom;

            switch (dirFromStartTaken)
            {
                case 1:
                    y -= spacing;
                    LoadExtraRoute();

                    break;
                case 2:
                    x += spacing;
                    LoadExtraRoute();
                    break;
                case 3:
                    x -= spacing;
                    LoadExtraRoute();
                    break;
                default:
                    return;
            }

            void LoadExtraRoute()
            {
                newPos = new Vector2Int(x, y);
                

                if (GeneratedInRoom(newPos))
                {
                    dirFromStartTaken++;
                    return;
                }

                secondRoom = LoadRoom(x, y, randomRoom(roomsArray));

                farthestRoom = secondRoom;
                farthestPos = newPos;
                greatestDistance = Vector2Int.Distance(previousRoomPos, newPos);

                DrawCorridor(secondRoom, firstRoom, newPos, previousRoomPos);
                CreateRooms(x, y, previousRoomPos, firstRoom, 0);
            }

            void SetVariablesXY()
            {
                x = newPos.x;
                y = newPos.y;
                previousRoomPos = farthestPos;
                previousRoom = farthestRoom;
            }
        }
        
    }
        

    private StructureObject LoadRoom(int x, int y, StructureObject objToLoad)
    {
        roomCount++;

        Vector2Int objectCenter = objToLoad.center;

        GameObject room = Instantiate(basicRoomPrefab, new Vector3(x, y, 0), Quaternion.identity, roomHolder.transform);
        BoxCollider2D boxCollider2D = room.GetComponent<BoxCollider2D>();
        BasicRoom roomScript = room.GetComponent<BasicRoom>();


        if (boxCollider2D == null)
        {
            Debug.LogError("The room prefab doesn't have a box collider!");
        }
        else
        {
            boxCollider2D.size = new Vector2(objToLoad.height, objToLoad.width);
        }


        if (roomScript == null)
        {
            Debug.LogError("The room prefab doesn't have a room script!");
        }
        else
        {
            roomScript.structureObject = objToLoad;
            roomScript.doorPrefab = basicDoorPrefab;
            roomScript.floorMap = floorMap;
        }


        foreach (StructureObject.TilePositionData gridData in objToLoad.tileData)
        {
            Vector2Int gridPosition = gridData.tilePosition;

            Vector3Int pos = new Vector3Int(gridPosition.x + x - objectCenter.x, gridPosition.y + y - objectCenter.y, 0);

            DrawFloorTiles(pos);
        }

        foreach (Vector2Int lightPos in objToLoad.lightSourceList)
        {
            Vector3 pos = new Vector3(lightPos.x + x - objectCenter.x, lightPos.y + y - objectCenter.y, 0);
            Instantiate(customLights[Random.Range(0,customLights.Length)], pos + Vector3.one * 0.5f, Quaternion.identity, room.transform);
        }

        roomsData.Add(new Vector2Int(x, y), objToLoad);

        return objToLoad;
        
    }

    private void DrawCorridor(StructureObject newObj, StructureObject prevObj, Vector2Int newRoomPos, Vector2Int previousRoomPos)
    {
        Vector2Int newRoomCenter = newObj.center;
        Vector2Int previousRoomCenter = prevObj.center;

        int dir = 0;

        if (newRoomPos.y < previousRoomPos.y)
        {
            dir = 2;
        } else if(newRoomPos.y > previousRoomPos.y)
        {
            dir = 0;
        } else if (newRoomPos.x < previousRoomPos.x)
        {
            dir = 3;
        } else if (newRoomPos.x > previousRoomPos.x)
        {
            dir = 1;
        }
        else
        {
            Debug.Log("Something fishy is going on with corridor drawing!");
        }

        switch (dir)
        {
            case 0:
                StructureObject.EntrancePointData southPoint0 = newObj.entrancesData.Find(x => x.direction == StructureObject.EntrancePointData.Direction.South);
                StructureObject.EntrancePointData  northPoint0 = prevObj.entrancesData.Find(x => x.direction == StructureObject.EntrancePointData.Direction.North);

                Vector2Int gridPosS0 = southPoint0.position - newRoomCenter + newRoomPos;
                Vector2Int gridPosN0 = northPoint0.position - previousRoomCenter + previousRoomPos;
                

                for (int xMap = gridPosN0.x; xMap <= gridPosS0.x + 1; xMap++)
                {
                    for (int yMap = gridPosN0.y - 1; yMap <= gridPosS0.y + 1; yMap++)
                    {
                        Vector3Int pos = new Vector3Int(xMap, yMap, 0);
                        DrawFloorTiles(pos);
                    }
                }

                break;
            case 1:
                StructureObject.EntrancePointData westPoint1 = newObj.entrancesData.Find(x => x.direction == StructureObject.EntrancePointData.Direction.West);
                StructureObject.EntrancePointData eastPoint1 = prevObj.entrancesData.Find(x => x.direction == StructureObject.EntrancePointData.Direction.East);

                Vector2Int gridPosW1 = westPoint1.position - newRoomCenter + newRoomPos;
                Vector2Int gridPosE1 = eastPoint1.position - previousRoomCenter + previousRoomPos;


                for (int xMap = gridPosE1.x - 1; xMap <= gridPosW1.x + 1; xMap++)
                {
                    for (int yMap = gridPosE1.y; yMap <= gridPosW1.y + 1; yMap++)
                    {
                        Vector3Int pos = new Vector3Int(xMap, yMap, 0);
                        DrawFloorTiles(pos);
                    }
                }

                break;
            case 2:
                StructureObject.EntrancePointData northPoint2 = newObj.entrancesData.Find(x => x.direction == StructureObject.EntrancePointData.Direction.North);
                StructureObject.EntrancePointData southPoint2 = prevObj.entrancesData.Find(x => x.direction == StructureObject.EntrancePointData.Direction.South);

                Vector2Int gridPosN2 = northPoint2.position - newRoomCenter + newRoomPos;
                Vector2Int gridPosS2 = southPoint2.position - previousRoomCenter + previousRoomPos;


                for (int xMap = gridPosN2.x; xMap <= gridPosS2.x + 1; xMap++)
                {
                    for (int yMap = gridPosN2.y - 1; yMap <= gridPosS2.y + 2; yMap++)
                    {
                        Vector3Int pos = new Vector3Int(xMap, yMap, 0);
                        DrawFloorTiles(pos);
                    }
                }

                break;
            case 3:
                StructureObject.EntrancePointData eastPoint3 = newObj.entrancesData.Find(x => x.direction == StructureObject.EntrancePointData.Direction.East);
                StructureObject.EntrancePointData westPoint3 = prevObj.entrancesData.Find(x => x.direction == StructureObject.EntrancePointData.Direction.West);

                Vector2Int gridPosE3 = eastPoint3.position - newRoomCenter + newRoomPos;
                Vector2Int gridPosW3 = westPoint3.position - previousRoomCenter + previousRoomPos;


                for (int xMap = gridPosE3.x - 1; xMap <= gridPosW3.x + 1; xMap++)
                {
                    for (int yMap = gridPosE3.y; yMap <= gridPosW3.y + 1; yMap++)
                    {
                        Vector3Int pos = new Vector3Int(xMap, yMap, 0);
                        DrawFloorTiles(pos);
                    }
                }

                break;
            default:
                Debug.LogWarning("Something wrong is going on with the generation of corridors!");
                break;

        }


    }

    private IEnumerator ExtraCorridors()
    {
        yield return null;

        Vector2Int newPos;
        Vector2Int previousPos;


        foreach (Vector2Int roomPos in roomsData.Keys)
        {
            previousPos = roomPos;          
            if (roomsData.ContainsKey(roomPos + Vector2Int.up * spacing))
            {
                newPos = roomPos + Vector2Int.up * spacing;
                CallDrawCorridor();
            } 

            if (roomsData.ContainsKey(roomPos + Vector2Int.right * spacing))
            { 
                newPos = roomPos + Vector2Int.right * spacing;
                CallDrawCorridor();
            } 

            if (roomsData.ContainsKey(roomPos + Vector2Int.down * spacing))
            {
                newPos = roomPos + Vector2Int.down * spacing;
                CallDrawCorridor();
            } 

            if (roomsData.ContainsKey(roomPos + Vector2Int.left * spacing))
            {
                newPos = roomPos + Vector2Int.left * spacing;
                CallDrawCorridor();
                
            }
            
        }


        void CallDrawCorridor()
        {
            if (Random.Range(1, 100) <= extraCorridorRate)
                DrawCorridor(roomsData[newPos], roomsData[previousPos], newPos, previousPos);
        }

    }
    
    private IEnumerator Fill()
    {
        yield return null;
        yield return null;
        FillWalls();
        CreateShadows();

    }


    private void CheckFarthestRoom(Vector2Int pos, StructureObject structObj)
    {
        float distance = Vector2Int.Distance(pos, firstRoomPos);
        if (distance > greatestDistance)
        {
            greatestDistance = distance;
            farthestRoom = structObj;
            farthestPos = pos;
        }
    }

    private void DrawFloorTiles(Vector3Int pos)
    {
        floorMap.SetTile(pos, floorRuleTile);
        extrasErosion.SetTile(pos, randomErosion);
        extrasDither.SetTile(pos, ditherRuleTile);
    }
    private void FillWalls()
    {

        for (int xMap = floorBounds.xMin - 10; xMap <= floorBounds.xMax + 10; xMap++)
        {
            for (int yMap = floorBounds.yMin - 10; yMap <= floorBounds.yMax + 10; yMap++)
            {

                Vector3Int pos = new Vector3Int(xMap, yMap, 0);
                Vector3Int posBelow = new Vector3Int(xMap, yMap - 1, 0);
                Vector3Int posAbove = new Vector3Int(xMap, yMap + 1, 0);

                Vector3Int posTwoAbove = new Vector3Int(xMap, yMap + 2, 0);

                Vector3Int posThreeBelow = new Vector3Int(xMap, yMap - 3, 0);




                TileBase floorTile = floorMap.GetTile(pos);
                TileBase floorTileBelow = floorMap.GetTile(posBelow);
                TileBase floorTileAbove = floorMap.GetTile(posAbove);

                TileBase floorTileThreeBelow = floorMap.GetTile(posThreeBelow);

                TileBase wallTileAbove = floorMap.GetTile(posAbove);

                darknessMap.SetTile(pos, emptyTile);

                if (floorTile == null)
                {
                    if (floorTileThreeBelow == null  && floorTileBelow == null) wallMap.SetTile(pos, wallRuleTile);
                    
                    if (wallTileAbove == null && floorTileBelow == null)invisWalls.SetTile(pos, emptyTile);
                    
                    if (floorTileBelow != null)
                    {
                        verticalWallMap.SetTile(pos, botWallTile);
                        verticalWallMap.SetTile(posAbove, topWallTile);
                        bulletCollision.SetTile(posAbove, emptyTile);
                        wallMap.SetTile(posTwoAbove, wallRuleTile);
                    }
                    else if (floorTileAbove != null)
                    {
                        wallMap.SetTile(posAbove, wallRuleTile);
                    }

                }
            }
        }
    }
    private void CreateShadows()
    {
        for (int xMap = floorBounds.xMin - 10; xMap <= floorBounds.xMax + 10; xMap++)
        {
            for (int yMap = floorBounds.yMin - 10; yMap <= floorBounds.yMax + 10; yMap++)
            {
                Vector3Int pos = new Vector3Int(xMap, yMap, 0);
                Vector3Int posRight = new Vector3Int(xMap + 1, yMap, 0);
                Vector3Int posTwoAbove = new Vector3Int(xMap, yMap + 2, 0);
                Vector3Int posAbove = new Vector3Int(xMap, yMap + 1, 0);

                Vector3Int posUpperRight = new Vector3Int(xMap + 1, yMap + 1, 0);

                TileBase wallTileUpperRight = wallMap.GetTile(posUpperRight);
                TileBase wallTileTwoAbove = wallMap.GetTile(posTwoAbove);
                TileBase wallTile = wallMap.GetTile(pos);

                TileBase verticalWallTileUpperleft = verticalWallMap.GetTile(posUpperRight);
                TileBase verticalWallTileAbove = verticalWallMap.GetTile(posAbove);
                TileBase verticalWallTileUpperRight = verticalWallMap.GetTile(posUpperRight);

                TileBase floorTile = floorMap.GetTile(pos);
                TileBase floorTileRight = floorMap.GetTile(posRight);


                if ((wallTileTwoAbove != null || wallTileUpperRight != null || verticalWallTileUpperleft != null || verticalWallTileAbove != null) && floorTile != null && wallTile == null)
                {
                    extrasShadow.SetTile(pos, shadowRuleTile);
                }

                if (verticalWallTileUpperRight != null && floorTileRight != null)
                {
                    extrasShadow.SetTile(pos, shadowRuleTile);
                }
            }
        }
    }

    private StructureObject randomRoom(StructureObject[] rooms)
    {
        StructureObject r = rooms[Random.Range(0, rooms.Length)];
        return r;
    }
    private bool GeneratedInRoom(Vector2Int pos)
    {

        if (roomsData.ContainsKey(pos))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ClearAllMaps()
    {
        floorMap.ClearAllTiles();

        extrasDither.ClearAllTiles();
        extrasErosion.ClearAllTiles();
        extrasShadow.ClearAllTiles();

        verticalWallMap.ClearAllTiles();
        wallMap.ClearAllTiles();
        darknessMap.ClearAllTiles();
    }

}

// Original generation algorithm by Erik Overflow from https://www.youtube.com/watch?v=QaryeJsjrI8
// The code was reworked by myself to be used in the game LeadenGhoul.
