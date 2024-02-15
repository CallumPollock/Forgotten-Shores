using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    FollowTarget followTarget;

    public override void Awake()
    {
        followTarget = GetComponent<FollowTarget>();
        base.Awake();
        
        
    }


    private void Start()
    {
        StartCoroutine(Attack());
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

    IEnumerator Attack()
    {
        GetHands()[Random.Range(0, GetHands().Count)].Hit();
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(Attack());
    }

}
