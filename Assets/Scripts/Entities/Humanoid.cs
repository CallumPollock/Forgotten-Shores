using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : Entity
{

    [SerializeField] List<Hand> hands = new List<Hand>();
    private AudioSource swish;

    public virtual void Awake()
    {
        foreach (Hand hand in GetComponentsInChildren<Hand>())
        {
            hands.Add(hand);
        }
        swish = GetComponent<AudioSource>();
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

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<DroppedItem>())
        {
            if (collision.GetComponent<DroppedItem>().item != null)
            {
                AddToInventory(collision.GetComponent<DroppedItem>().item);
                if(hands.Count > 0)
                    if (hands[0].GetEquippedItem() == null) hands[0].SetEquippedItem(collision.GetComponent<DroppedItem>().item);
                if(swish != null)
                    swish.Play();
                Destroy(collision.gameObject);
            }

        }
    }
}
