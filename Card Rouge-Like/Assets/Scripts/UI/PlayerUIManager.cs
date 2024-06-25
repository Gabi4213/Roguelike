using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [Header("Player Health")]
    public Image healthBarFill;
    public Color healthyColor, moderateColor, criticalColor;

    [Header("Player Stats")]
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
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        float currentHealth = PlayerStatistics.instance.currentHealth;
        float maxHealth = PlayerStatistics.instance.health;

        healthBarFill.fillAmount = currentHealth / maxHealth;

        if (currentHealth >= maxHealth * 0.5f)
        {
            healthBarFill.color = healthyColor;
        }
        else if (currentHealth >= maxHealth * 0.2f)
        {
            healthBarFill.color = moderateColor;
        }
        else
        {
            healthBarFill.color = criticalColor;
        }
    }
}
