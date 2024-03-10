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
}

public enum ItemType
{
    Consumable,
    Resource,
    Weapon,
    Armor, 
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
    God,
    Angelic,
    Demonic,
    Glorious
}

