using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Human
{

    Rigidbody2D rb;
    [SerializeField] Vector2 movement;

    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = movement * data.speed;
    }

    public void ExitScene()
    {
        movement.y = 1f;
    }    
}
