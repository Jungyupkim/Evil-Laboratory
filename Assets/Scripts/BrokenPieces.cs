using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector3 moveDirection;

    public float deceleration = 5f;

    public float lifeTime = 3f;

    public SpriteRenderer theSR;
    public float fadeSpeed = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        //this is to randomly decide which directions are the broken pieces going to travel
        moveDirection.x = Random.Range(-moveSpeed, moveSpeed);
        moveDirection.y = Random.Range(-moveSpeed, moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDirection * Time.deltaTime; //this code will let the broken pieces travel to different directions(moveDirection)

        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime); 
        //this code(Lerp function) will eventually make the acceleration of the broken pieces(moveDirection) to 0(Vector3.Zero) over specific period of time(deceleration, 5f, * Time.deltaTime)
        //to be more specific, for example, you can imagine decreasing the certain value by 50% per seconds. so 1 --> 0.5 --> 0.25 --> 0.125... and so on 

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0) 
        {
            //difference between MoveTowards and Lerp, Lerp will decrease/change the value by certain percentages per frame, while MoveTowards change the value in a fixed value per frame
            theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, Mathf.MoveTowards(theSR.color.a, 0f, fadeSpeed * Time.deltaTime));
            //this code will subtract certain amount(fadeSpeed) from the specific value(theSR.clor.a) per seconds, eventually making the value(theSR.color.a) to 0
            //e.g. 1 --> 0.8 --> 0.6 --> 0.4 --> 0.2 --> 0

            if(theSR.color.a == 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
