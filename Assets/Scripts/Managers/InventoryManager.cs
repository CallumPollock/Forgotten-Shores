using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    
    int currentEquippedIndex;
    [SerializeField] GameObject droppedItem;
    [SerializeField] GameObject itemUIPrefab;
    [SerializeField] Transform listContentTransform;


    private void Start()
    {
        Player.PlayerInventoryChanged += UpdateInventoryList;
    }

    public void UpdateInventoryList(List<Item> inventory, int equippedIndex)
    {
        foreach(Transform child in listContentTransform)
        {
            Destroy(child.gameObject);
        }

        foreach (Item invItem in inventory)
        {
            GameObject newListItem = Instantiate(itemUIPrefab);
            newListItem.transform.SetParent(listContentTransform);
            newListItem.transform.localScale = Vector3.one;
            newListItem.GetComponentInChildren<TMP_Text>().text = String.Format("{0} x{1}", invItem.name, invItem.stack);
            newListItem.GetComponentInChildren<Image>().sprite = invItem.icon;

        }
    }

    
}
