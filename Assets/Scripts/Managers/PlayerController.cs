using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    float speed;
    Rigidbody2D body;


    [Header("UI Elements")]
    [SerializeField]
    GameObject inventoryScreen;

    Player player;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        

        foreach(Hand hand in player.GetHands())
        {
            if(hand.GetIsHitting())
                continue;

            Vector2 direction = mousePosition - hand.transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            hand.transform.eulerAngles = new Vector3(0, 0, angle + hand.GetHandDirectionOffset());

            if (hand.GetEquippedItem() != null)
            {
                if (hand.GetEquippedItem().GetType() == typeof(BuildingItem))
                {
                    hand.GetEquippedItemTransform().position = new Vector3(mousePosition.x, mousePosition.y);
                }
            }
        }
        

        body.velocity = new Vector2(movement.x, movement.y) * speed;

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if(Input.GetMouseButtonDown(0))
        {
            player.GetHands()[0].Hit();
            
        }
        if(Input.GetMouseButtonDown(1))
        {
            player.GetHands()[1].Hit();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            player.ThrowEquipped(0);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            player.Interact();
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            player.ScrollEquippedItem(0, 1);
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            player.ScrollEquippedItem(0, -1);
    }

    public void ToggleInventory()
    {
        inventoryScreen.SetActive(!inventoryScreen.activeSelf);
    }

   
}
