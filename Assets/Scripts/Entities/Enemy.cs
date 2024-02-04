using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    Transform target;
    bool attackDelay = false;

    private void Update()
    {
        RaycastHit2D[] sight = Physics2D.CircleCastAll(transform.position, 3f, Vector2.zero);

        foreach(RaycastHit2D hit in sight)
        {
            if (hit.collider.GetComponent<Player>())
            {
                target = hit.collider.transform;
                if (Vector2.Distance(transform.position, target.position) > 0.3f)
                    transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
                else if(!attackDelay)
                    StartCoroutine(Attack(hit.collider.GetComponent<Player>()));

            }
        }
            
    }

    IEnumerator Attack(Player player)
    {
        attackDelay = true;
        player.ModifyHealth(-4);
        yield return new WaitForSeconds(1f);
        attackDelay = false;
    }
}
