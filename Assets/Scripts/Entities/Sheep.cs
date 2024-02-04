using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Entity
{
    void Start()
    {
        maxHealth = 30;
        health = 30;
        dropChance = 0.1f;
    }

    Transform target;

    private void Update()
    {
        RaycastHit2D[] sight = Physics2D.CircleCastAll(transform.position, 2f, Vector2.zero);

        foreach (RaycastHit2D hit in sight)
        {
            if (hit.collider.GetComponent<Player>())
            {
                target = hit.collider.transform;
                transform.position = Vector2.MoveTowards(transform.position, target.position, -1 * Time.deltaTime * speed);
            }
        }



    }
}
