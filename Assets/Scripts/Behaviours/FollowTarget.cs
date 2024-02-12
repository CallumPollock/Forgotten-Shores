using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{

    public Transform target;
    public float maxSightDistance;
    public float followDistance;
    public float speed;

    private void Update()
    {
        if (maxSightDistance != 0)
        {
            RaycastHit2D[] sight = Physics2D.CircleCastAll(transform.position, maxSightDistance, Vector2.zero);

            foreach (RaycastHit2D hit in sight)
            {
                if (hit.collider.GetComponent<Player>())
                {
                    target = hit.collider.transform;
                    if (Vector2.Distance(transform.position, target.position) > followDistance)
                        transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * speed);

                }
            }
        }
    }
}
