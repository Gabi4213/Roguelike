using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public Transform healthUIGroup;
    public GameObject healthUIPrefab;

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
