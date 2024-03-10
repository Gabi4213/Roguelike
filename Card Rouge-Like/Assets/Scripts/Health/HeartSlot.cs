using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartSlot : MonoBehaviour
{
    public bool filled;
    public Sprite filledSprite, emptySprite;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (filled)
        {
            image.sprite = filledSprite;
        }
        else
        {
            image.sprite = emptySprite;
        }
    }

}
