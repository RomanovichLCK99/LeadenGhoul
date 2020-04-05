using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    
    // Creating a Pool class to be shown in the inspector
    [System.Serializable]
    public class Pool
    {

        public string tag;
        public GameObject prefab;
        public int size;
    }


    // Making the script callable from other scripts
    #region Singleton

    public static ObjectPooler instance;
    void Awake() 
    {

        instance = this;

    }

    #endregion

    // Makes a list of the clss Pool
    public List<Pool> pools;
    // Makes a Dictionary - Queue will pulls out values in order. In this case, objects.
    public Dictionary<string,Queue<GameObject>> poolDictionary;

    void Start()
    {

        poolDictionary = new Dictionary<string,Queue<GameObject>>();

        foreach(Pool pool in pools)
        {

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0 ;i < pool.size; i++)
            {

                // Creates an objects for the pool and stores them in the queue;
                GameObject obj = Instantiate(pool.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);

            }

            poolDictionary.Add(pool.tag,objectPool);

        }

    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {

        // Called if the tag used doesn't correspond with any pools
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist!");
            return null;
        }

        // Pulls out a game object from the queue
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // Calls the OnObjectSpawn function in the spawned object;
        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObj != null){
            pooledObj.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;

    }

}
