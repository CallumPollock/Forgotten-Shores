using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Human : Entity
{

    [SerializeField] List<Hand> hands = new List<Hand>();
    private AudioSource audioSource;

    public virtual void Awake()
    {
        foreach (Hand hand in GetComponentsInChildren<Hand>())
        {
            hands.Add(hand);
        }
       audioSource = gameObject.AddComponent<AudioSource>();
    }

    public List<Hand> GetHands() { return hands; }

    public void ScrollEquippedItem(int handIndex, int scrollValue)
    {
        if (GetInventory().Count == 0)
            return;

        if (handIndex < hands.Count)
        {
            hands[handIndex].SetEquippedItem(GetInventory()[Mathf.Clamp(GetInventory().IndexOf(hands[handIndex].GetEquippedItem()) + scrollValue, 0, GetInventory().Count - 1)]);
        }
    }

    public void EquipItem(int handIndex, int itemIndex)
    {
        if (handIndex < hands.Count && itemIndex < GetInventory().Count)
        {
            hands[handIndex].SetEquippedItem(GetInventory()[itemIndex]);
        }
    }

    public override bool AddToInventory(Item item)
    {

        if (item.stack <= 0) return true;
        if (item.name.Contains("EXP")) return false;

        if (GetInventory().Any(i => i.itemID == item.itemID))
        {
            GetItemFromInventory(item.itemID).stack += item.stack;
        }
        else
        {
            GetInventory().Add(item);
        }
        InventoryChanged?.Invoke(this, GetInventory());
        CreateInfoText(String.Format("+{0} {1}", item.stack, item.name), Color.white);

        if (hands.Count > 0)
            if (hands[0].GetEquippedItem() == null) hands[0].SetEquippedItem(item);

        if (item.pickupSound != null)
            audioSource.PlayOneShot(item.pickupSound);
        else
            audioSource.PlayOneShot(GameState.instance.defaultPickupSound);

        return true;


    }
}
