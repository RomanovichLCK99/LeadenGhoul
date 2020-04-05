using System.Collections;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private Animator animator = null;

    [Header("Variables")]
    [SerializeField] private string bulletTag = "Bullet";
    [SerializeField] private int bulletLayer = 17;
    [SerializeField] private string enemyDeathEffectTag = "EnemyDeath";

    [SerializeField] private int health = 3;
    [SerializeField] private float hurtDuration = 2f;

    [SerializeField] private float slowDownReward = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(bulletTag) && collision.gameObject.layer == bulletLayer)
        {
            health--;
            StartCoroutine(Hurt());
;       }

        if (health <= 0) DestroySelf();
    }

    IEnumerator Hurt()
    {
        animator.SetBool("isHurt", true);
        yield return new WaitForSeconds(hurtDuration);
        animator.SetBool("isHurt", false);
    }
    void DestroySelf()
    {
        ObjectPooler.instance.SpawnFromPool(enemyDeathEffectTag, this.transform.position, Quaternion.identity);
        UIController.instance.slowDownValue += slowDownReward;
        UIController.instance.enemyCount++;
        Destroy(gameObject);
    }
}
