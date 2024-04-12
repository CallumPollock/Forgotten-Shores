using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeButton : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI itemName, description;
    [SerializeField] Image icon;
    public Transform recipeContainer;

    public void InitialiseRecipeButton(Item item)
    {
        itemName.text = item.name;
        description.text = item.data.description;
        icon.sprite = Item.GetItemIcon(item.data);
    }


}
