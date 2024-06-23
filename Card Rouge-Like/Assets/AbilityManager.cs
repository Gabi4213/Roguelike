using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;
    public AbilitySlot[] abilitySlots;

    private void Start()
    {
        if (!instance)
        {
            instance = this;
        }
    }
}
