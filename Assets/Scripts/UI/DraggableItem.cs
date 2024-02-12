using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Item item;
    public Image image;
    public TextMeshProUGUI amountText;
    [HideInInspector] public Transform parentAfterDrag;

    [SerializeField] GameObject tooltip;
    [SerializeField] TextMeshProUGUI tooltipTitle, tooltipDescription;

    public void OnPointerEnter(PointerEventData eventData)
    {
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
    }

    public void SetItem(Item itemToSetTo)
    {
        item = itemToSetTo;
        image.sprite = item.icon;

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
