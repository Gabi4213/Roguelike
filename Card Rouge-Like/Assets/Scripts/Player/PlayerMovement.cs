using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public Vector2 movement;
    private Rigidbody2D rb;

    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";

    private const string lastHorizontal = "LastHorizontal";
    private const string lastVertical = "LastVertical";

    [Header("PlayerStates")]
    public bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (InputManager.inventoryOpen || !canMove)
        {
            StopPlayer();
            return;
        }

        movement.Set(InputManager.movement.x, InputManager.movement.y);

        rb.velocity = movement * PlayerStatistics.instance.moveSpeed;

        animator.SetFloat(horizontal, movement.x);
        animator.SetFloat(vertical, movement.y);

        //if we are moving set the last move direciton. otherwise when we are not keep the last one set
        if(movement != Vector2.zero)
        {
            animator.SetFloat (lastHorizontal, movement.x);
        }
    }

    private void StopPlayer()
    {
        rb.velocity = Vector2.zero;
        //animator.SetFloat(horizontal, 0);
        //animator.SetFloat(vertical, 0);
    }

    public void Hit()
    {
        StartCoroutine(PlayerHit());
    }

    IEnumerator PlayerHit()
    {
        animator.SetFloat(lastHorizontal, movement.x);
        animator.SetTrigger("Hit");
        canMove = false;
        yield return new WaitForSeconds(PlayerStatistics.instance.hitDuration);
        canMove = true;
    }

}
