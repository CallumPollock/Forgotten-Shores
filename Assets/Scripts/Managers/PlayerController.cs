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

    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] Transform equippedItem;

    [SerializeField] Hand leftHand, rightHand;

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
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();

        //movement *= speed * Time.deltaTime;

        //transform.position = new Vector2(transform.position.x + movement.x, transform.position.y + movement.y);
        body.velocity = new Vector2(movement.x, movement.y) * speed;

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if(Input.GetMouseButtonDown(0))
        {
            leftHand.Hit();
            
        }
        if(Input.GetMouseButtonDown(1))
        {
            rightHand.Hit();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            //inventoryManager.DropItem();
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            inventoryManager.ChangeEquippedItem(1);
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            inventoryManager.ChangeEquippedItem(-1);
    }

    public void ToggleInventory()
    {
        inventoryScreen.SetActive(!inventoryScreen.activeSelf);
    }

   
}
