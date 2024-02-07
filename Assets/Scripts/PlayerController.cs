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

    [SerializeField] Collider2D weaponHitbox;

    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponent<Player>();
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mousePosition - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        equippedItem.transform.eulerAngles = new Vector3(0, 0, angle);
        Debug.DrawLine(equippedItem.transform.position, mousePosition);

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
            weaponHitbox.enabled = true;
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<Entity>())
                {
                    Debug.Log("Click on " + hit.collider.name);
                    
                }
            }
        }

        if(weaponHitbox.enabled)
        {
            if (Vector2.Distance(equippedItem.transform.position, transform.position) < 0.3f)
                equippedItem.position = Vector2.Lerp(equippedItem.position, mousePosition, Time.deltaTime*2f);
            else
                weaponHitbox.enabled = false;
        }
        else
            equippedItem.position = Vector2.Lerp(equippedItem.position, transform.position, Time.deltaTime*5f);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
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
