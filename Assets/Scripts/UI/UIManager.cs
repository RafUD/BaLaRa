using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI foodText;

    [Header("HUD Setup")]
    public GameObject hudPrefab;
    private GameObject currentHUD;

    [Header("Building Buttons")]
    public Button lumberCampBtn;
    public Button farmCampBtn;
    public Button barrackBtn;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void InitializeHUD()
    {
        if (hudPrefab == null)
        {
            Debug.LogError("[UIManager] HUD prefab is missing!");
            return;
        }

        if (currentHUD != null)
        {
            Debug.LogWarning("[UIManager] HUD already exists, skipping instantiation.");
            return;
        }
        
        currentHUD = Instantiate(hudPrefab);
        Debug.Log("[UIManager] HUD successfully initialized.");
        
        woodText = currentHUD.transform.Find("BarRessources/WoodText").GetComponent<TextMeshProUGUI>();
        foodText = currentHUD.transform.Find("BarRessources/FoodText").GetComponent<TextMeshProUGUI>();
        
        lumberCampBtn = currentHUD.transform.Find("BarPrix/LumberCampBtn").GetComponent<Button>();
        farmCampBtn = currentHUD.transform.Find("BarPrix/FarmCampBtn").GetComponent<Button>();
        barrackBtn = currentHUD.transform.Find("BarPrix/BarrackBtn").GetComponent<Button>();
        
        lumberCampBtn.onClick.AddListener(() => TryConstructBuilding("LumberCamp", lumberCampBtn));
        farmCampBtn.onClick.AddListener(() => TryConstructBuilding("FarmCamp", farmCampBtn));
        barrackBtn.onClick.AddListener(() => TryConstructBuilding("Barrack", barrackBtn));
    }

    public void UpdateResources(int wood, int food)
    {
        if (woodText == null || foodText == null)
        {
            Debug.LogError("[UIManager] Resource text elements are not assigned!");
            return;
        }

        woodText.text = wood.ToString();
        foodText.text = food.ToString();

        UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        if (lumberCampBtn != null)
            lumberCampBtn.interactable = CanAffordBuilding(lumberCampBtn);
        if (farmCampBtn != null)
            farmCampBtn.interactable = CanAffordBuilding(farmCampBtn);
        if (barrackBtn != null)
            barrackBtn.interactable = CanAffordBuilding(barrackBtn);
    }

    private bool CanAffordBuilding(Button buildingButton)
    {
        if (buildingButton == null) return false;

        TextMeshProUGUI priceText = buildingButton.transform.Find("PriceHolder").GetComponent<TextMeshProUGUI>();
        if (priceText == null)
        {
            Debug.LogError($"[UIManager] PriceHolder not found for {buildingButton.name}");
            return false;
        }

        if (!int.TryParse(priceText.text, out int buildingCost))
        {
            Debug.LogError($"[UIManager] Invalid price value for {buildingButton.name}: {priceText.text}");
            return false;
        }

        return NetworkPlayer.LocalPlayer != null && NetworkPlayer.LocalPlayer.wood >= buildingCost;
    }

    private void TryConstructBuilding(string buildingType, Button button)
    {
        if (NetworkPlayer.LocalPlayer == null)
        {
            Debug.LogError("[UIManager] No local player found!");
            return;
        }

        TextMeshProUGUI priceText = button.transform.Find("PriceHolder").GetComponent<TextMeshProUGUI>();
        if (priceText == null)
        {
            Debug.LogError($"[UIManager] PriceHolder not found for {buildingType}");
            return;
        }

        if (!int.TryParse(priceText.text, out int buildingCost))
        {
            Debug.LogError($"[UIManager] Invalid price value for {buildingType}: {priceText.text}");
            return;
        }

        if (NetworkPlayer.LocalPlayer.wood >= buildingCost)
        {
            Vector2 buildPosition = NetworkPlayer.LocalPlayer.transform.position + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2));
            
            NetworkPlayer.LocalPlayer.CmdConstructBuilding(buildPosition, buildingCost);
        }
        else
        {
            Debug.LogWarning($"[UIManager] Not enough wood to build {buildingType}. Required: {buildingCost}, Available: {NetworkPlayer.LocalPlayer.wood}");
        }
    }

}
