using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShieldPiece : MonoBehaviour
{
    public Vector3 frontView, backView, leftView, rightView;
    public int[] layerOrder;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;


    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        transform.localPosition = Vector3.zero + frontView;
    }

    private void Update()
    {
        //up
        if(playerMovement.movement.x == 0 &&  playerMovement.movement.y == 1)
        {
            transform.localPosition = Vector3.zero + backView;
            spriteRenderer.sortingOrder = layerOrder[0];
        }
        //down
        else if (playerMovement.movement.x == 0 && playerMovement.movement.y == -1)
        {
            transform.localPosition = Vector3.zero + frontView;
            spriteRenderer.sortingOrder = layerOrder[1];
        }
        //left
        if (playerMovement.movement.x == -1 && playerMovement.movement.y == 0)
        {
            transform.localPosition = Vector3.zero + leftView;
            spriteRenderer.sortingOrder = layerOrder[2];
        }
        //right
        else if (playerMovement.movement.x == 1 && playerMovement.movement.y == 0)
        {
            transform.localPosition = Vector3.zero + rightView;
            spriteRenderer.sortingOrder = layerOrder[3];
        }
    }
}
