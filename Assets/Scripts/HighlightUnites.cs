using UnityEngine;

public class HighlightOnHover : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color allyColor = Color.blue;  
    public Color enemyColor = Color.red;  
    private Color originalColor;

    public bool isAlly = false; 

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void OnMouseOver()
    {
        if (spriteRenderer != null)
        {
            if (isAlly)
            {
                spriteRenderer.color = allyColor;
            }
            else
            {
                spriteRenderer.color = enemyColor;
            }
        }
    }

    void OnMouseExit()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
}
