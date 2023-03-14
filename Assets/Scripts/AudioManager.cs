using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; //to allow other scripts to refer to this script to use a function

    public AudioSource levelMusic, gameOverMusic, winMusic; //music for different situation

    public AudioSource[] sfx; //creating a list to store all the avila sfx to play it whenever needed

    // Start is called before the first frame update
    void Start()
    {
        //this is to ensure there is only one audiomanager script running in the game
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGameOver()
    {
        //stop the main music and play the game over music
        levelMusic.Stop(); 

        gameOverMusic.Play();
    }

    public void PlayLevelWin()
    {
        //stop the main music and play the game over music
        levelMusic.Stop();

        winMusic.Play();
    }

    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop(); //this line of code is used to prevent the sfx sounds from keep stacking/overlapping 
        sfx[sfxToPlay].Play();
    }
}
