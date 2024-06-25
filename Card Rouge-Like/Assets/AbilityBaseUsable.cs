using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBaseUsable : MonoBehaviour
{
    [Header("General Information")]
    public Ability ability;
    public float currentCooldown;
    [Header("0 = first ability, 1 = second & 2= third")]
    public int abilityIndex;
    public bool inUse;
    private bool canUse;

    private InventoryAbility currentInventoryAbility;

    private void Start()
    {
        currentInventoryAbility = AbilityManager.instance.abilitySlots[abilityIndex].GetComponentInChildren<InventoryAbility>();
    }

    private void Update()
    {
        if (currentCooldown > 0)
        {
            canUse = false;
            currentCooldown -= Time.deltaTime;
        }
        else
        {
            canUse = true;
            currentCooldown = 0.0f;
        }

        if (InputManager.abilities[abilityIndex] && canUse)
        {
            inUse = true;
            currentCooldown = ability.cooldown;
        }

        currentInventoryAbility.cooldownImage.fillAmount = (currentCooldown / 10.0f);

        if(currentCooldown > 0)
        {
            currentInventoryAbility.cooldownText.text = currentCooldown.ToString("F0");
        }
        else
        {
            currentInventoryAbility.cooldownText.text = "";
        }
    }
}
