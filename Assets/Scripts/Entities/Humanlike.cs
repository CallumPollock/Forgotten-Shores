using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Humanlike : Entity
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
    public override void RemoveItem(ItemData item)
    {
        base.RemoveItem(item);
        foreach(Hand hand in hands)
        {
            if(hand.GetEquippedItem() == item) hand.SetEquippedItem(null);
        }
    }


    public override bool AddToInventory(ItemData item)
    {

        if (item.stack <= 0) return true;
        if (item.name.Contains("EXP")) return false;

        if (GetInventory().Any(i => i.name == item.name))
        {
            GetItemFromInventory(item.name).stack += item.stack;
        }
        else
        {
            GetInventory().Add(item);
        }
        
        CreateInfoText(String.Format("+{0} {1} ({2})", item.stack, item.name, GetItemFromInventory(item.name).stack), Color.white, 4f, 1f);

        if (hands.Count > 0)
            if (hands[0].GetEquippedItem() == null) hands[0].SetEquippedItem(item);

        if (item.pickupSoundName != null)
            audioSource.PlayOneShot(Item.GetItemPickupSound(item.pickupSoundName));
        else
            audioSource.PlayOneShot(GameState.instance.defaultPickupSound);


        InventoryChanged?.Invoke(GetInventory(), GetInventory().IndexOf(hands[0].GetEquippedItem()));
        return true;


    }
}
