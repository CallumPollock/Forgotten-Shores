using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NPC : Humanlike
{

    Rigidbody2D rb;
    [SerializeField]Vector2 moveToPos;
    [SerializeField] DialogueRunner dr;

    public override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerController.TriggerTestCutscene += TestCutscene;
        base.Start();
    }

    void TestCutscene(Transform player)
    {
        moveToPos = player.position + Vector3.up;
        dr.StartDialogue("FirstNight");
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
