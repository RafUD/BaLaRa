using UnityEngine;

public class MouvementUnite : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 targetPosition;
    private bool isMoving;
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
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                isMoving = true;
            }

            if (isMoving)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                if ((Vector2)transform.position == targetPosition)
                    isMoving = false;
            }
        }
    }
}
}
