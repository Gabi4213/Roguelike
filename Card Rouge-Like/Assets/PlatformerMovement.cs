using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerMovement : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public Vector2 movement;
    public Rigidbody2D rb;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float knockbackForce;

    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";

    private const string lastHorizontal = "LastHorizontal";

    public Transform playerFeet;
    public LayerMask groundLayer;

    public bool isGrounded;

    [Header("FX")]
    public GameObject dustFX;
    public ParticleSystem hitFX;
    public float particleSpawnInterval;

    [Header("PlayerStates")]
    public bool canMove = true;

    private bool isRunning;
    private Coroutine dustCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isRunning = false;
    }

    private void Update()
    {
        if (InputManager.inventoryOpen || !canMove)
        {
            StopPlayer();
            return;
        }

        movement.x = InputManager.movement.x * moveSpeed;

        isGrounded = Physics2D.OverlapCircle(playerFeet.position + new Vector3(0.0f, -0.15f, 0.0f), 0.2f, groundLayer);

        //jump
        if (InputManager.jump && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (movement.x != 0)
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

        rb.velocity = new Vector2(movement.x, rb.velocity.y);

        animator.SetFloat(horizontal, movement.x);

        animator.SetBool("isGrounded", isGrounded);

        if (movement.x != 0)
        {
            animator.SetFloat(lastHorizontal, Mathf.Sign(movement.x));
        }
    }

    public void StopPlayer()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private IEnumerator SpawnDustFXRoutine()
    {
        while (isRunning)
        {
            InstantiateDustFX();
            yield return new WaitForSeconds(particleSpawnInterval);
        }
    }

    private void InstantiateDustFX()
    {
        if (dustFX != null && isGrounded)
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
        animator.SetTrigger("Hit");
        canMove = false;
        //rb.velocity = new Vector2(-Mathf.Sign(movement.x) * knockbackForce, rb.velocity.y);
        yield return new WaitForSeconds(PlayerStatistics.instance.hitDuration);
        canMove = true;
    }

    private void OnDrawGizmos()
    {
        if (playerFeet != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerFeet.position + new Vector3(0.0f, -0.15f, 0.0f), 0.2f);
        }
    }
}
