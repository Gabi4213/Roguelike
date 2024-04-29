using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    public Transform healthUIGroup;
    public GameObject healthUIPrefab;

    public TextMeshProUGUI itemNameText, itemDescriptionText;

    public TextMeshProUGUI[] statsText;

    public Color defaultStatsColor, increasedStatsColor, decreasedStatsColor;
    public Color[] rarityTextColors;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void Start()
    {
        SetUpHealthUI();
    }

    private void SetUpHealthUI()
    {
        for (int i =0; i < PlayerStatistics.instance.health; i++)
        {
            GameObject go =  Instantiate(healthUIPrefab, healthUIGroup);

            if(i < PlayerStatistics.instance.currentHealth)
            {
                go.GetComponent<HeartSlot>().filled = true;
            }
            else
            {
                go.GetComponent<HeartSlot>().filled = false;
            }
        }
    }
}
