using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    Transform player;

    [SerializeField] float xOffset, yOffset;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x+xOffset, player.position.y+yOffset, -10f);
    }
}
