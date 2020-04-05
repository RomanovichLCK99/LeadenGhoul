using System.Collections;
using UnityEngine;

public class PlayFromPool : MonoBehaviour, IPooledObject
{
    

    private ParticleSystem myParticleSystem;

    float myDuration;

    void Awake()
    {
        myParticleSystem = gameObject.GetComponent<ParticleSystem>();
        myDuration = myParticleSystem.main.duration;
    }

    public void OnObjectSpawn()
    {
        myParticleSystem.Play();
        StartCoroutine(Deactivate());
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(myDuration);
        gameObject.SetActive(false);
    }
}
