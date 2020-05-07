using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private string bulletCollisionEffectTag = "BulletCollision";


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(this.gameObject.tag))
        {
            ObjectPooler.instance.SpawnFromPool(bulletCollisionEffectTag, this.transform.position, Quaternion.identity);
            this.gameObject.SetActive(false);
        }
        
    }

}
