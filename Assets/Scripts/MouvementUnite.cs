using UnityEngine;

public class MouvementUnite : MonoBehaviour
{
    public float moveSpeed = 5f; 
    private Vector2 targetPosition;
    private bool isMoving = false;
    private SelectionUnite selectionUniteScript;

    private void Start()
    {
        selectionUniteScript = GetComponent<SelectionUnite>(); 
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
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

              
                if ((Vector2)transform.position == targetPosition)
                {
                    isMoving = false;
                }
            }
        }
    }
}
