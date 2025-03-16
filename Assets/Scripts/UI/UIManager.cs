using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI woodText;
    public TextMeshProUGUI foodText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateResources(int wood, int food)
    {
        woodText.text = wood.ToString();
        foodText.text = food.ToString();
    }
}
