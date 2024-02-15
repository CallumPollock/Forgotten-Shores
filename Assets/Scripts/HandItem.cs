using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandItem : MonoBehaviour
{
    Hand hand;

    private void Awake()
    {
        hand = GetComponentInParent<Hand>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hand.OnTriggerEnter2D(collision);

    }
}
