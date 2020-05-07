using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private int bulletLayer = 17;


    [Space(10)]
    [Header("Variables")]
    [SerializeField] private float hurtDuration = 0.5f;

    bool invincible;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.layer == bulletLayer && other.tag == enemyTag)
        {
            if (!invincible) UIController.instance.health--;
            StartCoroutine(isHurt());
        }

        if (UIController.instance.health <= 0) PauseScreen.instance.GoToMainMenu();
        
    }

    IEnumerator isHurt()
    {
        invincible = true;
        animator.SetBool("isHurt", true);
        Physics2D.IgnoreLayerCollision(this.gameObject.layer, bulletLayer, true);
        yield return new WaitForSeconds(hurtDuration);
        Physics2D.IgnoreLayerCollision(this.gameObject.layer, bulletLayer, false);
        animator.SetBool("isHurt", false);
        invincible = false;
    }
}
