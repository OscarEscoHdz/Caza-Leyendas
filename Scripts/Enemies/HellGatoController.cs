using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellGatoController : MonoBehaviour, ITargetCombat
{
    public enum HellGatoState
    {
        Inactive,
        ChasePlayer,
        //Walk,
        WalkInTransformRight,
        Turn
    }

    [SerializeField] int health = 1;
    [SerializeField] HellGatoState hellGatoState = HellGatoState.Inactive;
    [SerializeField] DamageFeedbackEffect damageFeedbackEffect;
    [SerializeField] AnimatorController animatorController;
    [SerializeField] float speed = 1;
    [SerializeField] GameObject destructionPrefab;
    [SerializeField] LayerChecker groundChecker;
    [SerializeField] LayerChecker blockChecker;
    [SerializeField] LayerChecker visionRange;

    private Rigidbody2D rigidbody2D;

    private bool active;

    //private bool isExecutingState = false;
    private void Awake()
    {
        hellGatoState = HellGatoState.Inactive;
        animatorController.Pause();
        //animatorController.Play(AnimationId.Rise);
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (active)
        {
            if (hellGatoState == HellGatoState.WalkInTransformRight)
            {
                WalkInTransformRight();
            }
            if (hellGatoState == HellGatoState.ChasePlayer)
            {
                ChasePlayer();
            }
        }
    }

    void ChasePlayer()
    {
        var direction = (Vector2)HeroController.instance.transform.position - (Vector2)this.transform.position;

        rigidbody2D.velocity = new Vector2(direction.normalized.x * speed, rigidbody2D.velocity.y);
        if (direction.x < 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);

        }
        if (!visionRange.isTouching)
        {
            hellGatoState = HellGatoState.WalkInTransformRight;
        }
    }

    void WalkInTransformRight()
    {
        animatorController.Play(AnimationId.Follow);

        rigidbody2D.velocity = new Vector2(transform.right.x * speed, rigidbody2D.velocity.y);

        if (!groundChecker.isTouching || blockChecker.isTouching)
        {
            Turn();
        }

        if (visionRange.isTouching)
        {
            hellGatoState = HellGatoState.ChasePlayer;
        }
    }

    void Turn()
    {
        if (this.transform.right == Vector3.right)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("MainCamera"))
        {
            active = true;
            animatorController.Unpause();
        }

        if (collision.gameObject.tag.Equals("MainCamera") && hellGatoState == HellGatoState.Inactive)
        {
            //active = true;
            hellGatoState = HellGatoState.WalkInTransformRight;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("MainCamera"))
        {
            active = false;
            animatorController.Pause();
        }
    }

    public void TakeDamage(int damagePoints)
    {
        health = Mathf.Clamp(health - damagePoints, 0, 100);
        damageFeedbackEffect.PlayDamageEffect();
        if (health <= 0)
        {
            if (destructionPrefab)
            {
                Instantiate(destructionPrefab, (Vector2)this.transform.position + Vector2.up * 0.6F, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }
}

