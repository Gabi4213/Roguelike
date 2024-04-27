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
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rarityBorderImage.gameObject.SetActive(false);
        transform.position = Input.mousePosition;

        transform.localScale = new Vector2(1f, 1f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rarityBorderImage.gameObject.SetActive(true);
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        transform.localScale = new Vector2(1f, 1f);
    }
}
