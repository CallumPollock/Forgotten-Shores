using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject interactionUI;
    [SerializeField] TextMeshProUGUI interactionText;
    [SerializeField] GameObject respawnScreen;
    [SerializeField] TextMeshProUGUI levelText, expText;
    [SerializeField] GameObject inventoryScreen;

    [SerializeField] GameObject buildingItemGO;
    [SerializeField] TextMeshProUGUI buildingItemText;

    [SerializeField] GameObject navButton;
    [SerializeField] Transform content;
    [SerializeField] Transform navigationContainer;
    [SerializeField] Transform currentActiveTab;

    [SerializeField] List<Image> uiImageComponents;

    [SerializeField] GameObject pauseScreen;

    public static Action OnRespawnButtonClick;
    public static Action<BuildingItem> OnOpenCraftingMenu;

    private void Awake()
    {
        foreach (Transform child in content)
        {
            if (child.tag == "Menu")
            {
                Button newNavButton = Instantiate(navButton).GetComponent<Button>();
                uiImageComponents.Add(newNavButton.GetComponent<Image>());
                newNavButton.transform.SetParent(navigationContainer);
                newNavButton.transform.localScale = Vector3.one;
                newNavButton.onClick.AddListener(delegate { SetActiveTab(child); });
                newNavButton.GetComponentInChildren<TextMeshProUGUI>().text = child.name;
                child.gameObject.SetActive(false);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Player.PlayerEnterRangeOfBuilding += SetInteractionUI;
        Player.PlayerExitRangeOfBuilding += DisableInteractionUI;
        Player.OnPlayerDied += EnableDeathScreen;

        Player.OnExpUp += UpdateExp;
        Player.OnLevelUp += UpdateLevel;

        PlayerController.ToggleInventory += ToggleInventory;
        PlayerController.PressedPause += TogglePause;

        
        currentActiveTab = content.GetChild(0);
    }

    public void TogglePause()
    {
        pauseScreen.SetActive(!pauseScreen.activeSelf);
        Time.timeScale = pauseScreen.activeSelf ? 0.0f : 1.0f;
    }

    private void SetActiveTab(Transform tab)
    {
        currentActiveTab.gameObject.SetActive(false);
        currentActiveTab = tab;
        currentActiveTab.gameObject.SetActive(true);
    }

    void SetInteractionUI(BuildingEntity _buildingEntity)
    {
        interactionUI.SetActive(true);
        interactionText.text = _buildingEntity.name;
    }

    void DisableInteractionUI()
    {
        interactionUI.SetActive(false);
        buildingItemGO.SetActive(false);
        inventoryScreen.SetActive(false);
    }

    void EnableDeathScreen()
    {
        respawnScreen.SetActive(true);
    }

    void UpdateExp(int _experience, int _experienceToNextLevel)
    {
        expText.text = _experience + "/" + _experienceToNextLevel;
    }

    void UpdateLevel(int _level)
    {
        levelText.text = _level.ToString();
    }

    public void OnClickRespawn()
    {
        respawnScreen.SetActive(false);
        OnRespawnButtonClick?.Invoke();
    }

    void ToggleInventory(BuildingItem buildingItem)
    {
        inventoryScreen.SetActive(!inventoryScreen.activeSelf);
        

        if(inventoryScreen.activeSelf)
        {
            OnOpenCraftingMenu?.Invoke(buildingItem);

            Color color;

            if(buildingItem == null)
            {
                color = new Color(0f, 0.682353f, 1f);
            }
            else
            {
                color = buildingItem.color;
            }

            foreach (Image image in uiImageComponents)
            {
                image.color = color;
            }

        }

        if(buildingItem != null)
        {
            buildingItemGO.SetActive(inventoryScreen.activeSelf);
            buildingItemText.text = buildingItem.name;
            SetActiveTab(content.GetChild(1));
        }
        else
        {
            SetActiveTab(content.GetChild(0));
        }
    }
}
