using UnityEngine;

public class swordMovement : MonoBehaviour
{
    private float distance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        distance = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement instance = GameObject.Find("Player").GetComponent<playerMovement>();
    
        // Convert the player's viewport position to world position
        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(new Vector3(
            instance.playerBodyPosition.x,
            instance.playerBodyPosition.y,
            Camera.main.nearClipPlane
        ));
    
        // Calculate the offset using the player's forward direction
        float radianDirection = instance.playerDirection * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(
            Mathf.Cos(radianDirection) * distance,  // X offset
            Mathf.Sin(radianDirection) * distance,  // Y offset
            0
        );

        // Position the sword in front of the player
        transform.position = worldPosition - offset;

        // Align the sword's rotation with the player's direction
        transform.rotation = Quaternion.Euler(0, 0, instance.playerDirection);

        Debug.Log("Sword Position: " + transform.position);
    }
}
