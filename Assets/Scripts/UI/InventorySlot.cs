using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]Image icon;
    [SerializeField]TextMeshProUGUI amount;


    public void FillSlot(Sprite itemIcon, int stackSize)
    {
        icon.sprite = itemIcon;
        icon.gameObject.SetActive(true);
        if (stackSize <= 1)
            amount.text = "";
        else
            amount.text = stackSize.ToString();
    }

    public void EmptySlot()
    {
        icon.gameObject.SetActive(false);
        icon.sprite = null;
    }

    public void UpdateStack(int stackSize)
    {
        if (stackSize <= 1)
            amount.text = "";
        else
            amount.text = stackSize.ToString();
    }
}
