using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShieldPiece : MonoBehaviour
{
    public Vector3 frontView, backView, leftView, rightView;
    public float frontRotation, backRotation, leftRotation, rightRotation;
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
        if(playerMovement.rb.velocity.magnitude <= 0.1f)
        {   
            //left
            if (playerMovement.lastMovement.x == -1 && playerMovement.lastMovement.y == 0)
            {
                transform.localPosition = Vector3.zero + leftView;
                transform.localRotation = Quaternion.Euler(0.0f, 0.0f, leftRotation);
                spriteRenderer.sortingOrder = layerOrder[2];
            }
            //right
            else if (playerMovement.lastMovement.x == 1 && playerMovement.lastMovement.y == 0)
            {
                transform.localPosition = Vector3.zero + rightView;
                transform.localRotation = Quaternion.Euler(0.0f, 0.0f, rightRotation);
                spriteRenderer.sortingOrder = layerOrder[3];
            }
            //right
            if (playerMovement.lastMovement.x == 0 && playerMovement.lastMovement.y == 1)
            {
                transform.localPosition = Vector3.zero + rightView;
                transform.localRotation = Quaternion.Euler(0.0f, 0.0f, rightRotation);
                spriteRenderer.sortingOrder = layerOrder[3];
            }
            //right
            else if (playerMovement.lastMovement.x == 0 && playerMovement.lastMovement.y == -1)
            {
                transform.localPosition = Vector3.zero + rightView;
                transform.localRotation = Quaternion.Euler(0.0f, 0.0f, rightRotation);
                spriteRenderer.sortingOrder = layerOrder[3];
            }
            return;
        }

        //up
        if (playerMovement.movement.x == 0 &&  playerMovement.movement.y == 1)
        {
            transform.localPosition = Vector3.zero + backView;
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, backRotation);
            spriteRenderer.sortingOrder = layerOrder[0];
        }
        //down
        else if (playerMovement.movement.x == 0 && playerMovement.movement.y == -1)
        {
            transform.localPosition = Vector3.zero + frontView;
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, frontRotation);
            spriteRenderer.sortingOrder = layerOrder[1];
        }
        //left
        if (playerMovement.movement.x == -1 && playerMovement.movement.y == 0)
        {
            transform.localPosition = Vector3.zero + leftView;
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, leftRotation);
            spriteRenderer.sortingOrder = layerOrder[2];
        }
        //right
        else if (playerMovement.movement.x == 1 && playerMovement.movement.y == 0)
        {
            transform.localPosition = Vector3.zero + rightView;
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, rightRotation);
            spriteRenderer.sortingOrder = layerOrder[3];
        }
    }
}
