using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    bool attackDelay = false;
    

    private void Update()
    {

            
    }

    IEnumerator Attack(Player player)
    {
        attackDelay = true;
        player.ModifyHealth(-4);
        yield return new WaitForSeconds(1f);
        attackDelay = false;
    }
}
