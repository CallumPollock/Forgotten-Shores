using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;
    public Image image;
    public TextMeshProUGUI amountText;
    [HideInInspector] public Transform parentAfterDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        if (GetComponentInParent<EquipmentSlot>())
            GameState.instance.player.EquipHat(item.icon);
    }

    public void SetItem(Item itemToSetTo)
    {
        item = itemToSetTo;
        image.sprite = item.icon;

        UpdateStack();
    }

    public void UpdateStack()
    {
        if (item.stack <= 1)
            amountText.text = "";
        else
            amountText.text = item.stack.ToString();
    }

    // Start is called before the first frame update
    void Awake()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
