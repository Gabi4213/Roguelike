using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class AbilitySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform rect;
    Vector2 originalScale;
    Vector2 desiredScale;
    public float ScaleFactor = 1.20f;
    public bool triggerPlayerStats = false;

    private Image inventoryImage;
    public Sprite neutralSprite, hoveredSprite;

    public Ability currentAbiltiyInSlot;

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
        if (transform.childCount == 0)
        {
            InventoryAbility inventoryAbility = eventData.pointerDrag.GetComponent<InventoryAbility>();
            inventoryAbility.parentAfterDrag = transform;
        }
        inventoryImage.sprite = neutralSprite;

        Debug.Log("No slots detected below the item!");
    }
    private void Update()
    {
        if(transform.childCount > 0 && !currentAbiltiyInSlot)
        {
            currentAbiltiyInSlot = transform.GetChild(0).GetComponent<InventoryAbility>().ability;
        }

        if(transform.childCount  == 0 && currentAbiltiyInSlot)
        {
            currentAbiltiyInSlot = null;
        }

        rect.localScale = Vector2.Lerp(rect.localScale, desiredScale, Time.deltaTime * 5);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        desiredScale = new Vector2(originalScale.x * ScaleFactor, originalScale.y * ScaleFactor);
        inventoryImage.sprite = neutralSprite;


        if (transform.childCount > 0)
        {
            InventoryAbility inventoryAbility = transform.GetChild(0).GetComponent<InventoryAbility>();
            PlayerUIManager.instance.itemNameText.text = inventoryAbility.ability.abilityName;
            PlayerUIManager.instance.itemDescriptionText.text = inventoryAbility.ability.description;

            AbilityStatsSetUp(inventoryAbility.ability);
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

            for (int i = 0; i < PlayerUIManager.instance.statsText.Length; i++)
            {
                PlayerUIManager.instance.statsText[i].gameObject.SetActive(false);
            }
        }
    }

    public void RefreshItemDescription()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < PlayerUIManager.instance.statsText.Length; i++)
            {
                PlayerUIManager.instance.statsText[i].gameObject.SetActive(false);
            }
        }
    }

    public void AbilityStatsSetUp(Ability ability)
    {
        RefreshItemDescription();

        PlayerUIManager.instance.statsText[0].gameObject.SetActive(true);
        PlayerUIManager.instance.statsText[1].gameObject.SetActive(true);
        PlayerUIManager.instance.statsText[2].gameObject.SetActive(true);
        PlayerUIManager.instance.statsText[3].gameObject.SetActive(true);

        PlayerUIManager.instance.statsText[0].text = ability.damage.ToString() + "Damage";
        PlayerUIManager.instance.statsText[1].text = ability.cooldown.ToString() + "Cooldown Time";
        PlayerUIManager.instance.statsText[2].text = ability.duration.ToString() + "Duration";       
    }
}
