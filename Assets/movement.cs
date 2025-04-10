using System;
using UnityEngine;

public class Example : MonoBehaviour
{
    private Rigidbody2D body;
    private float speed;
    private float direction;
    private Vector2 v = new Vector2();
    private Vector2 mousePosition = new Vector2();
    private float turnAngle;
    private Vector2 bodyPosition;
    private float turnSpeed;

    void Start()
    {
        //Fetch the Rigidbody component you attach from your GameObject
        body = GetComponent<Rigidbody2D>();
        //Set the speed of the GameObject
        speed = 10.0f;
        turnSpeed = 1.0f;
    }
    
    

    void Update()
    {
        handleInput();
    }

    void handleInput()
    {
        direction = body.rotation;
        bodyPosition = (Vector2)Camera.main.WorldToViewportPoint (transform.position);
        
        v.Set(speed*Mathf.Cos(direction*Mathf.PI/180 ),speed*Mathf.Sin(direction*Mathf.PI/180));
        
        mousePosition = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);



        float turnAngle = AngleBetweenTwoPoints(bodyPosition, mousePosition);    
        
        if (Input.GetKey(KeyCode.W))
        {
            //Move the Rigidbody forwards constantly at speed you define (the blue arrow axis in Scene view)
            body.linearVelocity = -v;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //Move the Rigidbody backwards constantly at the speed you define (the blue arrow axis in Scene view)
            body.linearVelocity = v;
        }
        else
        {
            body.linearVelocity = new Vector2(0,0);
        }
        
        Debug.Log(Mathf.Sqrt(Mathf.Pow((bodyPosition.y - mousePosition.y), 2f) +
                             Mathf.Pow((bodyPosition.x - mousePosition.x), 2f)));

        if (Mathf.Sqrt(Mathf.Pow((bodyPosition.y - mousePosition.y), 2f) +
                       Mathf.Pow((bodyPosition.x - mousePosition.x), 2f)) > .06)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, turnAngle));
        }

    }
    
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
       
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
       
    }
    
    
}