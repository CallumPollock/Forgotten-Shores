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

    // Start is called before the first frame update
    void Start()
    {
        UpdateCraftMenu();
    }

    public void UpdateCraftMenu()
    {
        foreach (Item recipe in recipeBook)
        {
            RecipeButton newRecipeButton = Instantiate(recipeButton).GetComponent<RecipeButton>();
            newRecipeButton.InitialiseRecipeButton(recipe);
            newRecipeButton.transform.SetParent(viewport.transform, false);
            newRecipeButton.GetComponent<Button>().onClick.AddListener(delegate { UpdatePreview(recipe); });

            foreach(Item.Recipe ingredient in recipe.recipe)
            {
                GameObject newIngredient = Instantiate(ingredientObject);
                newIngredient.GetComponentInChildren<Image>().sprite = ingredient.ingredient.icon;
                newIngredient.GetComponentInChildren<TextMeshProUGUI>().text = ingredient.ingredient.name + " (" + ingredient.amount + ")";
                newIngredient.transform.SetParent(newRecipeButton.recipeContainer);
            }
        }
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

        foreach (Item.Recipe ingredient in itemSelection.recipe)
        {
            GameObject newIngredient = Instantiate(ingredientObject);
            newIngredient.GetComponentInChildren<Image>().sprite = ingredient.ingredient.icon;
            newIngredient.GetComponentInChildren<TextMeshProUGUI>().text = ingredient.ingredient.name + " (" + ingredient.amount + ")";
            newIngredient.transform.SetParent(recipePreviewContainer);
        }
    }
}
