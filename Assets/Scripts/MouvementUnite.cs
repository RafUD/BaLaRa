using UnityEngine;

public class MouvementUnite : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Vector2 targetPosition;
    private bool isMoving = false;
    private SelectionUnite selectionUniteScript;

    private Rigidbody2D rb;

    private void Start()
    {
        selectionUniteScript = GetComponent<SelectionUnite>();
        rb = GetComponent<Rigidbody2D>();  
    }

    private void Update()
    {
        if (selectionUniteScript.isSelected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);
                isMoving = true;
            }

            if (isMoving)
            {
                Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime);
                rb.MovePosition(newPosition); 

                if (Vector2.Distance(rb.position, targetPosition) < 1f) 
                {
                    isMoving = false;
                }
            }
        }
    }
}
