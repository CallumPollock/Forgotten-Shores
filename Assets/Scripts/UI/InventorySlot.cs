using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]Image icon;
    [SerializeField]TextMeshProUGUI amount;
    int stackCount = 0;
    public Item item;


    public void FillSlot(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.gameObject.SetActive(true);
        stackCount++;
        amount.text = stackCount.ToString();
    }

    public void EmptySlot()
    {
        icon.gameObject.SetActive(false);
        icon.sprite = null;
        stackCount = 0;
    }

    public void IncreaseStackAmount(int value)
    {
        stackCount += value;
        amount.text = stackCount.ToString();
    }

    public int GetStackCount()
    {
        return stackCount;
    }
}
