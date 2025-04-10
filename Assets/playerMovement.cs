using System;
using UnityEngine;
using UnityEngine.Serialization;

public class playerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private float speed;
    [FormerlySerializedAs("direction")] public float playerDirection;
    private Vector2 v = new Vector2();
    private Vector2 mousePosition = new Vector2();
    private float turnAngle;
    [FormerlySerializedAs("bodyPosition")] public Vector2 playerBodyPosition;

    void Start()
    {
        //Fetch the Rigidbody component you attach from your GameObject
        body = GetComponent<Rigidbody2D>();
        //Set the speed of the GameObject
        speed = 10.0f;
    }
    
    

    void Update()
    {
        handleInput();
    }

    void handleInput()
    {
        playerDirection = body.rotation;
        playerBodyPosition = (Vector2)Camera.main.WorldToViewportPoint(transform.position);

        v.Set(speed * Mathf.Cos(playerDirection * Mathf.PI / 180), speed * Mathf.Sin(playerDirection * Mathf.PI / 180));

        mousePosition = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float turnAngle = AngleBetweenTwoPoints(playerBodyPosition, mousePosition);

        // Calculate the smoothed rotation
        float smoothTurnSpeed = 7f; // Adjust this value for faster or slower turning
        float smoothedRotation = Mathf.LerpAngle(transform.rotation.eulerAngles.z, turnAngle, smoothTurnSpeed * Time.deltaTime);



        
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
        
        

        if (Mathf.Sqrt(Mathf.Pow((playerBodyPosition.y - mousePosition.y), 2f) +
                       Mathf.Pow((playerBodyPosition.x - mousePosition.x), 2f)) > .06)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, smoothedRotation));        }

    }
    
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
       
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
       
    }
    
    
}