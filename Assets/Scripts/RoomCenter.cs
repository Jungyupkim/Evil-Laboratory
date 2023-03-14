using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    public bool openWhenEnemiesCleared;

    public List<GameObject> enemies = new List<GameObject>(); //creating new empty list within the editor

    public Room theRoom;

    // Start is called before the first frame update
    void Start()
    {
        if (openWhenEnemiesCleared)
        {
            theRoom.closeWhenEntered = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Count is used to get the length of the list
        if (enemies.Count > 0 && theRoom.roomActive && openWhenEnemiesCleared)
        {
            //using for loop to specifically find which object is null, so that we can delete it from the list.
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--; //minus 1 from i since the total count of the list will decrease by 1 after deleting the object from the list
                }
            }

            if (enemies.Count == 0)
            {
                theRoom.OpenDoors();
            }
        }
    }
}
