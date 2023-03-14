using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom;
    public Color startColor, endColor, shopColor;

    public int distanceToEnd; //determines how many rooms will be generated in the level
    public bool includeShop;
    public int minDistanceShop, maxDistanceShop; //this is to prevent generating the shop to near to the player spawn or when the level is about to end

    public Transform generatorPoint;

    public enum Direction { up, right, down, left}; //created a new type of variable, direction e.g. int, string, etc
    public Direction selectedDirection;

    public float xOffset = 18f, yOffset = 10f;

    public LayerMask roomType;

    private GameObject endRoom, shopRoom;

    private List<GameObject> layoutRoomObjects = new List<GameObject>(); //everytime the new room is added, it will be added to this list

    public RoomPrefabs rooms; //referring to the newly created class called RoomPrefabs

    private List<GameObject> generatedOutlines = new List<GameObject>();

    public RoomCenter centerStart, centerEnd, centerShop;
    public RoomCenter[] potentialCenters;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startColor; //creating an assigned object with the start color at specific position

        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();

        for(int i = 0; i < distanceToEnd; i++) //for loop to generate specifc number of rooms for player to progress
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

            layoutRoomObjects.Add(newRoom);

            if(i + 1 == distanceToEnd)  //to find the end room and change the color of the end room
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                endRoom = newRoom;
            }

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            while (Physics2D.OverlapCircle(generatorPoint.position, .2f, roomType)) 
            {
                MoveGenerationPoint(); //this line will continue to run as long as the generation point is overlapping
            }
        }

        if (includeShop)
        {
            int shopLocation = Random.Range(minDistanceShop, maxDistanceShop+1);

            shopRoom = layoutRoomObjects[shopLocation]; //adding shop layer to the selected position(room number)
            layoutRoomObjects.RemoveAt(shopLocation); //deleting other random room generated at same room number as the shop layer
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor; //this is to differenciate the color of the shop room with other normal rooms
        }

        //create room outlines
        CreateRoomOutline(Vector3.zero); //creating outline for starting room
        foreach(GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position); //creating outline for each roomcenter in the list
        }
        CreateRoomOutline(endRoom.transform.position); //creating outline for end room
        if (includeShop)
        {
            CreateRoomOutline(shopRoom.transform.position); //create outline for shop
        }

        //crating room centers for every room outlines generated
        foreach(GameObject outline in generatedOutlines)
        {
            bool generateCenter = true;

            if(outline.transform.position == Vector3.zero) //creating center for starting room
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>(); //instantiating start center

                generateCenter = false;
            }

            if(outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                generateCenter = false;
            }

            if (includeShop)
            {
                if (outline.transform.position == shopRoom.transform.position)
                {
                    Instantiate(centerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                    generateCenter = false;
                }
            }

            if (generateCenter)
            {
                int centerSelect = Random.Range(0, potentialCenters.Length); //amoing the listed random room centers, it will choose one randomly

                Instantiate(potentialCenters[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>(); 
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
#if !UNITY_EDITOR //this code will only run in unity editor (mainly to test if the maps are being generated properlly)
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }

    public void MoveGenerationPoint()
    {
        switch (selectedDirection)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;

            case Direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;

            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;

            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
    }

    public void CreateRoomOutline(Vector3 roomPosition)
    {
        //checking if there is any overlapping room near the rooms
        //                                             position of the rooms, radius of the col area, what layer
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), .2f, roomType);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), .2f, roomType);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), .2f, roomType);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), .2f, roomType);

        //finding howmany rooms are around the room to decide on whice outlines to generate
        int directionCount = 0;
        if (roomAbove)
        {
            directionCount++;
        }
        if (roomBelow)
        {
            directionCount++;
        }
        if (roomRight)
        {
            directionCount++;
        }
        if (roomLeft)
        {
            directionCount++;
        }

        switch (directionCount)
        {
            case 0:
                Debug.LogError("No room exists!!");
                break;

            case 1:
                if (roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleUP, roomPosition, transform.rotation));
                }
                if (roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
                }
                if (roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                }
                if (roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                }
                break;

            case 2:
                if(roomAbove && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
                }
                if(roomRight && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation));
                }
                if(roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
                }
                if(roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleRightDown, roomPosition, transform.rotation));
                }
                if(roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleDownLeft, roomPosition, transform.rotation));
                }
                if(roomLeft && roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftUp, roomPosition, transform.rotation));
                }
                break;

            case 3:
                if(roomLeft && roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleLeftUpRight, roomPosition, transform.rotation));
                }
                if(roomAbove && roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpRightDown, roomPosition, transform.rotation));
                }
                if(roomRight && roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleRightDownLeft, roomPosition, transform.rotation));
                }
                if(roomBelow && roomLeft && roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleDownLeftUp, roomPosition, transform.rotation));
                }
                break;

            case 4:
                generatedOutlines.Add(Instantiate(rooms.fourway, roomPosition, transform.rotation));
                break;
        }
    }
}

[System.Serializable] //this will allow unity to process this as an object
public class RoomPrefabs //creating different class
{
    public GameObject singleUP, singleDown, singleLeft, singleRight,
        doubleUpDown, doubleLeftRight, doubleUpRight, doubleRightDown, doubleDownLeft, doubleLeftUp,
        tripleUpRightDown, tripleRightDownLeft, tripleDownLeftUp, tripleLeftUpRight,
        fourway;
}
