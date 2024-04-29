using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ItemType[] allowedItemTypes;

    RectTransform rect;
    Vector2 originalScale;
    Vector2 desiredScale;
    public float ScaleFactor = 1.20f;
    public bool triggerPlayerStats = false;

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

            ItemStatsSetUp(inventoryItem.item);
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

            for(int i=0; i < PlayerUIManager.instance.statsText.Length; i++)
            {
                PlayerUIManager.instance.statsText[i].text = "";
            }
        }
    }

    public void ItemStatsSetUp(Item item)
    {
        if(item.type == ItemType.Weapon || item.type == ItemType.Shield)
        {
            PlayerUIManager.instance.statsText[0].text = item.meleeDamage.ToString() + " Melee Damage";
            PlayerUIManager.instance.statsText[1].text = item.magicDamage.ToString() + " Magic Damage";
            PlayerUIManager.instance.statsText[2].text = item.criticalStrike.ToString() + " Critical Strike";
            PlayerUIManager.instance.statsText[3].text = item.attackSpeed.ToString() + " Attack Speed";
            PlayerUIManager.instance.statsText[4].text = item.movementSpeed.ToString() + " Movement Speed";
            PlayerUIManager.instance.statsText[5].text = item.stability.ToString() + " Stability";
            PlayerUIManager.instance.statsText[6].text = item.defence.ToString() + " Defence";
            PlayerUIManager.instance.statsText[7].text = item.projectileSpeed.ToString() + " Projectile Speed";
            PlayerUIManager.instance.statsText[8].text = item.projectileLifetime.ToString() + " Projectile Lifetime";

            UpdateTextColor("meleeDamage", PlayerStatistics.instance.meleeDamage, item.meleeDamage, PlayerUIManager.instance.statsText[0]);
            UpdateTextColor("magicDamage", PlayerStatistics.instance.magicDamage, item.magicDamage, PlayerUIManager.instance.statsText[1]);
            UpdateTextColor("criticalStrike", PlayerStatistics.instance.criticalStrike, item.criticalStrike, PlayerUIManager.instance.statsText[2]);
            UpdateTextColor("attackSpeed", PlayerStatistics.instance.attackSpeed, item.attackSpeed, PlayerUIManager.instance.statsText[3]);
            UpdateTextColor("moveSpeed", PlayerStatistics.instance.moveSpeed, item.movementSpeed, PlayerUIManager.instance.statsText[4]);
            UpdateTextColor("stability", PlayerStatistics.instance.stability, item.stability, PlayerUIManager.instance.statsText[5]);
            UpdateTextColor("defence", PlayerStatistics.instance.defence, item.defence, PlayerUIManager.instance.statsText[6]);
            UpdateTextColor("projectileLifetime", PlayerStatistics.instance.projectileSpeed, item.projectileSpeed, PlayerUIManager.instance.statsText[7]);
            UpdateTextColor("projectileLifetime", PlayerStatistics.instance.projectileLifetime, item.projectileLifetime, PlayerUIManager.instance.statsText[8]);
        }
    }

    void UpdateTextColor(string itemStatName, float playerStat, float itemStat, TextMeshProUGUI textToUpdate)
    {
        //item stats that are better when lower
        if(itemStatName == "attackSpeed")
        {
            if (playerStat == itemStat)
            {
                textToUpdate.color = PlayerUIManager.instance.defaultStatsColor;
            }
            else if (playerStat < itemStat && playerStat != 0)
            {
                textToUpdate.color = PlayerUIManager.instance.decreasedStatsColor;
            }
            else if (playerStat > itemStat)
            {
                textToUpdate.color = PlayerUIManager.instance.increasedStatsColor;
            }
            else if(playerStat == 0)
            {
                textToUpdate.color = PlayerUIManager.instance.increasedStatsColor;
            }
        }
        else
        {
            if (playerStat == itemStat)
            {
                textToUpdate.color = PlayerUIManager.instance.defaultStatsColor;
            }
            else if (playerStat > itemStat)
            {
                textToUpdate.color = PlayerUIManager.instance.decreasedStatsColor;
            }
            else if (playerStat < itemStat)
            {
                textToUpdate.color = PlayerUIManager.instance.increasedStatsColor;
            }
        }
    }
}
