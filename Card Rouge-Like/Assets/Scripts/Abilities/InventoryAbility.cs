using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using TMPro;

public class InventoryAbility : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image image;
    public Image cooldownImage;
    public TextMeshProUGUI cooldownText;

    [HideInInspector] public Ability ability;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    public void InitialiseAbility(Ability newAbility)
    {
        ability = newAbility;
        image.sprite = newAbility.image;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    //    image.raycastTarget = false;
    //    parentAfterDrag = transform.parent;
    //    transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
      //  transform.position = Input.mousePosition;
      //  transform.localScale = new Vector2(1f, 1f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      //  image.raycastTarget = true;
      //  transform.SetParent(parentAfterDrag);
      //  transform.localScale = new Vector2(1f, 1f);
    }
}