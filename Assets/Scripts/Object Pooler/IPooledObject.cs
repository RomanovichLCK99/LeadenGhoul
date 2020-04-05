using UnityEngine;

public interface IPooledObject 
{

    // Gets called on the object that got spawned;
    void OnObjectSpawn();

}
