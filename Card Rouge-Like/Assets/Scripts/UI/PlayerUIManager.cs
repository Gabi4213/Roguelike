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
    public TextMeshProUGUI healthBarText;

    [Header("Player Manna")]
    public Image mannaBarFill;

    [Header("Player Stats")]
    public GameObject itemInfoObject;
    public TextMeshProUGUI itemNameText, itemDescriptionText;
    public TextMeshProUGUI[] statsText;
    public Color defaultStatsColor, increasedStatsColor, decreasedStatsColor;
    public Color[] rarityTextColors;

    [Header("Soul Shards")]
    public TextMeshProUGUI soulShardsText;

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
        UpdateMannaUI();
        UpdateSoulsUI();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && itemInfoObject.activeSelf)
        {
            itemInfoObject.SetActive(false);
        }
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

        healthBarText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    public void UpdateMannaUI()
    {
        float currentManna = PlayerStatistics.instance.currentManna;
        float maxManna = PlayerStatistics.instance.manna;

        mannaBarFill.fillAmount = currentManna / maxManna;
    }

    public void UpdateSoulsUI()
    {
        soulShardsText.text = PlayerStatistics.instance.soulFragments.ToString();
    }
}
