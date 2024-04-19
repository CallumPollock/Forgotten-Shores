using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CraftMenuManager : MonoBehaviour
{
    [SerializeField] public List<Item> recipeBook = new List<Item>();
    [SerializeField] GameObject recipeButton;
    [SerializeField] GameObject ingredientObject;
    [SerializeField] Transform viewport;

    [Header("Selection Preview")]
    [SerializeField] TextMeshProUGUI previewName;
    [SerializeField] TextMeshProUGUI previewDescription;
    [SerializeField] Image previewIcon;
    [SerializeField] Transform recipePreviewContainer;

    [SerializeField] GameObject previewGO;
    [SerializeField] GameObject craftGO;

    [SerializeField] Button craftButton;

    [SerializeField] private Player player;
    private AudioSource audioSource;
    [SerializeField] AudioClip craftingClip;

    public static Action<Item> ItemCrafted;

    // Start is called before the first frame update
    void Start()
    {
        Player.OnPlayerSpawn += PlayerRespawn;
        UIManager.OnOpenCraftingMenu += UpdateCraftMenu;

        ObjectiveManager.CompletedObjective += UnlockNewRecipe;
        SaveLoadJSON.worldLoaded += LoadRecipes;

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void LoadRecipes(WorldData worldData)
    {
        foreach(string recipeName in worldData.unlockedRecipes)
        {
            recipeBook.Add(Resources.Load<Item>("ScriptableObjects/Items/" + recipeName));
        }
    }

    private void PlayerRespawn(Player _player) { player = _player; }

    public void UpdateCraftMenu(Data _data)
    {

        foreach(Transform child in viewport)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in recipeBook)
        {
            if(item.recipe.requiredBuilding != null)
            {
                if (item.recipe.requiredBuilding.name == _data.name)
                    CreateRecipeButtonUI(item);
            }
            else
            {
                CreateRecipeButtonUI(item);
            }
                
        }
    }

    private void CreateRecipeButtonUI(Item item)
    {
        RecipeButton newRecipeButton = Instantiate(recipeButton).GetComponent<RecipeButton>();
        newRecipeButton.InitialiseRecipeButton(item);
        newRecipeButton.transform.SetParent(viewport.transform, false);
        newRecipeButton.GetComponent<Button>().onClick.AddListener(delegate { UpdatePreview(item); });

        foreach (Item.Ingredient ingredient in item.recipe.ingredients)
        {
            GameObject newIngredient = Instantiate(ingredientObject);
            newIngredient.GetComponentInChildren<Image>().sprite = Item.GetItemIcon(ingredient.item.data);
            newIngredient.GetComponentInChildren<TextMeshProUGUI>().text = ingredient.item.name + " (" + ingredient.amount + ")";
            newIngredient.transform.SetParent(newRecipeButton.recipeContainer);
        }
    }

    void UnlockNewRecipe(Objective _objective)
    {
        foreach(Item newRecipe in _objective.unlocksRecipes)
        {
            recipeBook.Add(newRecipe);
            player.CreateInfoText("New Recipe: " + newRecipe.name, Color.green, 8f, 1f);
        }
    }

    public bool CheckItemCraftable(Item itemToCraft)
    {
        foreach(Item.Ingredient ingredient in itemToCraft.recipe.ingredients)
        {
            if (player.GetItemFromInventory(ingredient.item.data.name) == null) return false;
            if (player.GetItemFromInventory(ingredient.item.data.name).stack < ingredient.amount) return false;
        }

        return true;
    }

    public void CraftItem(Item itemToCraft)
    {
        if (!CheckItemCraftable(itemToCraft)) return;


        GameObject newDroppedObject = new GameObject();
        newDroppedObject.AddComponent<DroppedItem>().SetAsNewItem(itemToCraft.data);
        newDroppedObject.AddComponent<PolygonCollider2D>().isTrigger = true;
        Rigidbody2D droppedItemRB = newDroppedObject.AddComponent<Rigidbody2D>();
        droppedItemRB.gravityScale = 0f;
        droppedItemRB.drag = 5f;


        newDroppedObject.transform.position = new Vector2(player.transform.position.x, player.transform.position.y) + UnityEngine.Random.insideUnitCircle * 0.4f;
        newDroppedObject.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));

        ItemCrafted?.Invoke(itemToCraft);
        UpdatePreview(itemToCraft);
        audioSource.PlayOneShot(craftingClip);
    }

    public void UpdatePreview(Item itemSelection)
    {
        previewGO.SetActive(true);
        craftGO.SetActive(true);

        previewName.text = itemSelection.name;
        previewDescription.text = itemSelection.data.description;
        previewIcon.sprite = Item.GetItemIcon(itemSelection.data);

        foreach(Transform child in recipePreviewContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Item.Ingredient ingredient in itemSelection.recipe.ingredients)
        {
            GameObject newIngredient = Instantiate(ingredientObject);
            newIngredient.GetComponentInChildren<Image>().sprite = Item.GetItemIcon(ingredient.item.data);
            newIngredient.GetComponentInChildren<TextMeshProUGUI>().text = ingredient.item.name + " (" + ingredient.amount + ")";
            newIngredient.transform.SetParent(recipePreviewContainer);
        }

        craftButton.interactable = CheckItemCraftable(itemSelection);
        craftButton.onClick.RemoveAllListeners();
        craftButton.onClick.AddListener(delegate { CraftItem(itemSelection); });

    }
}
