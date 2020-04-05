using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using MyUtils;

public class GridSaveLoader : MonoBehaviour
{
    public StructureObject structureObject;
    

    [Space(10)]
    public Tile squareTile;
    public GenerationSettings generationSettings;

    [HideInInspector] public RuleTile floorRuleTile;

    [HideInInspector] public RuleTile wallRuleTile;
    [HideInInspector] public RuleTile shadowRuleTile;
    [HideInInspector] public Tile randomErosion;

    [HideInInspector] public RuleTile ditherRuleTile;

    [HideInInspector] public Tile topWallTile;
    [HideInInspector] public Tile botWallTile;
    [HideInInspector] public Tile emptyTile;


    [Space(10)]
    public Tilemap floorMap;
    public Tilemap entrance;
    public Tilemap enemySpawnPoints;
    public Tilemap enemySpawnArea;

    public Tilemap lightSource; 

    [Space(10)]
    [SerializeField] private Tilemap wallMap = null;
    [SerializeField] private Tilemap verticalWallMap = null;

    [Space(10)]

    [SerializeField] private Tilemap extrasDither = null;
    [SerializeField] private Tilemap extrasShadow = null;
    [SerializeField] private Tilemap extrasErosion = null;

    [SerializeField] private Tilemap darknessMap = null;

    

    private Camera cam;
    private BoundsInt floorBounds;

    private void Start()
    {
        cam = Camera.main;
        generationSettings.InitializeDesingMode(gameObject);
    }

    private void Update()
    {
        floorBounds = floorMap.cellBounds;

        int x = Mathf.FloorToInt(cam.ScreenToWorldPoint(Input.mousePosition).x);
        int y = Mathf.FloorToInt(cam.ScreenToWorldPoint(Input.mousePosition).y);
                     

        if (Input.GetKeyDown(KeyCode.L))
            SaveGridData();

        if (Input.GetKeyDown(KeyCode.Mouse2))
            LoadGridData(x, y);
        if (Input.GetKeyDown(KeyCode.C))
            ClearTileMap();
        if (Input.GetKeyDown(KeyCode.V))
        {
            FillWalls();
            CreateShadows();
        } else if (Input.GetKeyUp(KeyCode.V))
        {
            extrasDither.ClearAllTiles();
            extrasErosion.ClearAllTiles();
            extrasShadow.ClearAllTiles();

            verticalWallMap.ClearAllTiles();
            wallMap.ClearAllTiles();
            darknessMap.ClearAllTiles();
        }
            
    }


    void ClearTileMap()
    {
        floorMap.ClearAllTiles();
        enemySpawnArea.ClearAllTiles();
        enemySpawnPoints.ClearAllTiles();
        entrance.ClearAllTiles();
        lightSource.ClearAllTiles();
    }

    void SaveGridData()
    {
        Debug.Log("Saving Data!");


        floorBounds = floorMap.cellBounds;

        structureObject.height = floorBounds.size.y;
        structureObject.width = floorBounds.size.x;
        structureObject.center = new Vector2Int(floorBounds.size.x  / 2, floorBounds.size.y / 2);

        List<StructureObject.TilePositionData> gridData = new List<StructureObject.TilePositionData>(floorBounds.x * floorBounds.y);
        List<StructureObject.EntrancePointData> entrances = new List<StructureObject.EntrancePointData>();
        List<Vector2Int> lightSourcesList = new List<Vector2Int>();
        List<Vector2Int> enemySpwnPosList = new List<Vector2Int>();

        Vector2Int[] areaPointsArray = new Vector2Int[2];
        bool firstPoint = false;

        for (int xMap = floorBounds.xMin; xMap <= floorBounds.xMax + 2; xMap++)
        {
            for (int yMap = floorBounds.yMin; yMap <= floorBounds.yMax + 2; yMap++)
            {

                Vector3Int pos = new Vector3Int(xMap, yMap, 0);

                TileBase floorTile = floorMap.GetTile(pos);
                TileBase enemySpwnPoint = enemySpawnPoints.GetTile(pos);
                TileBase enemySpwnArea = enemySpawnArea.GetTile(pos);
                TileBase enterPoint = entrance.GetTile(pos);
                TileBase lightSquare = lightSource.GetTile(pos);

                Vector2Int gridPos = new Vector2Int(xMap, yMap);

                if (floorTile != null)
                {
                    StructureObject.TilePositionData tilePosData = new StructureObject.TilePositionData(gridPos);
                    gridData.Add(tilePosData);
                }

                if (enterPoint != null)
                {
                    StructureObject.EntrancePointData.Direction dir = StructureObject.EntrancePointData.Direction.North;

                    if (gridPos.x  > structureObject.center.x)
                    {
                        dir = StructureObject.EntrancePointData.Direction.East;
                    } 
                    else if(gridPos.x < structureObject.center.x) 
                    {
                        dir = StructureObject.EntrancePointData.Direction.West;
                    } 
                    else if(gridPos.y  > structureObject.center.y) 
                    {
                        dir = StructureObject.EntrancePointData.Direction.North;
                    } else if(gridPos.y - 1 < 0) 
                    {
                        dir = StructureObject.EntrancePointData.Direction.South;
                    } else
                    {
                        Debug.LogWarning("Something fishy is going on with the entrances!");
                    }

                    StructureObject.EntrancePointData enterPointData = new StructureObject.EntrancePointData(dir, gridPos);
                    entrances.Add(enterPointData);
                }

                if (enemySpwnPoint != null)
                {
                    enemySpwnPosList.Add(gridPos);
                } 

                if (enemySpwnArea != null)
                {
                    if (!firstPoint)
                    {
                        firstPoint = true;
                        areaPointsArray[0] = gridPos;
                    }
                    else
                    {
                        areaPointsArray[1] = gridPos;
                    }
                }

                if (lightSquare != null)
                {
                    lightSourcesList.Add(gridPos);
                }
            }
        }

        structureObject.lightSourceList = lightSourcesList;
        structureObject.tileData = gridData;
        structureObject.entrancesData = entrances;
        structureObject.enemySpawnPoints = enemySpwnPosList;

    }
    void LoadGridData(int x, int y)
    {
        floorMap.ClearAllTiles();

;

        foreach (StructureObject.TilePositionData gridData in structureObject.tileData)
        {

            Vector3Int pos = MyUtils.VectorConversion.V3IntFromV2Int(gridData.tilePosition);
            floorMap.SetTile(pos, floorRuleTile);
        }

        foreach (StructureObject.EntrancePointData entranceData in structureObject.entrancesData)
        {
            Vector3Int pos = MyUtils.VectorConversion.V3IntFromV2Int(entranceData.position);
            entrance.SetTile(pos, squareTile);
        }

        foreach (Vector2Int enemySpawnPoint in structureObject.enemySpawnPoints)
        {
            Vector3Int pos = MyUtils.VectorConversion.V3IntFromV2Int(enemySpawnPoint);
            enemySpawnPoints.SetTile(pos, squareTile);
        }

        foreach (Vector2Int light in structureObject.lightSourceList)
        {
            Vector3Int pos = MyUtils.VectorConversion.V3IntFromV2Int(light);
            lightSource.SetTile(pos, squareTile);
        }

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

                TileBase wallTileAbove = wallMap.GetTile(posAbove);

                darknessMap.SetTile(pos, emptyTile);

                if (floorTile == null)
                {
                    if (floorTileThreeBelow == null && floorTileBelow == null) wallMap.SetTile(pos, wallRuleTile);

                    if (floorTileBelow != null)
                    {
                        verticalWallMap.SetTile(pos, botWallTile);
                        verticalWallMap.SetTile(posAbove, topWallTile);
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
                Vector3Int posTwoAbove = new Vector3Int(xMap, yMap + 2, 0);
                Vector3Int posAbove = new Vector3Int(xMap, yMap + 1, 0);

                Vector3Int posUpperLeft = new Vector3Int(xMap + 1, yMap + 1, 0);

                TileBase wallTileUpperLeft = wallMap.GetTile(posUpperLeft);
                TileBase wallTileTwoAbove = wallMap.GetTile(posTwoAbove);
                TileBase wallTile = wallMap.GetTile(pos);

                TileBase verticalWallTileUpperleft = verticalWallMap.GetTile(posUpperLeft);
                TileBase verticalWallTileAbove = verticalWallMap.GetTile(posAbove);

                TileBase floorTile = floorMap.GetTile(pos);


                if ((wallTileTwoAbove != null || wallTileUpperLeft != null || verticalWallTileUpperleft != null || verticalWallTileAbove != null) && floorTile != null && wallTile == null)
                {
                    extrasShadow.SetTile(pos, shadowRuleTile);
                }
            }
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, Vector3.up * 100);
        Gizmos.DrawLine(Vector3.zero, Vector3.right * 100);
    }

}
