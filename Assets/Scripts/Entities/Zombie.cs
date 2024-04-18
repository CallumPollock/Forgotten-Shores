using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Humanlike
{
    public override void Start()
    {
        StartCoroutine(Attack());
        base.Start();

    }
    
    

}
