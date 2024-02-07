using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedItem : MonoBehaviour
{

    [SerializeField] Player player;


    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Entity>())
        {
            player.PlayerInteractWith(collision.GetComponent<Entity>());
        }    
        
    }
}
