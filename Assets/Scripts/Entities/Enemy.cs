using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    FollowTarget followTarget;

    [SerializeField] Transform body;

    public override void Awake()
    {
        followTarget = GetComponent<FollowTarget>();
        base.Awake();
        
        
    }


    private void Start()
    {
        StartCoroutine(Attack());
        body.localScale = new Vector3(Random.Range(0.2f, 1.5f), 1f);
    }
    private void Update()
    {
        if (followTarget.target == null) return;


        Vector2 direction = followTarget.target.transform.position - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);

        foreach (Hand hand in GetHands())
        {
            hand.transform.eulerAngles = new Vector3(0, 0, angle + hand.GetHandDirectionOffset());

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<DroppedItem>())
        {
            if (collision.GetComponent<DroppedItem>().item != null)
            {
                if (GetHands()[0].GetHeldItem() == null)
                {
                    GetHands()[0].EquipItemInHand(collision.GetComponent<DroppedItem>().item);
                    Destroy(collision.gameObject);

                }
            }

        }
    }
    
    IEnumerator Attack()
    {
        GetHands()[Random.Range(0, GetHands().Count)].Hit();
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(Attack());
    }

}
