using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour, ITargetCombat
{
    [Header("Power Up")]


    [SerializeField] private PowerUpId currentPowerUp;
    [SerializeField] private int _powerUpAmount;
    [SerializeField] private int powerUpAmount 
    {
        get 
        {
            return _powerUpAmount;
        }
        set 
        {
            if (value != _powerUpAmount) 
            {
                GameManager.instance.UpdatePowerUp(value);
            }
            _powerUpAmount = value;
        }
    }

    [SerializeField]
    SpellLauncherController bluePotionLauncher;
    [SerializeField]
    SpellLauncherController redPotionLauncher;

    [Header("Variables de salud")]
    private int _health = 10;
    [SerializeField] int health 
    {
        get
        {
            return _health;
        }
        set 
        {
            if (_health != value)
            {
                GameManager.instance.UpdateHealt(value);
            }
            _health = value;
        }
    }

    [SerializeField]
    DamageFeedbackEffect damageFeedbackEffect;

    [Header("Variables de ataque")]
    [SerializeField]
    SwordController swordController;

    [Header("Variables de animacion")]
    [SerializeField] AnimatorController animatorController;

    [Header("Checar variables")]
    [SerializeField]
    LayerChecker footA;
    [SerializeField]
    LayerChecker footB;

    [Header("Variables booleanas")]
    public bool playerIsAttacking;
    public bool playerIsUsingPowerUp;
    public bool playerIsRecovering;
    public bool canDoubleJump;
    public bool isLookingRight;

    [Header("Variables de interrupcion")]
    public bool canCheckGround;
    public bool canMove;

    public bool canFlip;

    [Header("Variables de rigidez")]
    [SerializeField] private float damageForce;
    [SerializeField] private float damageForceUp;

    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;

    [SerializeField] private float speed;

    [Header("Audio")]
    [SerializeField] AudioClip attackSfx;


    //Variables de control
    [SerializeField] private Vector2 movementDirection;
    private bool jumpPressed = false;
    private bool attackPressed = false;
    private bool usePowerUpPressed = false;
    private bool died;

    private int _coins=-1;
    private int coins 
    {
        get 
        {
            return _coins;
        }
        set 
        {
            if (_coins != value) 
            {
                GameManager.instance.UpdateCoins(value);
            }
            _coins = value;
        }
    }

    private bool playerIsOnGround;

    private new Rigidbody2D rigidbody2D;

   
    public static HeroController _instance;

    public static HeroController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<HeroController>();
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        if (instance && instance != this)
        {
            Destroy(this.gameObject);
        }

        health = 10;
        coins = 0;
    }

    void Start()
    {
        canCheckGround = true;
        canMove = true;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animatorController.Play(AnimationId.Idle);
    }


    void Update()
    {
        if (!died)
        {
            HandleControls();
            HandleMovement();
            HandleFlip();
            HandleJump();
            HandleAttack();
            HandleIsGrounding();
            HandleUsePowerUp();
        }
    }

    public void GiveCoin()
    {
        coins = Mathf.Clamp(coins + 1, 0, 10000000);
    }

    public void GiveHealthPoint()
    {
        health = Mathf.Clamp(health + 5, 0, 100);
    }

    public void ChangePowerUp(PowerUpId powerUpId, int amount)
    {
        currentPowerUp = powerUpId;
        powerUpAmount = amount;
        Debug.Log(currentPowerUp);
    }

    void HandleIsGrounding()
    {
        if (!canCheckGround) return;
        playerIsOnGround = footA.isTouching || footB.isTouching;
    }

    void HandleControls()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        jumpPressed = Input.GetButtonDown("Jump");
        attackPressed = Input.GetButtonDown("Attack");
        usePowerUpPressed = Input.GetButtonDown("UsePowerUp");  
    }

    void HandleMovement()
    {
        if (!canMove) return;

        rigidbody2D.velocity = new Vector2(movementDirection.x * speed, rigidbody2D.velocity.y);

        if (playerIsOnGround)
        {
            if (Mathf.Abs(rigidbody2D.velocity.x) > 0)
            {
                animatorController.Play(AnimationId.Run);
            }
            else
            {
                animatorController.Play(AnimationId.Idle);
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
            this.rigidbody2D.velocity = Vector2.zero;
            this.rigidbody2D.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
            canDoubleJump = false;
        }

        if (jumpPressed && playerIsOnGround)
        {
            this.rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            StartCoroutine(HandleJumpAnimation());
            canDoubleJump = true;
        }
    }

    void HandleAttack()
    {
        if (attackPressed && !playerIsAttacking)
        {
            if (playerIsOnGround)
            {
                rigidbody2D.velocity = Vector2.zero;
            }

            AudioManager.instance.PlaySfx(attackSfx);
            animatorController.Play(AnimationId.Attack);
            playerIsAttacking = true;
            swordController.Attack(0.1f, 0.3f);
            StartCoroutine(RestoreAttack());
        }
    }

    IEnumerator RestoreAttack()
    {
        if (playerIsOnGround)
            canMove = false;
        yield return new WaitForSeconds(0.4f);
        playerIsAttacking = false;
        if (!playerIsOnGround)
            animatorController.Play(AnimationId.Jump);
        canMove = true;
    }

    public void UpdatePosition(Vector2 position)
    {
        this.transform.position = position;
        rigidbody2D.velocity = Vector2.zero;
    }


    void HandleUsePowerUp()
    {
        if (attackPressed && !playerIsUsingPowerUp && currentPowerUp != PowerUpId.Nothing)
        {
            if (playerIsOnGround)
            {
                rigidbody2D.velocity = Vector2.zero;
            }
            AudioManager.instance.PlaySfx(attackSfx);
            animatorController.Play(AnimationId.UsePowerUp);
            playerIsUsingPowerUp = true;

            //swordController.Attack(0.1f, 0.3f);

            if (currentPowerUp == PowerUpId.BluePotion)
            {
                bluePotionLauncher.Launch((Vector2)transform.right + Vector2.up * 0.3f);
            }
            if (currentPowerUp == PowerUpId.RedPotion)
            {
                redPotionLauncher.Launch(transform.right);
            }

            StartCoroutine(RestoreUsePowerUp());
            powerUpAmount--;

            if (powerUpAmount <= 0)
            {
                currentPowerUp = PowerUpId.Nothing;
            }
        }
    }

    IEnumerator RestoreUsePowerUp()
    {
        if (playerIsOnGround)
            canMove = false;
        yield return new WaitForSeconds(0.4f);
        playerIsUsingPowerUp = false;
        if (!playerIsOnGround)
            animatorController.Play(AnimationId.Jump);
        canMove = true;
    }

    IEnumerator HandleJumpAnimation()
    {
        canCheckGround = false;
        playerIsOnGround = false;
        if (!playerIsAttacking)
            animatorController.Play(AnimationId.PreparedJump);
        yield return new WaitForSeconds(0.3f);
        if (!playerIsAttacking)
            animatorController.Play(AnimationId.Jump);
        canCheckGround = true;
    }

    public void TakeDamage(int damagePoints)
    {
        if (!playerIsRecovering &&! died)
        {
            health = Mathf.Clamp(health - damagePoints, 0, 100);

            if(health <= 0)
            {
                died = true;
            }

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
        animatorController.Play(AnimationId.Hurt);
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
