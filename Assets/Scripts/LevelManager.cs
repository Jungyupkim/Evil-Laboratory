using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float waitToLoad = 4f;

    public string nextLevel;

    public bool isPaused;

    public bool bulletTime;

    public int currentCoins;

    //[HideInInspector]
    public int currentScore;
    public string sceneName;
         
    private void Awake()
    {
        instance = this;
        sceneName = SceneManager.GetActiveScene().name;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (sceneName == "Victory")
        {
            currentScore = CharacterTracker.instance.currentScore;
            Time.timeScale = 1f;
        }
        else 
        { 
            currentCoins = CharacterTracker.instance.currentCoins;
            currentScore = CharacterTracker.instance.currentScore;
            Time.timeScale = 1f;
            UIController.instance.coinText.text = currentCoins.ToString(); //value of currentCoins is in int so need to convert to string
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            slowmo();
        }
    }

    public IEnumerator LevelEnd() //coroutine that will last for 4 secs and then load the next scene
    {
        AudioManager.instance.PlayLevelWin();

        PlayerController.instance.canMove = false; //preventing player from moving after entering the "end"

        UIController.instance.startFadeToBlack();

        yield return new WaitForSeconds(waitToLoad); //coroutine will last for 4 secs

        if (nextLevel == "Victory")
        {
            currentScore += 1000;
            currentScore += PlayerHealthController.instance.currentHealth * 500;
        }

        //before moving to next level, use these codes to change the current coins and etc for them to load in next level.
        CharacterTracker.instance.currentCoins = currentCoins;
        CharacterTracker.instance.currentHealth = PlayerHealthController.instance.currentHealth;
        CharacterTracker.instance.maxHealth = PlayerHealthController.instance.maxHealth;
        CharacterTracker.instance.currentScore = currentScore;
   
        SceneManager.LoadScene(nextLevel);
        
    }

    public void PauseUnpause()
    {
        if (!isPaused)
        {
            UIController.instance.pauseMenu.SetActive(true); //showing the pause menu in the ui

            isPaused = true;

            Time.timeScale = 0f; //time progression in the game is set to 0, which means that mo objects in the game will be able to move
        }
        else
        {
            UIController.instance.pauseMenu.SetActive(false); //un-showing the pause menu in the ui

            isPaused = false;
            if (!bulletTime)
            {
                Time.timeScale = 1f; //setting the timescale back to 1 to resume the game
            }
            else
            {
                Time.timeScale = 0.05f;
            }
            
        }
    }

    public void slowmo()
    {
        if (!bulletTime)
        {
            UIController.instance.startSlowmo();
            bulletTime = true;

            Time.timeScale = 0.05f; //time progression of the game is set to 0.05 to make the time go slow
        }
        else
        {
            UIController.instance.stopSlowmo();
            bulletTime = false;

            Time.timeScale = 1f; //setting the timescale back to 1 to resume the game
        }
    }

    public void GetCoins(int amount)
    {
        currentCoins += amount;

        UIController.instance.coinText.text = currentCoins.ToString();
    }

    public void SpendCoins(int amount)
    {
        currentCoins -= amount;

        if(currentCoins < 0)
        {
            currentCoins = 0;
        }

        UIController.instance.coinText.text = currentCoins.ToString();
    }
}
