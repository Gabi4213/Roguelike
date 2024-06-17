using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Data")]
    public EnemyState currentState;
    public float knockbackForce;
    public float knockbackSpeed;
    public float moveSpeed;
    public float stoppingDistance;

    public bool enemyHit;

    [Header("Animation Data")]
    public Animator anim;

    private Rigidbody2D rb;
    private Transform target;


    public int damageAmount;
    public float damageDelay; 

    private bool canDealDamage = true;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        enemyHit = false;
        currentState = EnemyState.Follow;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Frozen:
                Frozen();
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
                Dead();
                break;
        }
    }

    void Frozen()
    {
        rb.velocity = Vector3.zero;
    }

    void Recover()
    {
        //gradually stop the enemy moving
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
        if (target != null)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetState(EnemyState.Attack);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetState(EnemyState.Follow);
        }
    }

    void Attack()
    {
        DealDamageToPlayer();
    }

    void Dead()
    {
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
        Debug.Log("Hit");
        anim.SetBool("Moving", false);

        Vector3 directionToPlayer = (target.position - transform.position).normalized;
        Vector3 knockback = directionToPlayer * knockbackForce; // Multiply by knockback force directly
        rb.velocity = Vector3.zero; // Ensure no residual velocity
        rb.AddForce(knockback, ForceMode2D.Impulse); // Apply knockback force

        yield return new WaitForSeconds(0.3f);
        enemyHit = false;
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

}


public enum EnemyState
{
    Frozen,
    Follow,
    Recover,
    Attack,
    TakingDamage,
    Dead
}