using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHealth : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;

    public GameObject healthBar;
    public GameObject healthBarFill;
    private SpriteRenderer healthBarFillSprite;

    public Color healthyColor, moderateColor, criticalColor;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBarFillSprite = healthBarFill.GetComponent<SpriteRenderer>();
        UpdateHealthBar();
    }

    private void Update()
    {
        if (currentHealth == maxHealth)
        {
            if (healthBar.activeSelf)
            {
                healthBar.SetActive(false);
            }
        }
        else if (currentHealth != maxHealth)
        {
            if (!healthBar.activeSelf)
            {
                healthBar.SetActive(true);
            }
        }
    }

    // Function to update the health bar fill size and position based on current health.
    void UpdateHealthBar()
    {
        float fillSize = currentHealth / maxHealth; // Calculate the fill size percentage.
        fillSize = Mathf.Clamp01(fillSize); // Clamp the fill size between 0 and 1.

        // Set the local scale of the health bar fill object.
        healthBarFill.transform.localScale = new Vector3(fillSize, 1f, 1f);

        // Calculate the x position based on the fill size and ratio.
        float xOffset = (fillSize - 1f) * 0.44f; // Ratio: 0.5

        // Set the local position of the health bar fill object.
        Vector3 newPosition = healthBarFill.transform.localPosition;
        newPosition.x = xOffset;
        healthBarFill.transform.localPosition = newPosition;

        // Update the color of the health bar based on current health.
        if (currentHealth >= maxHealth * 0.5f)
        {
            healthBarFillSprite.color = healthyColor;
        }
        else if (currentHealth >= maxHealth * 0.2f)
        {
            healthBarFillSprite.color = moderateColor;
        }
        else
        {
            healthBarFillSprite.color = criticalColor;
        }
    }

    public void SetHealth(float inHealth)
    {
        currentHealth += inHealth;
        currentHealth = Mathf.Max(currentHealth, 0f); // Ensure health doesn't go below 0.
        UpdateHealthBar();
        GetComponent<Enemy>().SetState(EnemyState.TakingDamage);
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}