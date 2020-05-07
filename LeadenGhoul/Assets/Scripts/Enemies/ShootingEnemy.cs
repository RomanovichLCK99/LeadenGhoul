using System.Collections;
using UnityEngine;
using MyUtils;

public class ShootingEnemy : MonoBehaviour
{

    [SerializeField] private string poolName = "EnemyBullet";
    [SerializeField] private string playerTag = "Player";

    [Space(10)]
    [Header("Variables")]
    [SerializeField] private float minDistanceToPlayer = 3f;
    [SerializeField] private float maxDistanceToPlayer = 10f;
    [SerializeField] private float speed = 3f;

    [SerializeField] private float bulletForce = 20f;
    [SerializeField] private float timeBetweenShots = 3f;
    [SerializeField] private float warmUp = 0.5f;

    [Space(10)]
    public bool isSentry = false;

    [Space(10)]
    public bool isShotgun;
    [Range(1, 20)]
    [SerializeField] private int numberofBullets = 1;
    [Range(0,360)]
    [SerializeField] private float spreadAngle = 30f;

    Transform playerTransform;
    float distanceToPlayer;
    Vector2 playerDir;

    bool isShooting = true;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Transform>();
        StartCoroutine(WarmUp());
    }

    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        playerDir = (playerTransform.position - transform.position).normalized;
    }

    private void FixedUpdate()
    {

        if (isSentry == false)
        {
            if (isShooting == false && distanceToPlayer < maxDistanceToPlayer && distanceToPlayer > minDistanceToPlayer)
            {
                if (!isShotgun)
                {
                    StartCoroutine(Shoot(1,0f));
                }
                else
                {
                    StartCoroutine(Shoot(numberofBullets, spreadAngle));
                }
            }
            else if (distanceToPlayer < minDistanceToPlayer)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, -speed * Time.deltaTime);
            }
            else if (distanceToPlayer > maxDistanceToPlayer)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
            }
        } else
        {
            if (isShooting == false && distanceToPlayer < maxDistanceToPlayer)
            {
                if (!isShotgun)
                {
                    StartCoroutine(Shoot(1, 0f));
                }
                else
                {
                    StartCoroutine(Shoot(numberofBullets, spreadAngle));
                }
            }
        }

    }


    IEnumerator WarmUp()
    {
        yield return new WaitForSeconds(warmUp);
        isShooting = false;
    }
    IEnumerator Shoot(int bulletQuantity, float spread)
    {
        isShooting = true;
        createBullets(bulletQuantity, spread);
        yield return new WaitForSeconds(timeBetweenShots);
        isShooting = false;
    }
    
    void createBullets(int bulletQuantity, float spread )
    {
        float aimAngle = Utility.GetAngleFromVectorFloat(playerDir) + spread * 0.5f;
        Vector3 playerDir3D = new Vector3(playerDir.x, playerDir.y);

        for(int i = 0; i <= bulletQuantity - 1 ; i++ )
        {

            float angleIncrease = 0f;

            if (bulletQuantity > 1)
            {
                angleIncrease = -spread + (spread / (bulletQuantity - 1)) * i ;
            } 
            
            GameObject bullet = ObjectPooler.instance.SpawnFromPool(poolName, transform.position, Quaternion.Euler(0, 0, aimAngle + angleIncrease - 90f));
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;

            Vector2 shootDir = Utility.GetVectorFromAngle(aimAngle + angleIncrease);
            rb.AddForce( shootDir * bulletForce, ForceMode2D.Impulse);
        }

        
    }
    
}
