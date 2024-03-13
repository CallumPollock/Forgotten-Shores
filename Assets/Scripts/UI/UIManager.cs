using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject interactionUI;
    [SerializeField] TextMeshProUGUI interactionText;

    // Start is called before the first frame update
    void Start()
    {
        Player.PlayerEnterRangeOfBuilding += SetInteractionUI;
        Player.PlayerExitRangeOfBuilding += DisableInteractionUI;
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
}
