using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    float speed;
    
    Player player;

    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement;
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        movement *= speed * Time.deltaTime;

        transform.position = new Vector2(transform.position.x + movement.x, transform.position.y + movement.y);

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

        

    }
}
