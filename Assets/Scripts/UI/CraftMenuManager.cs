using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftMenuManager : MonoBehaviour
{
    [SerializeField] List<Item> recipeBook = new List<Item>();
    [SerializeField] GameObject recipeButton;
    [SerializeField] GameObject ingredientObject;
    [SerializeField] Transform viewport;

    [Header("Selection Preview")]
    [SerializeField] TextMeshProUGUI previewName;
    [SerializeField] TextMeshProUGUI previewDescription;
    [SerializeField] Image previewIcon;
    [SerializeField] Transform recipePreviewContainer;

    [SerializeField] Button craftButton;

    [SerializeField] private Player player;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCraftMenu();
        Player.OnPlayerSpawn += PlayerRespawn;
    }

    private void PlayerRespawn(Player _player) { player = _player; }

    public void UpdateCraftMenu()
    {
        foreach(Transform child in viewport)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in recipeBook)
        {
            RecipeButton newRecipeButton = Instantiate(recipeButton).GetComponent<RecipeButton>();
            newRecipeButton.InitialiseRecipeButton(item);
            newRecipeButton.transform.SetParent(viewport.transform, false);
            newRecipeButton.GetComponent<Button>().onClick.AddListener(delegate { UpdatePreview(item); });

            foreach(Item.Ingredient ingredient in item.recipe.ingredients)
            {
                GameObject newIngredient = Instantiate(ingredientObject);
                newIngredient.GetComponentInChildren<Image>().sprite = ingredient.item.icon;
                newIngredient.GetComponentInChildren<TextMeshProUGUI>().text = ingredient.item.name + " (" + ingredient.amount + ")";
                newIngredient.transform.SetParent(newRecipeButton.recipeContainer);
            }
        }
    }

    public bool CheckItemCraftable(Item itemToCraft)
    {
        foreach(Item.Ingredient ingredient in itemToCraft.recipe.ingredients)
        {
            if (player.GetItemFromInventory(ingredient.item.itemID) == null) return false;
            if (player.GetItemFromInventory(ingredient.item.itemID).stack < ingredient.amount) return false;
        }

        return true;
    }

    public void CraftItem(Item itemToCraft)
    {
        if (!CheckItemCraftable(itemToCraft)) return;


        GameObject newDroppedObject = new GameObject();
        newDroppedObject.AddComponent<DroppedItem>().SetAsNewItem(Instantiate(itemToCraft));
        newDroppedObject.AddComponent<PolygonCollider2D>().isTrigger = true;
        Rigidbody2D droppedItemRB = newDroppedObject.AddComponent<Rigidbody2D>();
        droppedItemRB.gravityScale = 0f;
        droppedItemRB.drag = 5f;


        newDroppedObject.transform.position = new Vector2(player.transform.position.x, player.transform.position.y) + Random.insideUnitCircle * 0.4f;
        newDroppedObject.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }

    public void UpdatePreview(Item itemSelection)
    {
        previewName.text = itemSelection.name;
        previewDescription.text = itemSelection.description;
        previewIcon.sprite = itemSelection.icon;

        foreach(Transform child in recipePreviewContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Item.Ingredient ingredient in itemSelection.recipe.ingredients)
        {
            GameObject newIngredient = Instantiate(ingredientObject);
            newIngredient.GetComponentInChildren<Image>().sprite = ingredient.item.icon;
            newIngredient.GetComponentInChildren<TextMeshProUGUI>().text = ingredient.item.name + " (" + ingredient.amount + ")";
            newIngredient.transform.SetParent(recipePreviewContainer);
        }

        craftButton.interactable = CheckItemCraftable(itemSelection);
        craftButton.onClick.AddListener(delegate { CraftItem(itemSelection); });

    }
}
