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

    public float knockbackForce;

    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";

    private const string lastHorizontal = "LastHorizontal";
    private const string lastVertical = "LastVertical";

    public Transform playerFeet;

    [Header("FX")]
    public GameObject dustFX;
    public ParticleSystem hitFX;
    public float particleSpawnInterval;

    [Header("PlayerStates")]
    public bool canMove = true;

    private Vector2 previousMovement;

    private bool isRunning;
    private Coroutine dustCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        previousMovement = Vector2.zero;
        isRunning = false;
    }

    private void Update()
    {
        if (InputManager.inventoryOpen || !canMove)
        {
            StopPlayer();
            return;
        }

        movement.Set(InputManager.movement.x, InputManager.movement.y);

        // Check if the player is moving
        if (movement != Vector2.zero)
        {
            if (!isRunning)
            {
                isRunning = true;
                dustCoroutine = StartCoroutine(SpawnDustFXRoutine());
            }
        }
        else
        {
            if (isRunning)
            {
                isRunning = false;
                if (dustCoroutine != null)
                {
                    StopCoroutine(dustCoroutine);
                }
            }
        }

        //set velocity
        rb.velocity = movement * PlayerStatistics.instance.moveSpeed;

        //animations
        animator.SetFloat(horizontal, movement.x);
        animator.SetFloat(vertical, movement.y);

        //if we are moving set the last move direction. otherwise when we are not keep the last one set
        if (movement != Vector2.zero)
        {
            animator.SetFloat(lastHorizontal, movement.x);
            animator.SetFloat(lastVertical, movement.y);
        }

        // Update the previousMovement to the current movement
        previousMovement = movement;
    }

    private void StopPlayer()
    {
        rb.velocity = Vector2.zero;
        //animator.SetFloat(horizontal, 0);
        //animator.SetFloat(vertical, 0);
    }

    private IEnumerator SpawnDustFXRoutine()
    {
        while (isRunning)
        {
            InstantiateDustFX();
            yield return new WaitForSeconds(particleSpawnInterval); // Interval between dustFX instantiation
        }
    }

    private void InstantiateDustFX()
    {
        if (dustFX != null)
        {
            Instantiate(dustFX, playerFeet.position, Quaternion.identity);
        }
    }

    public void Hit()
    {
        StartCoroutine(PlayerHit());
    }

    IEnumerator PlayerHit()
    {
        CameraFollow.instance.ShakeCamera();
        hitFX.Play();
        animator.SetFloat(lastHorizontal, movement.x);
        animator.SetTrigger("Hit");
        canMove = false;
        yield return new WaitForSeconds(PlayerStatistics.instance.hitDuration);
        canMove = true;
    }

}
