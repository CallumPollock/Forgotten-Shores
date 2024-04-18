using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D body;


    [Header("UI Elements")]
    [SerializeField]
    GameObject inventoryScreen;

    Player player;

    public static Action<Data> ToggleInventory;
    public static Action PressedPause;
    public static Action DebugKey;
    public static Action<Entity> SpawnHealthBar;

    public static Action<Transform> TriggerTestCutscene;


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
                if (hand.GetEquippedItem().itemType == Item.ItemType.placeable)
                {
                    hand.GetEquippedItemTransform().position = GameState.instance.grid.GetCellCenterWorld(GameState.instance.grid.WorldToCell(mousePosition));
                    hand.GetEquippedItemTransform().rotation = Quaternion.Euler(0f, 0f, 0f);
                }
            }
        }
        

        body.velocity = movement * player.data.speed;

        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            player.GetHands()[0].Hit();
            
        }
        if(Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            player.GetHands()[1].Hit();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory?.Invoke(player.data);
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            player.ThrowEquipped(0);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            player.Interact();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PressedPause?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            DebugKey?.Invoke();
        }    

        if(Input.GetKeyDown(KeyCode.F2))
        {
            SpawnHealthBar?.Invoke(player);
        }

        if(Input.GetKeyDown(KeyCode.F3))
        {
            TriggerTestCutscene?.Invoke(transform);
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            player.ScrollEquippedItem(Input.GetKey(KeyCode.LeftShift) ? 1 : 0, 1);
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            player.ScrollEquippedItem(Input.GetKey(KeyCode.LeftShift) ? 1 : 0, -1);
    }

   
}
