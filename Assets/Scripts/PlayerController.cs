using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance; //static wont allow to edit through unity editor

    //variables
    public float moveSpeed = 6f;
    private Vector2 moveInput; //can not edit in the unity editor
    public Rigidbody2D theRB; //"the rigid body"

    public Transform gunHand; //it is to move the aim(transform of the arm holding weapons)

    private Camera cam;

    public Animator anim;

    public SpriteRenderer bodySR; 

    private float activeMoveSpeed;
    public float dashSpeed = 10f, dashLength = .5f, dashCooldown = 1f, dashInvinc = .5f;
    [HideInInspector]
    public float dashCounter;
    private float dashCoolCounter;

    public int dashSound = 8; //serial number of different sfx

    [HideInInspector]
    public bool canMove = true;

    public void Awake()
    {
        instance = this; //will make sure that there is one instance is running instead of having multiple one
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main; //Unity will only search for the main camera at the start of the level (make the process more efficient by putting it at start instead of update.) 
        activeMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !LevelManager.instance.isPaused)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal"); //using unity input system to get the value of x (right: 1, Left: -1)
            moveInput.y = Input.GetAxisRaw("Vertical"); //using unity input system to get the value of y (up: 1, down: -1)

            moveInput.Normalize(); //make the player movement more consistent by noramlizing all the distance (can imagine the distance to be in a circle)

            //transform.position += new Vector3(moveInput.x, moveInput.y, 0) * Time.deltaTime * moveSpeed;

            theRB.velocity = moveInput * activeMoveSpeed; //this is to set the speed(velocity) in the rididBody2D by doing vector2 value * moveSpeed(float)

            //-------rotation of the gun-------
            Vector3 mousePos = Input.mousePosition; //position of the cursor
            Vector3 screenPoint = cam.WorldToScreenPoint(transform.localPosition); //position of the player

            if (mousePos.x < screenPoint.x) //if the mouse is on the left of the player,...
            {
                transform.localScale = new Vector3(-1f, 1f, 1f); //the player will turn left. (player will face right on default)
                gunHand.localScale = new Vector3(-1f, -1f, 1f); //this is to keep the gun facing left(-1 * -1 = 1) and invert the y axis to keep the gun facing upwards
            }
            else //if the mouse is NOT on the left of the player,...
            {
                transform.localScale = Vector3.one; //equivilant to (1f, 1f, 1f)the player will turn back to right
                gunHand.localScale = Vector3.one; //the gun will be facing normally 
            }

            //-------rotate gun arm-------
            Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y); //this is to calculate value of x axis and y axis in between the mouse/cursor and the player
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;//using trigometry to find the rad value of angle, then converting the value to degree
            //Unity uses Quaternion(x, y, z, w) to handle rotation. However, we can still just key in x, y, z value using "Euler" (able to let user interact as if they are chaning values in the unity editor straight)
            gunHand.rotation = Quaternion.Euler(0, 0, angle);
         

            //-------player dash-------
            if (Input.GetKeyDown(KeyCode.Space)) //when player press space
            {
                if (dashCoolCounter <= 0 && dashCounter <= 0) ///player dashing
                {
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength; //declaring how long the dash speed will last

                    anim.SetTrigger("dash");

                    PlayerHealthController.instance.MakeInvincible(dashInvinc);

                    AudioManager.instance.PlaySFX(dashSound);
                }
            }

            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime; //minusing off deltaTime(sescond) from the duration of dash speed
                if (dashCounter <= 0) //when dashCounter turns to 0, the player will go back to normal speed
                {
                    activeMoveSpeed = moveSpeed;
                    dashCoolCounter = dashCooldown;
                }
            }

            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime; //minusing off deltaTime(per second) from the duration of dashCooldown
            }

            //-------player movement-------
            if (moveInput != Vector2.zero) //when velocity is not 0,
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }

        }
        else 
        {
            theRB.velocity = Vector2.zero; //velocity of player is 0 when player can not move/ the game is paused
            anim.SetBool("isMoving", false);
        }
    }
}
