using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletFire; //bullets to fire
    public Transform fireFrom; //location where bullets would be fired from

    public float fireRate = 7.5f; //intervals between each bullets being fired
    private float bulletCounter; //bulletCounter is set to 0 in the beginning

    public int playerGunSound = 12;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.canMove && !LevelManager.instance.isPaused)
        {
            if (bulletCounter > 0)
            {
                bulletCounter -= Time.deltaTime;
            }
            else
            {
                //-------fire the bullet-------
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) //when left mouse button is being pressed...
                {
                    Instantiate(bulletFire, fireFrom.position, fireFrom.rotation); //copy bullet from the prefab and fire it
                    bulletCounter = fireRate; //to prevent the bullet from firing twice instantly

                    AudioManager.instance.PlaySFX(playerGunSound);
                }
            }
        }
    }
}
