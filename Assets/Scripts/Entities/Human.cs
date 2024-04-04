using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void EquipItem(int handIndex, int itemIndex)
    {
        if (handIndex < hands.Count && itemIndex < GetInventory().Count)
        {
            hands[handIndex].SetEquippedItem(GetInventory()[itemIndex]);
        }
    }
    public override void RemoveItem(Item item)
    {
        base.RemoveItem(item);
        foreach(Hand hand in hands)
        {
            if(hand.GetEquippedItem() == item) hand.SetEquippedItem(null);
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
        CreateInfoText(String.Format("+{0} {1} ({2})", item.stack, item.name, GetItemFromInventory(item.itemID).stack), Color.white, 4f, 1f);

        if (hands.Count > 0)
            if (hands[0].GetEquippedItem() == null) hands[0].SetEquippedItem(item);

        if (item.pickupSound != null)
            audioSource.PlayOneShot(item.pickupSound);
        else
            audioSource.PlayOneShot(GameState.instance.defaultPickupSound);

        return true;


    }
}
