using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Data")]
    public EnemyState currentState;
    public float knockbackForce;
    public float knockbackSpeed;

    [Header("Animation Data")]
    public Animator anim;

    private Rigidbody2D rb;
    private Transform target;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        currentState = EnemyState.Idle;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Frozen:
                Frozen();
                break;
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Roaming:
                Roaming();
                break;
            case EnemyState.Follow:
                Follow();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.TakingDamage:
                TakingDamage();
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

    void Idle()
    {
        if(rb.velocity.magnitude > 0.1f)
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
        }

        anim.SetBool("Moving", false);
    }
    void Roaming()
    {
        anim.SetBool("Moving", true);
    }

    void Follow()
    {
        anim.SetBool("Moving", true);
    }

    void Attack()
    {
        anim.SetBool("Moving", false);
    }

    void TakingDamage()
    {
        anim.SetTrigger("Hit");

        Vector3 directionToPlayer = target.position - transform.position;
        directionToPlayer.Normalize();
        Vector3 force = -directionToPlayer * knockbackForce;
        rb.AddForce(force);

        SetState(EnemyState.Idle);
    }

    void Dead()
    {
        Destroy(gameObject);
    }

    public void SetState(EnemyState inState)
    {
        currentState = inState;
    }
}


public enum EnemyState
{
    Frozen,
    Idle,
    Roaming,
    Follow,
    Attack,
    TakingDamage,
    Dead
}