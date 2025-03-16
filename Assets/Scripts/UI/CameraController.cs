using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public Tilemap tilemap;
    public float moveSpeed = 5f;
    public float edgeScrollSpeed = 3f;
    public float edgeThreshold = 20f;
    public Vector3 startPosition = new Vector3(0, 0, 0);

    private Camera cam;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    private void Start()
    {
        cam = Camera.main;

        Bounds tilemapBounds = tilemap.localBounds;
        minBounds = tilemapBounds.min;
        maxBounds = tilemapBounds.max;

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        startPosition.x = Mathf.Clamp(startPosition.x, minBounds.x + camWidth, maxBounds.x - camWidth);
        startPosition.y = Mathf.Clamp(startPosition.y, minBounds.y + camHeight, maxBounds.y - camHeight);

        transform.position = new Vector3(startPosition.x, startPosition.y, transform.position.z);
    }

    private void Update()
    {
        Vector3 moveDirection = Vector3.zero;
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            moveDirection += Vector3.up;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            moveDirection += Vector3.down;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            moveDirection += Vector3.left;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveDirection += Vector3.right;
        
        moveDirection.Normalize();
        
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
        
        Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
        
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x + camWidth, maxBounds.x - camWidth);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y + camHeight, maxBounds.y - camHeight);

        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }
}