using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float edgeScrollSpeed = 3f;
    public float edgeThreshold = 20f;

    private Rigidbody rb;
    private Vector3 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = false;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        HandleManualMovement();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    private void HandleManualMovement()
    {
        moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            moveDirection += Vector3.up;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            moveDirection += Vector3.down;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            moveDirection += Vector3.left;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveDirection += Vector3.right;

        moveDirection.Normalize();

        // Edge screen movement
        Vector2 mousePosition = Input.mousePosition;
        if (mousePosition.x <= edgeThreshold)
            moveDirection += Vector3.left;
        if (mousePosition.x >= Screen.width - edgeThreshold)
            moveDirection += Vector3.right;
        if (mousePosition.y <= edgeThreshold)
            moveDirection += Vector3.down;
        if (mousePosition.y >= Screen.height - edgeThreshold)
            moveDirection += Vector3.up;

        moveDirection.Normalize();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("[Camera] Collided with: " + collision.gameObject.name);
    }
}
