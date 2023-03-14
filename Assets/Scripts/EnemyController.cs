using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed;

    [Header("Chase Player")]
    public bool shouldChasePlayer;
    public float enemyDetectionRange;
    private Vector3 moveDirection;

    [Header("Runaway")]
    public bool shouldRunAway;
    public float runawayRange;

    [Header("Wondering")]
    public bool shouldWander;
    public float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDirection;

    [Header("Patrolling")]
    public bool shouldPatrol;
    public Transform[] patrolPoints;
    private int currentPatrolPoint;

    [Header("Shooting")]
    public bool shouldShoot; //some enemy will shoot and some will not

    public GameObject bullet;
    public Transform fireFrom;
    public float fireRate;
    private float bulletCounter;

    public float shootRange;

    [Header("Variables")]
    public Animator anim;
    public SpriteRenderer theBody; //to check if the body sprite is rendering
    public int health = 150;

    public GameObject[] deathSplatters;
    public GameObject hitEffect;

    [Header("Enemy drop")]
    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    [Header("SFX")]
    public int enemyDeathSound = 1, enemyHurtSound = 2, enemyGunSound = 13;
    // Start is called before the first frame update
    void Start()
    {
        if (shouldWander) //letting enemy to pause for random amount of time and moving to random direction at a random time
        {
            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(theBody.isVisible && PlayerController.instance.gameObject.activeInHierarchy) //enemies will only attack player, if the player gameObject is active in the game scene
        {
            moveDirection = Vector3.zero; //enemies will stop moving by default

            //-------finding direction towards the player-------
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < enemyDetectionRange && shouldChasePlayer)
            {
                moveDirection = PlayerController.instance.transform.position - transform.position; //position of the player - position of the enemy to get direction to the player
            }
            else
            {
                if (shouldWander)
                {
                    if(wanderCounter > 0)
                    {
                        wanderCounter -= Time.deltaTime;

                        //move the enemy
                        moveDirection = wanderDirection; //new vector3 value assigned for enemies to move towards

                        if(wanderCounter <= 0)
                        {
                            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f); //to set different value of pauseCounter to give variation in delay between the enemy movement
                        }
                    }

                    if(pauseCounter > 0)
                    {
                        pauseCounter -= Time.deltaTime;

                        if(pauseCounter <= 0)
                        {
                            wanderCounter = Random.Range(wanderLength * .75f, wanderLength * 1.25f);

                            wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f); //random direction for enemy to travel towards.
                        }
                    }
                }

                if (shouldPatrol)
                {
                    moveDirection = patrolPoints[currentPatrolPoint].position - transform.position; //position of the (position of the emptyobject in the list) - transform.position

                    if(Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < .1f)
                    {
                        currentPatrolPoint++; //plus one to the currentPatrolPoint to move to different gameobjecy in the list
                        if(currentPatrolPoint >= patrolPoints.Length)
                        {
                            currentPatrolPoint = 0; //it will go back to its starting point and patrol again
                        }
                    }
                }
            }

            if(shouldRunAway && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < runawayRange)
            {
                moveDirection = transform.position - PlayerController.instance.transform.position;
            }

            moveDirection.Normalize(); //to normalize the distance of diagnal

            theRB.velocity = moveDirection * moveSpeed;

            //------enemy shooting-------
            if (shouldShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < shootRange)
            {
                bulletCounter -= Time.deltaTime;

                if (bulletCounter <= 0)
                {
                    bulletCounter = fireRate;
                    Instantiate(bullet, fireFrom.transform.position, fireFrom.transform.rotation);

                    AudioManager.instance.PlaySFX(enemyGunSound);
                }
            }
        }
        else //prevent enemies from moving/travelling after the player dies
        {
            theRB.velocity = Vector2.zero;
        }

        //------- enemy movement------- animation
        if (moveDirection != Vector3.zero) //when velocity is not 0 (when the enemy is moving)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;

        Instantiate(hitEffect, transform.position, transform.rotation);

        AudioManager.instance.PlaySFX(enemyHurtSound); //sound effect

        if (health <= 0)
        {
            Destroy(gameObject); //destroy the enemy when health become 0 or lesser

            int selectedSplatter = Random.Range(0, deathSplatters.Length); //randomize the deathSplatter image being instantiated

            int rotation = Random.Range(0, 4); //randomize the rotation of the deathSplatter image being instantiated

            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, rotation * 90f));

            LevelManager.instance.currentScore += 100;

            if (shouldDropItem)
            {
                float dropChance = Random.Range(0f, 100f);

                if (dropChance < itemDropPercent)
                {
                    int randomItem = Random.Range(0, itemsToDrop.Length);

                    Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
                }
            }

            AudioManager.instance.PlaySFX(enemyDeathSound);
        }
    }
}
