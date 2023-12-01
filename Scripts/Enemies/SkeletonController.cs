using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour, ITargetCombat
{
    public enum SkeletonState
    {
        Inactive,
        Rise,
        //Walk,
        WalkInTransformRight,
        Turn
    }

    [SerializeField] int health = 1;
    [SerializeField] SkeletonState skeletonState = SkeletonState.Inactive;
    [SerializeField] DamageFeedbackEffect damageFeedbackEffect;
    [SerializeField] AnimatorController animatorController;
    [SerializeField] float speed = 1;
    [SerializeField] GameObject destructionPrefab;
    [SerializeField] LayerChecker groundChecker;
    [SerializeField] LayerChecker blockChecker;

    private new Rigidbody2D rigidbody2D;

    private bool active;

    private bool isExecutingState = false;
    private void Awake()
    {
        skeletonState = SkeletonState.Inactive;
        animatorController.Pause();
        //animatorController.Play(AnimationId.Rise);
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (active)
        {

            if (!isExecutingState && skeletonState == SkeletonState.Rise)
            {
                Rise();
                isExecutingState = true;
            }

            if (skeletonState == SkeletonState.WalkInTransformRight)
            {
                WalkInTransformRight();
            }
        }
    }

    void WalkInTransformRight()
    {
        animatorController.Play(AnimationId.Walk);

        rigidbody2D.velocity = transform.right * speed;
        rigidbody2D.velocity = new Vector2(transform.right.x * speed, rigidbody2D.velocity.y);

        if (!groundChecker.isTouching || blockChecker.isTouching)
        {
            Turn();
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

    void Rise()
    {
        StartCoroutine(_Rise());
    }


    IEnumerator _Rise()
    {
        yield return new WaitForSeconds(0.2F);

        animatorController.Play(AnimationId.Rise);
        yield return new WaitForSeconds(0.50F);
        skeletonState = SkeletonState.WalkInTransformRight;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("MainCamera"))
        {
            active = true;
            animatorController.Unpause();
        }

        if (collision.gameObject.tag.Equals("MainCamera") && skeletonState == SkeletonState.Inactive)
        {
            //active = true;
            skeletonState = SkeletonState.Rise;
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
