using UnityEngine;

public class SelectionUnite : MonoBehaviour
{
    public bool isSelected;
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    public LayerMask unitLayerMask;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D clickedCollider = Physics2D.OverlapPoint(mouseWorldPosition, unitLayerMask);

            if (clickedCollider != null && clickedCollider.gameObject == gameObject)
            {
                isSelected = !isSelected;
                spriteRenderer.color = isSelected ? Color.green : Color.white;
            }
        }

        if (Input.GetMouseButtonDown(1) && isSelected)
        {
            isSelected = false;
            spriteRenderer.color = Color.white;
                Debug.Log(gameObject.name + " is deselected!");
            }
        }
    }
}
