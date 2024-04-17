using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Humanlike
{

    Rigidbody2D rb;
    [SerializeField]Vector2 moveToPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void TestCutscene(Transform player)
    {
        transform.position = player.position;
    }

    public override void Update()
    {
        base.Update();
        rb.MovePosition(Vector2.MoveTowards(rb.position, moveToPos, data.speed));
    }

    public void ExitScene()
    {
        moveToPos = new Vector2(-42.23f, 50f);
    }    
}
