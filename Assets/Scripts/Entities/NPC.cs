using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Humanlike
{

    Rigidbody2D rb;
    [SerializeField] Vector2 movement;

    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Update()
    {
        base.Update();
        rb.velocity = movement * data.speed;
    }

    public void ExitScene()
    {
        movement.y = 1f;
    }    
}
