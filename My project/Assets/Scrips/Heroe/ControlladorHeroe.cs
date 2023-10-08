using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlladorHeroe : MonoBehaviour, ITargetCombat
{
    [Header("Health vaiables")]
    [SerializeField] int health = 10;
    [SerializeField] DamageFeedbackEffect damageFeedbackEffect;


    [Header("Attacke vaiables")]
    [SerializeField] SwordController swordController;


    [Header("Animation Variables")]
    [SerializeField] AnimatorController animatorController;

    [Header("Checker Variables")]
    [SerializeField] LayerChecker footA;
    [SerializeField] LayerChecker footB;

    [Header("Interruption Variables")]   /*Esta linea estaba comentada*/
    private bool canCheckGround;   /*Esta linea estaba comentada*/
    private bool canMove;
    private bool canFlip;


    [Header("Rigid Variables")]
    [SerializeField] private float damageForce;
    [SerializeField] private float damageForceUp;
    [SerializeField] private float jumpForce=5;
    [SerializeField] private float doubleJumpForce;
    

    [SerializeField] private float speed; 
    [SerializeField] private Vector2 movementDirection;

    [Header("Boolean Variables")]
    public bool canDoubleJump;
    public bool isLookingRight;
    public bool playerIsRecovering;
    private bool playerIsOnGround;

    [Header("Audio")]
    [SerializeField] AudioClip attackSfx;
    

    private Rigidbody2D rigidbody2D;
    private bool jumpPressed = false;
    private bool attackPressed = false;
    public bool playerIsAttacking;








    
    void Start()
    {
        canMove = true;
        canCheckGround = true;  /*Esta linea estaba comentada*/
        rigidbody2D = GetComponent<Rigidbody2D>();
        animatorController.Play(AnimationID.Correr);

    }

    
    void Update()
    {
        HandleControls();
        HandleMovement();
        HandleFlip();
        HandleJump();
        HandleAttack();
        HandleIsGrounding();
    }

    void HandleIsGrounding() 
    {
        if (!canCheckGround) return;  /*Esta linea estaba comentada*/
        playerIsOnGround = footA.isTouching || footB.isTouching;
    }


    void HandleControls() 
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        jumpPressed = Input.GetKeyDown(KeyCode.Space);
        attackPressed = Input.GetButtonDown("Attack");
    }

    void HandleMovement() 
    {
        if (!canMove) return;

        rigidbody2D.velocity = new Vector2(movementDirection.x * speed, rigidbody2D.velocity.y);

        if (playerIsOnGround)
        {
            

            if (Mathf.Abs(rigidbody2D.velocity.x) > 0)
            {
                animatorController.Play(AnimationID.Correr);
            }
            else
            {
                animatorController.Play(AnimationID.Idle);
            }
        }

    }

    void HandleFlip() 
    {
        if (!canFlip) return;


        if (rigidbody2D.velocity.magnitude > 0)
        {
            
            if (rigidbody2D.velocity.x >= 0)
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                isLookingRight = true;
            }
            else
            {
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
                isLookingRight = false;
            }
        }
    }


    void HandleJump() 
    {
        if (canDoubleJump && jumpPressed && !playerIsOnGround) 
        {
            /*this.rigidbody2D.velocity = Vector2.zero;*/  /*Esta linea estaba comentada*/
            this.rigidbody2D.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
            canDoubleJump = false;
        }

        if (jumpPressed && playerIsOnGround) {
            this.rigidbody2D.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
            StartCoroutine(HandleJumpAnimation());
            canDoubleJump = true;
            
        }
    }


    IEnumerator HandleJumpAnimation() 
    {
        canCheckGround = false;   /*Esta linea estaba comentada*/
        playerIsOnGround = false; /*Esta linea estaba comentada*/
        if (!playerIsAttacking) 
        {
            animatorController.Play(AnimationID.PrepareJump);
        }
        yield return new WaitForSeconds(0.2f);
        if (!playerIsAttacking) 
        {
            animatorController.Play(AnimationID.Jump);
        }
        canCheckGround = true;    /*Esta linea estaba comentada*/
    }




    void HandleAttack() 
    {
        if (attackPressed &&! playerIsAttacking) 
        {
            AudioManager.instance.PlayASfx(attackSfx);
            animatorController.Play(AnimationID.Attack);
            playerIsAttacking = true;
            swordController.Attack(0.35f);
            StartCoroutine(RestoreAttack());
        }
    }

    IEnumerator RestoreAttack()
    {
        canMove = false;
        yield return new WaitForSeconds(0.35f);
        playerIsAttacking = false;
        if (!playerIsAttacking)
        {
            animatorController.Play(AnimationID.Jump);
        }
        canMove = true;


    }

    public void TakeDamage(int damagePoints)
    {
        if (!playerIsRecovering)
        {
            health = Mathf.Clamp(health - damagePoints, 0, 10);
            StartCoroutine(StartPlayerRecover());

            if (isLookingRight)
            {
                rigidbody2D.AddForce(Vector2.left * damageForce + Vector2.up * damageForceUp, ForceMode2D.Impulse);
            }
            else 
            {
                rigidbody2D.AddForce(Vector2.right * damageForce + Vector2.up * damageForceUp, ForceMode2D.Impulse);
            }
        }

    }


    IEnumerator StartPlayerRecover()
    {
        canMove = false;
        canFlip = false;
        animatorController.Play(AnimationID.Hurt);
        yield return new WaitForSeconds(0.2f);
        canMove = true;
        canFlip = true;
        rigidbody2D.velocity = Vector2.zero;
        playerIsRecovering = true;
        damageFeedbackEffect.PlayBlinkDamageEffect();
        yield return new WaitForSeconds(2);
        damageFeedbackEffect.StopBlinkDamageEffect();
        playerIsRecovering = false;
    }
}
