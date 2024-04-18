using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Humanlike : Entity
{

    [SerializeField] List<Hand> hands = new List<Hand>();
    private AudioSource pickupSource;
    
    bool isHitting = false;

    public virtual void Awake()
    {
        foreach (Hand hand in GetComponentsInChildren<Hand>())
        {
            hands.Add(hand);
        }
        pickupSource = gameObject.AddComponent<AudioSource>();
        pickupSource.spatialBlend = 1f;
        pickupSource.maxDistance = 25f;
        pickupSource.rolloffMode = AudioRolloffMode.Linear;
    }

    public override void Update()
    {
        if(this.GetType() != typeof(Player))
        {
            if (GetTarget() == null) return;
            Vector2 direction = GetTarget().transform.position - transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, direction);

            foreach (Hand hand in GetHands())
            {
                hand.transform.eulerAngles = new Vector3(0, 0, angle + hand.GetHandDirectionOffset());

            }

            if (Vector2.Distance(transform.position, GetTarget().transform.position) <= 3f && !isHitting)
            {
                StartCoroutine(Attack());
            }
        }
        

        base.Update();
    }

    

    public IEnumerator Attack()
    {
        GetHands()[UnityEngine.Random.Range(0, GetHands().Count)].Hit();
        isHitting = true;
        yield return new WaitForSeconds(0.3f);
        isHitting = false;
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
            if (hands[0].GetEquippedItem().stack == 0) hands[0].SetEquippedItem(item);

        if (Item.GetItemPickupSound(item.pickupSoundName) != null)
            pickupSource.PlayOneShot(Item.GetItemPickupSound(item.pickupSoundName));
        else
            pickupSource.PlayOneShot(GameState.instance.defaultPickupSound);


        InventoryChanged?.Invoke(GetInventory(), GetInventory().IndexOf(hands[0].GetEquippedItem()));
        return true;


    }

    public override void OnAttacked(Entity entity)
    {
        SetTarget(entity);
    }
}
