using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float detectionDistance = 5f;
    public float attackRange = 1f;

    [Header("Patrol Settings")]
    public float patrolDistance = 3f; 
    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 currentTarget;

    [Header("Attack Settings")]
    public int damageAmount = 1;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private AttributesComponent attributesComponent;
    private bool bIsDead = false;
    private ScoreManager scoreManager;

    void Start()
    {
        attributesComponent.OnDeath += OnEnemyDeath;

        // Find patrol points
        pointA = transform.position + Vector3.left * patrolDistance;
        pointB = transform.position + Vector3.right * patrolDistance;
        currentTarget = pointA;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
        attributesComponent = GetComponent<AttributesComponent>();
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }
    private void Update()
    {
        if (bIsDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionDistance)
        {
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (anim) anim.SetBool("IsRunning", true);

        Vector2 direction = (currentTarget - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        FlipSpriteX(direction);

        // Did it get to the point
        if (Vector2.Distance(transform.position, currentTarget) < 0.2f)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
        }
    }

    private void ChasePlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > attackRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

            if (anim) anim.SetBool("IsRunning", true);

            FlipSpriteX(direction);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            if (anim) anim.SetBool("IsRunning", false);

            if (Time.time > lastAttackTime + attackCooldown)
            {
                AttackPlayer();
            }
        }
    }

    private void FlipSpriteX(Vector2 direction)
    {
        if (direction.x > 0) sprite.flipX = true;
        else if (direction.x < 0) sprite.flipX = false;
    }

    private void AttackPlayer()
    {
        lastAttackTime = Time.time;

        if (anim) anim.SetTrigger("Attack");

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            var playerAttributes = player.GetComponent<AttributesComponent>();
            if (playerAttributes != null)
            {
                playerAttributes.ApplyDamage(damageAmount);
            }
        }
    }

    private void OnEnemyDeath()
    {
        bIsDead = true;
        rb.linearVelocity = Vector2.zero;
        if (anim) anim.SetTrigger("Death");
        scoreManager.IncreaseKillCounter(); 
    }

    void OnDestroy()
    {
        attributesComponent.OnDeath -= OnEnemyDeath;
    }
}
