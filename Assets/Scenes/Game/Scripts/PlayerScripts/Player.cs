using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float movementSpeed = 15f;
    private Vector2 movementVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        movementVector = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movementVector.y += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            movementVector.y += -1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            movementVector.x += -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movementVector.x += 1;
        }

        movementVector = movementVector.normalized; // Normalize to prevent faster diagonal movement
    }

    private void FixedUpdate() // Use FixedUpdate for physics-related updates
    {
        rb.MovePosition(rb.position + movementVector * movementSpeed * Time.fixedDeltaTime);
    }
}
