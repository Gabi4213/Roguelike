using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    public Vector2 movement;
    public Rigidbody2D rb;

    public float LastHorizontal, LastVertical;
    public float horizontal, vertical;

    private Vector2 previousMovement;

    public float moveSpeed;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        previousMovement = Vector2.zero;
    }

    private void Update()
    {
        movement.Set(InputManager.movement.x, InputManager.movement.y);

        rb.velocity = movement * moveSpeed;

        horizontal= movement.x;
        vertical= movement.y;

        if(movement != Vector2.zero)
        {
            LastHorizontal = movement.x;
            LastVertical = movement.y;
        }

        previousMovement = movement;
    }
}
