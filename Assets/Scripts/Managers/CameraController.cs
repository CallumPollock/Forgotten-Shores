using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    Transform player;


    private Vector3 playerPos;
    [SerializeField] float xOffset, yOffset;


    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            playerPos = new Vector3(player.position.x + xOffset, player.position.y + yOffset, -10f);
        }
        else if(GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        transform.position = Vector3.Lerp(transform.position, playerPos, 4f * Time.deltaTime);
    }
}
