using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Humanlike
{
    void Start()
    {
        StartCoroutine(Attack());

    }
    
    

}
