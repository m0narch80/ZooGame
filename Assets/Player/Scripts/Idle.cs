using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Idle : MonoBehaviour
{
    public float walkSpeed = 3f;

    private Rigidbody2D rb;
    private Vector2 input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Get input for movement
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Prevent diagonal movement: prioritize horizontal over vertical
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            input.y = 0;
        else
            input.x = 0;
    }

    private void FixedUpdate()
    {
        // Move character based on input
        if (input != Vector2.zero)
        {
            Vector2 newPos = rb.position + input.normalized * walkSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
    }
}
