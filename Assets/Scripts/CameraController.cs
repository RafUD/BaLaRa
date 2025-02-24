using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public Tilemap tilemap;
    public float moveSpeed = 5f;
    public Vector3 startPosition = new Vector3(0, 0, 0);

    private Camera cam;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        cam.orthographicSize = 5;

        Bounds tilemapBounds = tilemap.localBounds;
        minBounds = tilemapBounds.min;
        maxBounds = tilemapBounds.max;

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        startPosition.x = Mathf.Clamp(startPosition.x, minBounds.x + camWidth, maxBounds.x - camWidth);
        startPosition.y = Mathf.Clamp(startPosition.y, minBounds.y + camHeight, maxBounds.y - camHeight);

        transform.position = new Vector3(startPosition.x, startPosition.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            move += Vector3.up;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            move += Vector3.down;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            move += Vector3.left;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            move += Vector3.right;

        move.Normalize();

        Vector3 newPosition = transform.position + move * moveSpeed * Time.deltaTime;

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x + camWidth, maxBounds.x - camWidth);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y + camHeight, maxBounds.y - camHeight);

        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }
}
