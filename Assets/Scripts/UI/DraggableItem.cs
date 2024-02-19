using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    [SerializeField] Item item;
    public Image image;
    public TextMeshProUGUI amountText;

    [SerializeField] GameObject tooltip;
    [SerializeField] TextMeshProUGUI tooltipTitle, tooltipDescription;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;
        tooltip.SetActive(true);
        tooltipTitle.text = item.name;
        tooltipDescription.text = item.description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GetComponentInParent<InventorySlot>())
            GetComponentInParent<InventorySlot>().OnItemRemovedFromSlot();

        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Item oldItem = item;
        SetItem(eventData.pointerDrag.GetComponent<DraggableItem>().item);
        eventData.pointerDrag.GetComponent<DraggableItem>().SetItem(oldItem);
    }

    public void SetItem(Item itemToSetTo)
    {
        if(itemToSetTo == null)
        {
            image.sprite = null;
            image.color = new Color(0f, 0f, 0f, 0f);
            return;
        }

        item = itemToSetTo;
        image.sprite = item.icon;
        image.color = item.color;

        UpdateStack();
    }

    public Item GetItem() { return item; }

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

    
}
