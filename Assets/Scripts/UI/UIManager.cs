using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject interactionUI;
    [SerializeField] TextMeshProUGUI interactionText;
    [SerializeField] GameObject respawnScreen;
    [SerializeField] TextMeshProUGUI levelText, expText;
    [SerializeField] GameObject inventoryScreen;

    public static Action OnRespawnButtonClick;

    // Start is called before the first frame update
    void Start()
    {
        Player.PlayerEnterRangeOfBuilding += SetInteractionUI;
        Player.PlayerExitRangeOfBuilding += DisableInteractionUI;
        Player.OnPlayerDied += EnableDeathScreen;

        Player.OnExpUp += UpdateExp;
        Player.OnLevelUp += UpdateLevel;

        PlayerController.ToggleInventory += ToggleInventory;
    }

    void SetInteractionUI(BuildingEntity _buildingEntity)
    {
        interactionUI.SetActive(true);
        interactionText.text = _buildingEntity.name;
    }

    void DisableInteractionUI()
    {
        interactionUI.SetActive(false);
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

    void ToggleInventory()
    {
        inventoryScreen.SetActive(!inventoryScreen.activeSelf);
    }
}
