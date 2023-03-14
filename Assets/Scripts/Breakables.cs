using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    public GameObject []brokenPieces; //sprites of broken boxes will be stored in this list
    public int maxPieces = 5;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop; //this is to store data of which items to be dropped when the box get broken
    public float itemDropPercent;

    public int breakSound = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void smash()
    {
        Destroy(gameObject);

        AudioManager.instance.PlaySFX(breakSound);

        //-------show broken pieces-------
        int piecesToDrop = Random.Range(1, maxPieces); //generating random number to determine how many pieces will be instantiated 

        for (int i = 0; i < piecesToDrop; i++)
        {
            int randomPiece = Random.Range(0, brokenPieces.Length);
            //randomly generating one of the broken pieces of the box 
            Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
        }

        //-------drop items-------
        if (shouldDropItem)
        { 
            float dropChance = Random.Range(0f, 100f); //will select ONE random int from 0 to 99(total 100 numbers) 

            if (dropChance < itemDropPercent) 
            //e.g if itemDropPercent = 10, dropChance(from 0 to 100) < 10. so if this code were to be activated only possible int for dropChance is from 0 to 9 (total 10 numbers) 
            //so the percentage of this code going through is 10/100, which is same as itemDropPercent
            {
                int randomItem = Random.Range(0, itemsToDrop.Length); //instantiating one of the random item in the itemsToDrop list. e.g. coin, healthpack

                Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //-------breaking boxes -------
        if(other.tag == "Player")
        {
            //will only destroy the object when the player is dashing
            if(PlayerController.instance.dashCounter > 0)
            {
                smash();
            }
        }

        if(other.tag == "PlayerBullet") //if the player bullet get into contact, it will break the box
        {
            smash();
        }
    }
}
