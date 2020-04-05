using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class BasicRoom : MonoBehaviour
{

    [HideInInspector] public StructureObject structureObject;
    [HideInInspector] public GameObject doorPrefab;
    [HideInInspector] public Tilemap floorMap;

    private List<Vector2> enemySpawnPoints = new List<Vector2>();
    
    private void OnEnable()
    {
        StartCoroutine(CreateEnemySpawnPoints());
        StartCoroutine(CreateDoors());
    }

   
    IEnumerator CreateEnemySpawnPoints()
    {
        yield return null;
        foreach (Vector2Int enemySpawnPos in structureObject.enemySpawnPoints)
        {
            Vector2 position = new Vector2(enemySpawnPos.x + transform.position.x - structureObject.center.x + 0.5f , enemySpawnPos.y + transform.position.y - structureObject.center.y + 0.5f);
            enemySpawnPoints.Add(position);
        }
    }

    IEnumerator CreateDoors()
    {
        yield return null;
        yield return null;
        yield return null;



        foreach (StructureObject.EntrancePointData entranceData in structureObject.entrancesData)
        {
            Vector3Int pos = new Vector3Int(entranceData.position.x + (int)transform.position.x - structureObject.center.x, entranceData.position.y + (int)transform.position.y - structureObject.center.y, 0);
            if (CheckPos(entranceData.direction, pos))
            {
                InstantiateDoor(entranceData.direction, pos);
            }
            
        }

        bool CheckPos(StructureObject.EntrancePointData.Direction dir, Vector3Int pos)
        {
            switch (dir)
            {
                case StructureObject.EntrancePointData.Direction.North:
                    pos += Vector3Int.up;
                    break;
                case StructureObject.EntrancePointData.Direction.East:
                    pos += Vector3Int.right;
                    break;
                case StructureObject.EntrancePointData.Direction.South:
                    pos += Vector3Int.down;
                    break;
                case StructureObject.EntrancePointData.Direction.West:
                    pos += Vector3Int.left;
                    break;

            }

            if (floorMap.GetTile(pos) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void InstantiateDoor(StructureObject.EntrancePointData.Direction dir, Vector3 pos)
        {
            GameObject obj = Instantiate(doorPrefab, pos, Quaternion.identity, transform);
            Animator objAnim = obj.GetComponent<Animator>();

            if (objAnim == null)
            {
                Debug.LogError("Door prefab doesn't have an animator!");
                return;
            }
            switch (dir)
            {
                case StructureObject.EntrancePointData.Direction.North:
                    objAnim.SetTrigger("DoorUp");
                    obj.transform.position += Vector3.one + Vector3.back;
                    break;
                case StructureObject.EntrancePointData.Direction.East:
                    objAnim.SetTrigger("DoorRight");
                    obj.transform.position += Vector3.right * 1.5f;
                    break;
                case StructureObject.EntrancePointData.Direction.South:
                    objAnim.SetTrigger("DoorDown");
                    obj.transform.position += Vector3.down + Vector3.right;
                    break;
                case StructureObject.EntrancePointData.Direction.West:
                    objAnim.SetTrigger("DoorLeft");
                    obj.transform.position += Vector3.left * 0.5f;
                    break;

            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector2 pos in enemySpawnPoints)
        {
            Gizmos.DrawWireSphere(pos, 0.5f);
        }
    }
}
