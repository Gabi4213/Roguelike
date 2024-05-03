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

    private GameObject[] hearts;

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
        hearts = new GameObject[PlayerStatistics.instance.health];
        for (int i =0; i < PlayerStatistics.instance.health; i++)
        {
            GameObject go =  Instantiate(healthUIPrefab, healthUIGroup);
            hearts[i] = go;
            if (i < PlayerStatistics.instance.currentHealth)
            {
                go.GetComponent<HeartSlot>().filled = true;
            }
            else
            {
                go.GetComponent<HeartSlot>().filled = false;
            }
        }
    }

    public void UpdateHealthUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < PlayerStatistics.instance.currentHealth)
            {
                hearts[i].GetComponent<HeartSlot>().filled = true;
            }
            else
            {
                hearts[i].GetComponent<HeartSlot>().filled = false;
            }
        }
    }
}
