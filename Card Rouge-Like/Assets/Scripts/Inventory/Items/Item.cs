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

    [Header("UI")]
    public Sprite image;
    public bool stackable = true;
    public int maxStackAmount = 0;

    [Header("Item Info")]
    public string itemName;
    public Rarity rarity;
    public string description;

    [Header("Item Stats")]
    public float meleeDamage;
    public float magicDamage;
    public float criticalStrike;
    public float attackSpeed;
    public float movementSpeed;
}

public enum ItemType
{
    Consumable,
    Resource,
    Weapon,
    Armor, 
    Shield,
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
    Angelic,
    Demonic,
    Glorious
}

