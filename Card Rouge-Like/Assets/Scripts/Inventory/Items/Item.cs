using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class Item : ScriptableObject
{   
    [Header("Gameplay")]
    public ItemType type;
    public ActionType actionType;
    public GameObject usablePrefab;
    public GameObject pickupablePrefab;

    [Header("UI")]
    public Sprite image;
    public bool stackable = true;
    public int maxStackAmount = 0;
    public Color itemColor;

    [Header("Item Info")]
    public string itemName;
    public Rarity rarity;
    public string description;

    [Header("Item Stats")]
    public float damage;
    public float criticalStrike;
    public float attackSpeed;
    public float knockbackForce;
    public float movementSpeed;
    public float stability;
    public float defence;
    public float projectileSpeed;
    public float projectileLifetime;
}

public enum ItemType
{
    Consumable,
    Resource,
    MeleeWeapon,
    MagicWeapon,
    RangedWeapon,
    Armor, 
    Shield,
    Ammo,
    Other
}

public enum ActionType
{
    None,
    Consumable,
    Attack
}

public enum Rarity
{
    Base,
    Uncommon,
    Rare,
    Ultimate,
    Godly,
    Wraithboud,
    Angelic,
    Demonic,
    Glorious
}

