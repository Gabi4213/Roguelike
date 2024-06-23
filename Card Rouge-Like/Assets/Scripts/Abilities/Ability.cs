using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Ability")]
public class Ability : ScriptableObject
{
    [Header("Basic Information")]
    public string abilityName;
    public string instantiatedName;
    public string description;
    public float damage;
    public float cooldown;
    public float duration;
    public AbilityLevel abilityLevel;

    [Header("Gameplay")]
    public GameObject usableAbility;

    [Header("UI")]
    public Sprite image;
}

public enum AbilityLevel
{
    Novice,
    Apprentice,
    Adept,
    Expert,
    Master,
    Grandmaster,
    Legendary,
    Mythical,
    Divine,
    Ultimate
}


