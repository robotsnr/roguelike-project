using UnityEngine;

public class Example : MonoBehaviour
{
    Rigidbody2D body;
    float speed;
    float direction;

    void Start()
    {
        //Fetch the Rigidbody component you attach from your GameObject
        body = GetComponent<Rigidbody2D>();
        //Set the speed of the GameObject
        speed = 10.0f; 
    }

    void Update()
    {
        direction = body.rotation;
        Vector2 v = new Vector2(speed*Mathf.Cos(direction*Mathf.PI/180 + Mathf.PI/2),speed*Mathf.Sin(direction*Mathf.PI/180+Mathf.PI/2));

        
        if (Input.GetKey(KeyCode.W))
        {
            //Move the Rigidbody forwards constantly at speed you define (the blue arrow axis in Scene view)
            body.linearVelocity = v;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //Move the Rigidbody backwards constantly at the speed you define (the blue arrow axis in Scene view)
            body.linearVelocity = -v;
        }
        else
        {
            body.linearVelocity = new Vector2(0,0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            //Rotate the sprite about the Y axis in the positive direction
            body.rotation -= 1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            //Rotate the sprite about the Y axis in the negative direction
            body.rotation += 1f;
        }
    }
}