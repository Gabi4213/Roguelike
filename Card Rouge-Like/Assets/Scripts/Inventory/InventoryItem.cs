using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image image;
    public Image rarityBorderImage;
    public Sprite[] rarityBorders;
    public TextMeshProUGUI countText;

    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        SetUpRarityBorder();
        RefreshCount();
    }

    public void SetUpRarityBorder()
    {
        switch (item.rarity)
        {
            case Rarity.Base:
                rarityBorderImage.sprite = rarityBorders[0];
                break;
            case Rarity.Uncommon:
                rarityBorderImage.sprite = rarityBorders[1];
                break;
            case Rarity.Rare:
                rarityBorderImage.sprite = rarityBorders[2];
                break;
            case Rarity.Ultimate:
                rarityBorderImage.sprite = rarityBorders[3];
                break;
            case Rarity.Godly:
                rarityBorderImage.sprite = rarityBorders[4];
                break;
            case Rarity.Angelic:
                rarityBorderImage.sprite = rarityBorders[5];
                break;
            case Rarity.Demonic:
                rarityBorderImage.sprite = rarityBorders[6];
                break;
            case Rarity.Glorious:
                rarityBorderImage.sprite = rarityBorders[7];
                break;
        }
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");

        rarityBorderImage.gameObject.SetActive(false);
        transform.position = Input.mousePosition;

        transform.localScale = new Vector2(1f, 1f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");

        // Check if the pointer is outside of a slot
        if (IsPointerOverUIObject())
        {
            DropItem();
        }
        else
        {
            rarityBorderImage.gameObject.SetActive(true);
            image.raycastTarget = true;
            transform.SetParent(parentAfterDrag);
            transform.localScale = new Vector2(1f, 1f);
        }
    }

    private bool IsPointerOverUIObject()
    {
        // Check if the pointer is over any UI object using EventSystem
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.tag == "Background" && !results[i].gameObject.GetComponent<InventorySlot>())
            {
                return true;
            }
        }

        return false;
    }

    public void DropItem()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            Vector3 initialDropPosition = player.transform.position + player.transform.right * 1.5f; // Adding to the X direction
            Vector3 dropPosition = initialDropPosition;
            bool foundValidDropPosition = false;

            // Raycast to check if the initial drop position is obstructed
            RaycastHit2D hit = Physics2D.Raycast(initialDropPosition, Vector2.down, 2.0f);
            if (hit.collider == null)
            {
                foundValidDropPosition = true;
            }
            else
            {
                // If the initial drop position is obstructed, try the opposite direction
                dropPosition = player.transform.position - player.transform.right * 1.5f; // Subtracting from the X direction

                // Raycast again to check if the opposite direction is obstructed
                hit = Physics2D.Raycast(dropPosition, Vector2.down, 2.0f);
                if (hit.collider == null)
                {
                    foundValidDropPosition = true;
                }
                else if (hit.collider.GetComponent<PickupableItem>())
                {
                    //currently drops the item on top of one another TODO: offset them randomly so they dont overlap
                    foundValidDropPosition = true; 
                }
            }

            // If a valid drop position is found, instantiate the item
            if (foundValidDropPosition)
            {
                Instantiate(item.pickupablePrefab, dropPosition, Quaternion.identity);
                Destroy(gameObject);
            }
        }

        rarityBorderImage.gameObject.SetActive(true);
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        transform.localScale = new Vector2(1f, 1f);
    }
}

    
