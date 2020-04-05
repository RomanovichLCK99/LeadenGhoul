using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    [SerializeField] private Transform firePoint = null;

    [Header("Variables")]
    [Space(10f)]
    public float timeBetweenShots = 0.5f;
    public float bulletForce = 20f;

    bool isShooting;

    // Update is called once per frame
    void Update()
    {

        if(Input.GetButton("Fire1"))
        { 
            if (!isShooting) StartCoroutine(Shoot());
        }

    }

    IEnumerator Shoot() 
    {
        isShooting = true;
        createBullet();
        yield return new WaitForSeconds(timeBetweenShots);
        isShooting = false;
    }

    void createBullet()
    {
        GameObject bullet = ObjectPooler.instance.SpawnFromPool("Bullet", firePoint.position, firePoint.rotation  );
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
