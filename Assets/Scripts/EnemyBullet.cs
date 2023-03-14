using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;
    private Vector3 direction;

    public int impactSound = 4;

    // Start is called before the first frame update
    void Start()
    {
        //-------bullet trvelling in a straight line towards the player------- 
        //the bullet will only be able to find the most recent position of the player and travel towards that position in a straight line (it wont chase the enemy)
        direction = PlayerController.instance.transform.position - transform.position;
        direction.Normalize(); 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerHealthController.instance.damagePlayer(); 
        }

        Destroy(gameObject);
        AudioManager.instance.PlaySFX(impactSound);

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject); 
    }
}
