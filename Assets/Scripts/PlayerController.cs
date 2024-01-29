using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player), typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    Player player;
    [SerializeField]
    float speed;
    Rigidbody2D body;


    [Header("UI Elements")]
    [SerializeField]
    GameObject inventoryScreen;

    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponent<Player>();
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement;
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        //movement *= speed * Time.deltaTime;

        //transform.position = new Vector2(transform.position.x + movement.x, transform.position.y + movement.y);
        body.velocity = new Vector2(movement.x, movement.y) * speed;

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if(Input.GetMouseButtonDown(0))
        {
            
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<Entity>())
                {
                    Debug.Log("Click on " + hit.collider.name);
                    player.PlayerInteractWith(hit.collider.GetComponent<Entity>());
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryScreen.SetActive(!inventoryScreen.activeSelf);
        }

    }
}
