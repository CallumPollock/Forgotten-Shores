using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    Transform target;

    private void Update()
    {
        RaycastHit2D[] sight = Physics2D.CircleCastAll(transform.position, 5f, Vector2.zero );

        foreach(RaycastHit2D hit in sight)
        {
            if (hit.collider.GetComponent<Player>())
            {
                target = hit.collider.transform;
                transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * 0.8f);
            }
        }

        
            
    }
}
