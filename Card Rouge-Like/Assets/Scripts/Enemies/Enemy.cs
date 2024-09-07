using NUnit.Framework.Constraints;
using System.Collections;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Data")]
    public EnemyState currentState;
    public float knockbackForce;
    public float knockbackSpeed;
    public float moveSpeed;
    public float stoppingDistance;
    public float atertTime;
    public float spottedDistance;

    public bool enemyHit;

    [Header("Animation Data")]
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    public Sprite defaultSprite, hitSprite;

    private Rigidbody2D rb;
    private Transform target;

    public ParticleSystem hitFX;

    public int damageAmount;
    public float damageDelay;

    private bool canDealDamage = true;

    public GameObject killedFX;

    public Animator AboveHeadTextAnim;
    public TextMeshPro aboveNameText;

    private bool enemyActivated;
    private bool enemyAlerted;

    private DropLoot dropLoot;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dropLoot = GetComponent<DropLoot>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        aboveNameText.text = "";
        enemyActivated = false;
        enemyHit = false;
        currentState = EnemyState.Frozen;
    }

    private void Update()
    {       
        switch (currentState)
        {
            case EnemyState.Frozen:
                ActivateEnemy();
                Frozen();
                break;
            case EnemyState.Alerted:
                Alerted();
            break;
            case EnemyState.Follow:
                Follow();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Recover:
                Recover();
                break;
            case EnemyState.TakingDamage:
                if (!enemyHit)
                {
                    enemyHit = true;                   
                    StartCoroutine(EnemyHit());
                }
                break;
            case EnemyState.Dead:
                StartCoroutine(Dead());
                break;
        }

        if(currentState == EnemyState.Frozen)
        {
            anim.SetBool("Frozen", true);
        }
        else
        {
            anim.SetBool("Frozen", false);
        }
    }

    void ActivateEnemy()
    {
        if (enemyActivated) return;
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        if (distanceToPlayer <= spottedDistance)
        {
            enemyActivated = true;
            SetState(EnemyState.Alerted);
        }
    }

    void Frozen()
    {
        rb.velocity = Vector3.zero;
    }

    void Recover()
    {
        // gradually stop the enemy moving
        if (rb.velocity.magnitude > 0.1f)
        {
            Debug.Log("reducing velocity");
            // Calculate the new velocity using Lerp
            Vector3 newVelocity = Vector3.Lerp(rb.velocity, Vector3.zero, knockbackSpeed * Time.deltaTime);

            // Assign the new velocity to the Rigidbody
            rb.velocity = newVelocity;
        }
        else
        {
            rb.velocity = Vector3.zero;
            SetState(EnemyState.Follow);
        }
    }

    void Follow()
    {
        if (target != null && currentState != EnemyState.Frozen)
        {
            // Calculate the direction towards the target
            Vector3 direction = (target.position - transform.position).normalized;

            // Calculate the distance to the target
            float distance = Vector3.Distance(transform.position, target.position);

            // Check if we're within stopping distance
            if (distance > stoppingDistance)
            {
                // Set the velocity of the Rigidbody to move towards the target
                SetState(EnemyState.Follow);
                rb.velocity = direction * moveSpeed;
                anim.SetBool("Moving", true);
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    void Alerted()
    {
        if (enemyAlerted) return;
        StartCoroutine(EnemyAlerted());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && currentState != EnemyState.Frozen)
        {
            SetState(EnemyState.Attack);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && currentState != EnemyState.Frozen)
        {
            SetState(EnemyState.Attack);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && currentState != EnemyState.Frozen)
        {
            SetState(EnemyState.Follow);
        }
    }

    void Attack()
    {
        DealDamageToPlayer();
    }

    IEnumerator Dead()
    {
        // Apply additional knockback force before destruction
        Vector3 directionToPlayer = (target.position - transform.position).normalized;
        Vector3 knockback = directionToPlayer * (knockbackForce * 1.2f); // Increase knockback force
        rb.velocity = Vector3.zero; // Ensure no residual velocity
        rb.AddForce(knockback, ForceMode2D.Impulse); // Apply knockback force

        yield return new WaitForSeconds(0.5f); // Delay before destruction

        Instantiate(killedFX,transform.position, Quaternion.identity);
        dropLoot.DropSoulFragments();
        dropLoot.DropItems();
        Destroy(gameObject);
    }

    public void SetState(EnemyState inState)
    {
        currentState = inState;
    }

    public void SetKnockbackForce(float inKockbackForce)
    {
        knockbackForce = inKockbackForce;
    }

    IEnumerator EnemyHit()
    {
        anim.SetTrigger("Hit");
        anim.SetBool("Moving", false);

        //Sprite Change
        spriteRenderer.sprite = hitSprite;

        hitFX.Play();

        // Knockback
        Vector3 directionToPlayer = (target.position - transform.position).normalized;
        Vector3 knockback = directionToPlayer * knockbackForce; // Multiply by knockback force directly
        rb.velocity = Vector3.zero; // Ensure no residual velocity
        rb.AddForce(knockback, ForceMode2D.Impulse); // Apply knockback force

        yield return new WaitForSeconds(0.3f);
        enemyHit = false;
        spriteRenderer.sprite = defaultSprite;
        SetState(EnemyState.Recover);
    }

    void DealDamageToPlayer()
    {
        if (canDealDamage)
        {
            Debug.Log(gameObject.name + " :Dealt Damage");
            StartCoroutine(DealDamage());
        }
    }

    IEnumerator DealDamage()
    {
        canDealDamage = false;
        PlayerStatistics.instance.SetHealth(-1.0f);
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(damageDelay);
        canDealDamage = true;
    }

    IEnumerator EnemyAlerted()
    {
        enemyAlerted = true;
        aboveNameText.text = "!";
        AboveHeadTextAnim.SetTrigger("Play");
        rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(atertTime);

        aboveNameText.text = "";
        SetState(EnemyState.Follow);
    }
}

public enum EnemyState
{
    Frozen,
    Alerted,
    Follow,
    Recover,
    Attack,
    TakingDamage,
    Dead
}
