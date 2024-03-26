using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    Transform target;


    private Vector3 targetPos;
    [SerializeField] float xOffset, yOffset;


    private void Start()
    {
        Player.OnPlayerSpawn += UpdatePlayerTransform;
    }

    public void UpdatePlayerTransform(Player player)
    {
        target = player.transform;
    }

    public void TargetSpeaker(string speaker)
    {
        target = GameObject.Find(speaker).transform;
        Debug.Log("Set target to " + target.name);
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = new Vector3(target.position.x + xOffset, target.position.y + yOffset, -10f);

        transform.position = Vector3.Lerp(transform.position, targetPos, 4f * Time.deltaTime);
    }
}
