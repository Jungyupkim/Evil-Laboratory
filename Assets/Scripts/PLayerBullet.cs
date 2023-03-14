using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerBullet : MonoBehaviour
{

    public float speed = 8f;
    public Rigidbody2D theRB;

    public int bulletDamage = 50;

    public GameObject hitEffect;

    public int impactSound = 4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = transform.right * speed; //it is to make sure the bullet keeps traveling towards "right" side of the bullet 
    }

    //playing bullet hit effect after collision and destroy the gameObject
    private void OnTriggerEnter2D(Collider2D other) //will automatically run when the object collides with "other" collider
    {
        Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(gameObject); //when it collides with other collider, it will destroy itself

        AudioManager.instance.PlaySFX(impactSound);

        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().DamageEnemy(bulletDamage);
        }
    }

    //destroying the gameObject after the bullet goes outside of the game scene
    private void OnBecameInvisible() //when the bullet is outside of the camera
    {
        Destroy(gameObject);
    }
}
