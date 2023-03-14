using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool closeWhenEntered /*, openWhenEnemiesCleared (this is commented out as we no longer keep track of how many enemies are left in this script. instead, it will be done inside RoomCenter.cs)*/;

    public GameObject[] doors;

    //public List<GameObject> enemies = new List<GameObject>(); //creating new empty list within the editor (this is commented out as the room is no longr able to track how many enemies are in the room

    [HideInInspector]
    public bool roomActive;

    public GameObject mapHider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false);

            closeWhenEntered = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            CameraController.instance.ChangeTarget(transform);

            if (closeWhenEntered)
            {
                foreach(GameObject doorObject in doors) //activating all the doors in the array using foreach loop
                {
                    doorObject.SetActive(true);
                }
            }

            roomActive = true;

            mapHider.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            roomActive = false;
        }
    }
}
