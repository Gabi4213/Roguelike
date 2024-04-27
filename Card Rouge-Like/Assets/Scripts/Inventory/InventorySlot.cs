using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ItemType[] allowedItemTypes;

    RectTransform rect;
    Vector2 originalScale;
    Vector2 desiredScale;
    public float ScaleFactor = 1.20f;

    private Image inventoryImage;

    public Sprite neutralSprite, hoveredSprite;

    void Start()
    {
        inventoryImage = GetComponent<Image>();
        rect = gameObject.GetComponent<RectTransform>();

        originalScale = rect.localScale;
        desiredScale = new Vector2(1, 1);
        inventoryImage.sprite = neutralSprite;
    }

    public void OnDrop(PointerEventData eventData)
    {
       //check if the slot is empty
        if(transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();

            foreach(ItemType type in allowedItemTypes)
            {
                if(type == inventoryItem.item.type)
                {
                    inventoryItem.parentAfterDrag = transform;
                }
            }
            return;
        }

        inventoryImage.sprite = neutralSprite;
    }
    private void Update()
    {
        rect.localScale = Vector2.Lerp(rect.localScale, desiredScale, Time.deltaTime * 5);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {       
        desiredScale = new Vector2(originalScale.x * ScaleFactor, originalScale.y * ScaleFactor);
        inventoryImage.sprite = neutralSprite;

        if (transform.childCount > 0)
        {
            InventoryItem inventoryItem = transform.GetChild(0).GetComponent<InventoryItem>();
            PlayerUIManager.instance.itemNameText.text = inventoryItem.item.itemName;
            PlayerUIManager.instance.itemDescriptionText.text = inventoryItem.item.description;
        }
        else
        {
            inventoryImage.sprite = hoveredSprite;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        desiredScale = new Vector2(originalScale.x, originalScale.y);
        inventoryImage.sprite = neutralSprite;

        if (transform.childCount > 0)
        {
            PlayerUIManager.instance.itemNameText.text = "";
            PlayerUIManager.instance.itemDescriptionText.text = "";
        }
    }
}
