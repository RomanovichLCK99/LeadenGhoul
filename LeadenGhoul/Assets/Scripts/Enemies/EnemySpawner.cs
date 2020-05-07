using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private LayerMask boundsLayer = 0;
    [SerializeField] private string spawnEffectTag = "EnemySpawn";
    [SerializeField] private GameObject[] enemyList = null;

    [Space(10)]
    [Header("Variables")]
    [SerializeField] private float maxWait = 10f;
    [SerializeField] private float minWait = 5f;
    [SerializeField] private float effectDuration = 3f;

    [Tooltip("Used to generate an OverlapCircle to check whether an enemy can spawn somewhere")]
    [SerializeField] private float checkRadius = 2f;
    [SerializeField] private LayerMask nonspawnableLayers = 0;

    [SerializeField] private float extraSizeX = 15f;
    [SerializeField] private float extraSizeY = 5f;
    [SerializeField] private float minDistanceFromPlayer = 12f ;

    GameObject player;
    Bounds myBounds;
    Vector2 screenSize;
    
    Vector2 randomPos;

    bool isSpawning = false;

    private void Start()
    {
        myBounds = new Bounds();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // Calculates the Screen size 
        screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)));
        screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)));

        // Sets the bounds size
        myBounds.size = new Vector3(screenSize.x + extraSizeX, screenSize.y + extraSizeY, 20);
        myBounds.center = Camera.main.transform.position;

        randomPos = new Vector2(Random.Range(myBounds.min.x, myBounds.max.x), Random.Range(myBounds.min.y, myBounds.max.y));

        bool insideCollider = Physics2D.OverlapCircle(randomPos, checkRadius, nonspawnableLayers);
        bool insideBounds = Physics2D.OverlapCircle(randomPos, checkRadius * 0.5f, boundsLayer);

        if (!insideCollider && !isSpawning && Vector2.Distance(player.transform.position, randomPos) > minDistanceFromPlayer && insideBounds)
        {
            StartCoroutine(SpawnEnemy(randomPos));
        }
    }

    IEnumerator SpawnEnemy(Vector2 spawnPos)
    {
        isSpawning = true;

        Vector3 spawnPos3D = spawnPos;

        yield return new WaitForSeconds(Random.Range(minWait, maxWait) - effectDuration);
        ObjectPooler.instance.SpawnFromPool(spawnEffectTag, spawnPos, Quaternion.identity);
        yield return new WaitForSeconds(effectDuration);
        Instantiate(enemyList[Random.Range(0, enemyList.Length)], spawnPos3D, Quaternion.identity, this.transform);

        isSpawning = false;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube (myBounds.center, myBounds.size);

    }
}
