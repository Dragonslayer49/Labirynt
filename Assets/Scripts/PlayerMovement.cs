using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;


    private Rigidbody rb;

    void Awake()
    {
        // Get the Rigidbody on this GameObject
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get targetMovingSpeed based on overrides
        float targetMovingSpeed = speed;


        // Get input for movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculate targetVelocity
        Vector2 targetVelocity = new Vector2(x * targetMovingSpeed, z * targetMovingSpeed);

        // Apply movement.
        GetComponent<Rigidbody>().velocity = transform.rotation * new Vector3(targetVelocity.x, GetComponent<Rigidbody>().velocity.y, targetVelocity.y);
    }
}