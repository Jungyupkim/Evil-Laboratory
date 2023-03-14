using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public GameObject buyMessage;

    private bool inBuyZone;

    public bool isHealthRestore, isHealthUpgrade; //isWeapon; (was going to add more weapons but I think Im getting too much ambitious

    public int itemCost;

    public int healthUpgradeAmount;

    private int shopBuySound = 18, shopNoMonSound = 19;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inBuyZone)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(LevelManager.instance.currentCoins >= itemCost)
                {
                    LevelManager.instance.SpendCoins(itemCost);

                    if (isHealthRestore) //restoring players health
                    {
                        PlayerHealthController.instance.HealPlayer(PlayerHealthController.instance.maxHealth);
                    }

                    if (isHealthUpgrade) //upgrading players max health
                    {
                        PlayerHealthController.instance.IncreaseMaxHealth(healthUpgradeAmount);
                    }

                    gameObject.SetActive(false); //to prevent player from buying the same item again
                    inBuyZone = false; //to not show the "buy message" to the player after player has bought the item

                    AudioManager.instance.PlaySFX(shopBuySound);
                }
                else
                {
                    AudioManager.instance.PlaySFX(shopNoMonSound);
                }
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            buyMessage.SetActive(true); //to show the message appear when the player approaches to the item

            inBuyZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            buyMessage.SetActive(false); //to show the message appear when the player approaches to the item

            inBuyZone = false;
        }
    }
}
