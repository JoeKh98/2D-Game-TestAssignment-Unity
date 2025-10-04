using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;



    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public int jumpsAllowed = 2;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public float variableJumpMultiplier = 0.5f;
    private bool bIsGrounded;
    private bool bIsJumpRequested = false;
    private int jumpCounter = 0;


    [Header("Attack Settings")]
    public float attackRadius = 5.0f;
    public Transform attackPoint;
    public LayerMask enemyLayerMask;
    private bool bCanAttack = true;


    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    private AttributesComponent attributesComponent;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        attributesComponent = GetComponent<AttributesComponent>(); 
    }

    void Start()
    {
        if (attributesComponent != null)
        {
            attributesComponent.OnDeath += OnPlayerDeath;   
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            bIsJumpRequested = value.isPressed && CanJump();
        }
        Debug.Log("Jump button pressed");

    }

    public void OnAttack(InputValue value)
    {
        StartAttack();
    }

    void Update()
    {
        // Ground check
        bIsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        //Debug.Log(bIsGrounded);


        // Flip srpite 
        if (sprite != null)
        {
            FlipSpriteX();
        }

        // Set anim parameters
        if (anim != null)
        {
            anim.SetBool("Run", moveInput.x != 0);
            anim.SetBool("Grounded", bIsGrounded);
        }

    }

    private void FlipSpriteX()
    {
        if (moveInput.x > 0)
        {
            sprite.flipX = true;
            attackPoint.localPosition = new Vector2(Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y);
        }
        else if (moveInput.x < 0)
        {
            sprite.flipX = false;
            attackPoint.localPosition = new Vector2(-Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        if (bIsJumpRequested)
        {
            TryJump();
        }

    }

    private void TryJump()
    {
        if (bIsGrounded)
        {
            jumpCounter = 0;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        anim.SetTrigger("jump");
        bIsGrounded = false;
        bIsJumpRequested = false;
        jumpCounter++;
    }

    private bool CanJump()
    {
        return bIsGrounded || (jumpsAllowed > jumpCounter);
    }

    public void StartAttack()
    {
        if (!bCanAttack)
        {
            return;
        }

        anim.SetBool("isAttacking", true);
    }

    public void Attack()
    {

        Debug.Log("Attack");
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayerMask);

        foreach (Collider2D enemyGameObject in enemies)
        {
            Debug.Log("Hit!");

            if (enemyGameObject != null)
            {
                enemyGameObject.GetComponent<AttributesComponent>().ApplyDamage(1);
            }

        }
    }

    public void EndAttack()
    {
        anim.SetBool("isAttacking", false);
    }

    private void OnPlayerDeath()
    {
        var gameManager = FindFirstObjectByType<GameManager>();
        if(gameManager != null)
        {
            gameManager.PlayerGameOver();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);

    }

    
}
