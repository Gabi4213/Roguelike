using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GoldenTouchAbility : MonoBehaviour
{
    public Ability ability;
    public float currentCooldown;
    private bool canUse;


    private void Update()
    {
        if(currentCooldown > 0)
        {
            canUse = false;
            currentCooldown -= Time.deltaTime;
        }
        else
        {
            canUse = true;
            currentCooldown = 0.0f;
        }

        if(InputManager.ability1 && canUse)
        {
            currentCooldown = ability.cooldown;
        }

        //AbilityManager.instance.abilitySlots[0].cooldownImage.fillAmount = (currentCooldown / 10.0f);
        AbilityManager.instance.abilitySlots[0].GetComponentInChildren<InventoryAbility>().cooldownImage.fillAmount = (currentCooldown / 10.0f);
    }
}
