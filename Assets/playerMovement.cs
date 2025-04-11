using System;
using Unity.VisualScripting;
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
    private float turnSpeed;
    private float swingSpeed;
    private bool isSwing;
    private bool swingDirection; //false is left, true is right
    private float swingTurnAngle;
    private float swingCooldown;
    private float lastSwingTime;
    private int swingLoopTimes;
    private float targetAngle;
    [FormerlySerializedAs("bodyPosition")] public Vector2 playerBodyPosition;

    void Start()
    {
        //Fetch the Rigidbody component you attach from your GameObject
        body = GetComponent<Rigidbody2D>();
        //Set the speed of the GameObject
        speed = 7.0f;
        turnSpeed = 5f;
        swingSpeed = 30f;
        isSwing = false;
        swingDirection = false;
        swingTurnAngle = 120;
        swingCooldown = 0.5f;
        swingLoopTimes = -1;
        

    }
    
    

    void Update()
    {
        handleInput();
    }

    void handleInput()
    {
        
        Debug.Log($"Key A: {Input.GetKeyDown(KeyCode.A)}, Key D: {Input.GetKeyDown(KeyCode.D)}, isSwing: {isSwing}, Time Since Last Swing: {Time.time - lastSwingTime}");

        
        playerDirection = body.rotation;
        playerBodyPosition = (Vector2)Camera.main.WorldToViewportPoint(transform.position);

        v.Set(speed * Mathf.Cos(playerDirection * Mathf.PI / 180), speed * Mathf.Sin(playerDirection * Mathf.PI / 180));

        mousePosition = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float turnAngle = AngleBetweenTwoPoints(playerBodyPosition, mousePosition);
        
        

        // Calculate the smoothed rotation
        float smoothedRotation = Mathf.LerpAngle(transform.rotation.eulerAngles.z, turnAngle, turnSpeed * Time.deltaTime);
        float smoothedRotationSwing = Mathf.LerpAngle(transform.rotation.eulerAngles.z, swingTurnAngle, swingSpeed * Time.deltaTime);


        
        if(!isSwing) 
        {   
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
            
        }
        
        if (!isSwing && Time.time - lastSwingTime >= swingCooldown)
        {

            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("Swing triggered: Left");
                swingDirection = true;
                isSwing = true;
                lastSwingTime = Time.time;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("Swing triggered: Right");
                swingDirection = false;
                isSwing = true;
                lastSwingTime = Time.time;
            }
        }
        
        if (isSwing)
        {
            
            Debug.Log($"Swing in progress. Current Angle: {transform.rotation.eulerAngles.z}");
            if (swingDirection)
            {

                float leftSwingAngle = swingTurnAngle; // Set the angle for left swing
                float currentAngle = transform.rotation.eulerAngles.z;
                
                swingLoopTimes += 1;
                if (swingLoopTimes == 0)
                {
                    targetAngle = leftSwingAngle + currentAngle;

                }
                Debug.Log($"Swing in progress. target Angle:" + targetAngle);

                // Normalize the angles to handle negative values and 360° wraparound
                if (targetAngle > 180f) 
                {
                    targetAngle -= 360f;
                }
                if (targetAngle < 0)
                {
                    targetAngle += 360f;
                }

                float smoothedRotationSwingLeft = Mathf.LerpAngle(transform.rotation.eulerAngles.z, targetAngle,
                    swingSpeed * Time.deltaTime);

                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, smoothedRotationSwingLeft));
                if (Mathf.Abs(transform.rotation.eulerAngles.z - targetAngle) < 0.05f)
                {
                    CompleteSwing();
                }
                

            }

            if (!swingDirection)
            {

                float rightSwingAngle = -swingTurnAngle; // Set the angle for right swing
                float currentAngle = transform.rotation.eulerAngles.z;
                swingLoopTimes += 1;
                if (swingLoopTimes == 0)
                {
                    targetAngle = rightSwingAngle + currentAngle;

                }
                Debug.Log($"Swing in progress. target Angle:" + targetAngle);

                // Normalize the angles to handle negative values and 360° wraparound
                if (targetAngle > 180f) 
                {
                    targetAngle -= 360f;
                }

                if (targetAngle < 0)
                {
                    targetAngle += 360f;
                }
                
                

                float smoothedRotationSwingRight = Mathf.LerpAngle(currentAngle, targetAngle, swingSpeed * Time.deltaTime);

                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, smoothedRotationSwingRight));
                if (Mathf.Abs(transform.rotation.eulerAngles.z - targetAngle) < 0.05f)
                {
                    CompleteSwing();
                }
               

            }

        }
        
        
        

        if (!isSwing)
        {
            if (Mathf.Sqrt(Mathf.Pow((playerBodyPosition.y - mousePosition.y), 2f) +
                           Mathf.Pow((playerBodyPosition.x - mousePosition.x), 2f)) > .06)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, smoothedRotation));

            }
        }




    }
    
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
       
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
       
    }
    
    void CompleteSwing()
    {
        isSwing = false;
        Debug.Log("Swing reset.");
        Debug.Log($"isSwing: {isSwing}, Time since last swing: {Time.time - lastSwingTime}");
        swingLoopTimes = -1;
    }
    
    
}