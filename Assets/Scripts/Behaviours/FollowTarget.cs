using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{

    //public Entity target;
    public float maxSightDistance;
    public float followDistance;
    public float speed;

    List<Entity> possibleTargets = new List<Entity>();
    Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void Update()
    {
        if (entity == null) return;


        if (maxSightDistance != 0)
        {
            RaycastHit2D[] sight = Physics2D.CircleCastAll(transform.position, maxSightDistance, Vector2.zero);
            possibleTargets.Clear();
            foreach (RaycastHit2D hit in sight)
            {
                if (hit.collider.GetComponentInParent<Entity>())
                {
                    possibleTargets.Add(hit.collider.GetComponentInParent<Entity>());

                }
            }
            if(possibleTargets.Count > 0)
            {
                entity.SetTarget(entity.GetBestTargetEntity(possibleTargets));

                if (entity.GetTarget() != null)
                {
                    if (Vector2.Distance(transform.position, entity.GetTarget().transform.position) > followDistance)
                        transform.position = Vector2.MoveTowards(transform.position, entity.GetTarget().transform.position, Time.deltaTime * speed);
                }
            }
            
            
        }
    }
}
