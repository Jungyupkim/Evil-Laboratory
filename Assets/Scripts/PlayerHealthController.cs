using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public int currentHealth;
    public int maxHealth;

    public float damageInvincLength = 1f;
    private float invincCount;

    public int playerDeathSound = 9, playerHurtSound = 11;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = CharacterTracker.instance.maxHealth;
        currentHealth = CharacterTracker.instance.currentHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Minusing off from the duration of players invincibility
        if(invincCount > 0)
        {
            invincCount -= Time.deltaTime;
            if(invincCount <= 0)
            {
                PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g,
                PlayerController.instance.bodySR.color.b, 1f); //setting back the player sprite to be opaque
            }
        }
    }

    //damaging the player if the player is not invincible at the moment
    public void damagePlayer()
    {
        if (invincCount <= 0)
        {


            currentHealth--;

            AudioManager.instance.PlaySFX(playerHurtSound);

            invincCount = damageInvincLength; //this is to let player be invincible after getting hit
            //this is to give visual aid to let player know that they are invincible
            PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, 
                PlayerController.instance.bodySR.color.b, 0.5f); 

            if (currentHealth <= 0)
            {
                PlayerController.instance.gameObject.SetActive(false); //disable player from the heirachy 

                UIController.instance.scoreText.text = "Score: " + LevelManager.instance.currentScore;
                UIController.instance.deathScreen.SetActive(true);

                AudioManager.instance.PlaySFX(playerDeathSound);
                AudioManager.instance.PlayGameOver();
                
            }

            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }
    }

    //making player invincible
    public void MakeInvincible(float length)
    {
        invincCount = length;

        //this is to give visual aid to let player know that they are invincible
        PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g,
                PlayerController.instance.bodySR.color.b, 0.5f);
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth; //prevent from player to exceeding their max health
        }

        //update UI
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    public void IncreaseMaxHealth(int amount)
    { 
        maxHealth += amount;
        currentHealth += amount;

        //update ui
        UIController.instance.healthSlider.maxValue = maxHealth; //to allow player to have max health and be able to display it through the UI slider for player health
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
}
