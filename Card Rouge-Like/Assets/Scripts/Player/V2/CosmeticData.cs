using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Cosmetics/Cosmetic")]
public class CosmeticData : ScriptableObject
{
    public string cosmeticName;

    public Sprite[] idleDown, idleLeft, idleRight, idleUp;
    public Sprite[] runDown, runLeft, runRight, runUp;

    public bool[] idleDownFlipX, idleLeftFlipX, idleRightFlipX, idleUpFlipX;
    public bool[] runDownFlipX, runLeftFlipX, runRightFlipX, runUpFlipX;
}
