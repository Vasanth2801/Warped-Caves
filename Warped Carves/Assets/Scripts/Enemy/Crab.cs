using System.Collections;
using UnityEngine;

public class Crab : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] bool isChasing;
    [SerializeField] private Animator animator;

    [Header("Enemy Settings")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private int facingDirection = 1;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 2;
    [SerializeField] private LayerMask playerLayer;

    [Header("Attack Settings")]
    [SerializeField] private float attackCoolDown = 1f;
    [SerializeField] private bool isAttacking = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(player == null)
        {
            return;
        }

        float distance = Vector2.Distance(player.position,transform.position);

        if(distance <= attackRange)
        {
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isChasing", false);

            if(!isAttacking)
            {
                StartCoroutine(AttackCoroutine());
            }

            return;
        }

        if(isChasing == true)
        {
            Chase();
        }
    }

    void Chase()
    {
        animator.SetBool("isChasing", true);
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        while(true)
        {
            if(player == null)
            {
                break;
            }

            float distance = Vector2.Distance(player.position, transform.position);
            if (distance > attackRange)
            {
                break;
            }

            Attack();

            yield return new WaitForSeconds(attackCoolDown);
        }

        isAttacking = false;
    }

    void Attack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach(Collider2D hit in hitPlayer)
        {
            if(player == null)
            {
                continue;
            }

            Debug.Log("Player Hit");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isChasing = false;
            animator.SetBool("isChasing", false);
            rb.linearVelocity = Vector2.zero;
        }
    }
}
