using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArmourPiece : MonoBehaviour
{
    public Vector3 offsetPosition;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;
    public Sprite frontView, backView, leftView, rightView;


    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        transform.localPosition = Vector3.zero + offsetPosition;
    }

    private void Update()
    {
        //up
        if(playerMovement.movement.x == 0 &&  playerMovement.movement.y == 1)
        {
            spriteRenderer.sprite = backView;
        }
        //down
        else if (playerMovement.movement.x == 0 && playerMovement.movement.y == -1)
        {
            spriteRenderer.sprite = frontView;
        }
        //left
        if (playerMovement.movement.x == -1 && playerMovement.movement.y == 0)
        {
            spriteRenderer.sprite = leftView;
        }
        //right
        else if (playerMovement.movement.x == 1 && playerMovement.movement.y == 0)
        {
            spriteRenderer.sprite = rightView;
        }
    }
}
