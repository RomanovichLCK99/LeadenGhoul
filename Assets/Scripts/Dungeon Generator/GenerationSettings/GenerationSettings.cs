using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "Generation Settings", menuName = "Dungeon Generation/Generation Settings")]
public  class GenerationSettings : ScriptableObject
{
    public RuleTile floorRuleTile;
    public RuleTile wallRuleTile;


    public RuleTile shadowRuleTile;
    public Tile randomErosion;

    public RuleTile ditherRuleTile;

    [Space(10)]

    public Tile topWallTile;
    public Tile botWallTile;
    public Tile emptyTile;

    [Space(10)]

    public StructureObject[] roomsArray;
    public GameObject[] customLights;
    [Space(10)]
    public GameObject basicRoomPrefab;
    public GameObject basicDoorPrefab;

    [Space(10)]
    public int deviationRate = 5;
    public int maxRouteLength = 5;
    public int maxRoutes = 20;
    public int spacing = 20;
    public int extraCorridorRate = 50;
    public int minNumberOfRooms = 7;
    public int maxNumberOfRooms = 10;

    [Space(10)]



    private DungeonGenerator dgGenerator;
    private GridSaveLoader saveLoader;

    public void Initialize(GameObject obj)
    {
        dgGenerator = obj.GetComponent<DungeonGenerator>();

        dgGenerator.floorRuleTile = floorRuleTile;
        dgGenerator.wallRuleTile = wallRuleTile;

        dgGenerator.shadowRuleTile = shadowRuleTile;
        dgGenerator.randomErosion = randomErosion;
        dgGenerator.ditherRuleTile = ditherRuleTile;

        dgGenerator.topWallTile = topWallTile;
        dgGenerator.botWallTile = botWallTile;
        dgGenerator.emptyTile = emptyTile;

        dgGenerator.roomsArray = roomsArray;

        dgGenerator.deviationRate = deviationRate;
        dgGenerator.maxRouteLength = maxRouteLength;
        dgGenerator.maxRoutes = maxRoutes;
        dgGenerator.spacing = spacing;
        dgGenerator.extraCorridorRate = extraCorridorRate;
        dgGenerator.minNumberOfRooms = minNumberOfRooms;
        dgGenerator.maxNumberOfRooms = maxNumberOfRooms;

        dgGenerator.customLights = customLights;
        dgGenerator.basicRoomPrefab = basicRoomPrefab;
        dgGenerator.basicDoorPrefab = basicDoorPrefab;
    }

    public void InitializeDesingMode(GameObject obj)
    {
        saveLoader = obj.GetComponent<GridSaveLoader>();

        saveLoader.floorRuleTile = floorRuleTile;
        saveLoader.wallRuleTile = wallRuleTile;

        saveLoader.shadowRuleTile = shadowRuleTile;
        saveLoader.randomErosion = randomErosion;
        saveLoader.ditherRuleTile = ditherRuleTile;

        saveLoader.topWallTile = topWallTile;
        saveLoader.botWallTile = botWallTile;
        saveLoader.emptyTile = emptyTile;

    }
        
}
