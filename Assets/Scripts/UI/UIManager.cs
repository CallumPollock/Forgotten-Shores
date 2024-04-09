using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject interactionUI;
    [SerializeField] TextMeshProUGUI interactionText;
    [SerializeField] GameObject respawnScreen;
    [SerializeField] TextMeshProUGUI levelText, expText;
    [SerializeField] GameObject inventoryScreen;

    [SerializeField] GameObject buildingItemGO;
    [SerializeField] TextMeshProUGUI buildingItemText;

    [SerializeField] GameObject navButton;
    [SerializeField] Transform content;
    [SerializeField] Transform navigationContainer;
    [SerializeField] Transform currentActiveTab;

    [SerializeField] List<Image> uiImageComponents;

    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject debugMenu;
    [SerializeField] Transform debugScrollContent;

    [SerializeField] Transform hotbarContainer;

    [SerializeField] Transform worldSpaceCanvas;
    [SerializeField] GameObject entityHealthBarPrefab;


    [SerializeField] TextMeshProUGUI nowPlayingText;

    public static Action OnRespawnButtonClick;
    public static Action<BuildingItem> OnOpenCraftingMenu;

    List<HealthBar> healthBarPool = new List<HealthBar>();

    private void Awake()
    {
        foreach (Transform child in content)
        {
            if (child.tag == "Menu")
            {
                Button newNavButton = Instantiate(navButton).GetComponent<Button>();
                uiImageComponents.Add(newNavButton.GetComponent<Image>());
                newNavButton.transform.SetParent(navigationContainer);
                newNavButton.transform.localScale = Vector3.one;
                newNavButton.onClick.AddListener(delegate { SetActiveTab(child); });
                newNavButton.GetComponentInChildren<TextMeshProUGUI>().text = child.name;
                child.gameObject.SetActive(false);
                
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Player.PlayerEnterRangeOfBuilding += SetInteractionUI;
        Player.PlayerExitRangeOfBuilding += DisableInteractionUI;
        Player.OnPlayerDied += EnableDeathScreen;

        Player.OnExpUp += UpdateExp;
        Player.OnLevelUp += UpdateLevel;
        Player.PlayerInventoryChanged += UpdateHotbar;

        PlayerController.ToggleInventory += ToggleInventory;
        PlayerController.PressedPause += TogglePause;
        PlayerController.DebugKey += ToggleDebug;
        //PlayerController.SpawnHealthBar += AddEntityHealthBar;
        Entity.TriggerEntityInfo += AddEntityHealthBar;

        MusicManager.onPlayMusicTrack += MusicPlaying;

        currentActiveTab = content.GetChild(0);

        foreach (Item item in Resources.LoadAll<Item>("ScriptableObjects/Items"))
        {
            Button newNavButton = Instantiate(navButton).GetComponent<Button>();

            newNavButton.transform.SetParent(debugScrollContent);
            newNavButton.transform.localScale = Vector3.one;
            newNavButton.onClick.AddListener(delegate { DebugSpawnItem(item); });
            newNavButton.GetComponentInChildren<TextMeshProUGUI>().text = "DEBUG: Spawn " + item.name;
        }
    }

    private void MusicPlaying(AudioClip clip) { StartCoroutine(NowPlaying(clip)); }

    private IEnumerator NowPlaying(AudioClip clip)
    {
        
        nowPlayingText.text = "Now Playing... " + clip.name;
        yield return new WaitForSeconds(6f);
        nowPlayingText.text = "";
    }

    private void AddEntityHealthBar(Entity entity)
    {
        foreach(HealthBar healthBar in healthBarPool)
        {
            if (healthBar.activeEntity == entity)
                return;
            else if (!healthBar.isActiveAndEnabled)
            {
                healthBar.UpdateEntityInfo(entity);
                healthBar.transform.position = entity.transform.position;
                return;
            }
        }

        HealthBar newHealthbar = Instantiate(entityHealthBarPrefab).GetComponent<HealthBar>();
        healthBarPool.Add(newHealthbar);

        newHealthbar.transform.SetParent(worldSpaceCanvas);
        newHealthbar.UpdateEntityInfo(entity);
        newHealthbar.transform.position = entity.transform.position;

    }

    public void TogglePause()
    {
        pauseScreen.SetActive(!pauseScreen.activeSelf);
        Time.timeScale = pauseScreen.activeSelf ? 0.0f : 1.0f;
    }

    private void SetActiveTab(Transform tab)
    {
        currentActiveTab.gameObject.SetActive(false);
        currentActiveTab = tab;
        currentActiveTab.gameObject.SetActive(true);
    }

    void SetInteractionUI(BuildingEntity _buildingEntity)
    {
        interactionUI.SetActive(true);
        interactionText.text = _buildingEntity.name;
    }

    void DisableInteractionUI()
    {
        interactionUI.SetActive(false);
        buildingItemGO.SetActive(false);
        inventoryScreen.SetActive(false);
    }

    void EnableDeathScreen(Entity _entity)
    {
        respawnScreen.SetActive(true);
    }

    void UpdateExp(int _experience, int _experienceToNextLevel)
    {
        expText.text = _experience + "/" + _experienceToNextLevel;
    }

    void UpdateLevel(int _level)
    {
        levelText.text = _level.ToString();
    }

    public void OnClickRespawn()
    {
        respawnScreen.SetActive(false);
        OnRespawnButtonClick?.Invoke();
    }

    void UpdateHotbar(List<Item> inventory, int currentEquippedIndex)
    {
        hotbarContainer.gameObject.SetActive(true);

        for(int i = 0; i < hotbarContainer.childCount; i++)
        {
            Sprite itemIcon = null;
            string itemCount = "";
            int invIndex = currentEquippedIndex - 3 + i;
            if(invIndex < 0)
            {

            }
            else if (invIndex < inventory.Count)
            {
                itemIcon = inventory[invIndex].icon;

                if (inventory[invIndex].stack > 1) itemCount = inventory[invIndex].stack.ToString();
            }


            hotbarContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = itemIcon;
            hotbarContainer.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = itemCount;
        }

        //hotbarContainer.GetChild(hotbarContainer.childCount / 2 % 100).GetChild(0).GetComponent<Image>().sprite = inventory[currentEquippedIndex].icon;
    }

    void ToggleInventory(BuildingItem buildingItem)
    {
        inventoryScreen.SetActive(!inventoryScreen.activeSelf);
        

        if(inventoryScreen.activeSelf)
        {
            OnOpenCraftingMenu?.Invoke(buildingItem);

            Color color;

            if(buildingItem == null)
            {
                color = new Color(0f, 0.682353f, 1f);
            }
            else
            {
                color = buildingItem.color;
            }

            foreach (Image image in uiImageComponents)
            {
                image.color = color;
            }

        }

        if(buildingItem != null)
        {
            buildingItemGO.SetActive(inventoryScreen.activeSelf);
            buildingItemText.text = buildingItem.name;
            SetActiveTab(content.GetChild(1));
        }
        else
        {
            SetActiveTab(content.GetChild(0));
        }
    }

    private void DebugSpawnItem(Item item)
    {
        GameObject newDroppedObject = new GameObject();
        newDroppedObject.name = item.name;
        newDroppedObject.AddComponent<DroppedItem>().SetAsNewItem(item);
        newDroppedObject.AddComponent<PolygonCollider2D>().isTrigger = true;
        newDroppedObject.transform.position = Camera.main.ScreenToWorldPoint(Vector3.one);

        Rigidbody2D droppedItemRB = newDroppedObject.AddComponent<Rigidbody2D>();
        droppedItemRB.gravityScale = 0f;
        droppedItemRB.drag = 5f;
    }

    private void ToggleDebug()
    {
        debugMenu.SetActive(!debugMenu.activeSelf);

        if (!debugMenu.activeSelf) return;

        
    }
}
