using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour, ITargetCombat
{
    public enum GhostState
    {
        Inactive,
        Patrol,
        ChasePlayer
    }

    [SerializeField] WayPointsManager wayPointsManager;
    [SerializeField] int health = 2;
    [SerializeField] GhostState ghostState = GhostState.Inactive;
    [SerializeField] DamageFeedbackEffect damageFeedbackEffect;
    
    [SerializeField] LayerChecker ghostVision;
    [SerializeField] float speed = 1;
    [SerializeField] GameObject destructionPrefab;

    private Rigidbody2D rigidbody2D;

    private bool active;

    //private bool isExecutingState = false;

    private Vector2 currentWayPoint;

    Vector2 moveDirection;
    private void Awake()
    {
        currentWayPoint = wayPointsManager.GetRandomPoint();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (active)
        {
            if (ghostState == GhostState.Patrol)
            {
                Patrol();
            }

            if (ghostState == GhostState.ChasePlayer)
            {
                ChasePlayer();
            }
        }
    }

    void ChasePlayer()
    {
        var direction = (Vector2)HeroController.instance.transform.position - (Vector2)this.transform.position;

        rigidbody2D.velocity = direction.normalized * speed;
        if (direction.x < 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (!ghostVision.isTouching)
        {
            ghostState = GhostState.Patrol;
        }
    }

    void Patrol()
    {
        var direction = currentWayPoint - (Vector2)this.transform.position;

        if (direction.x < 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        rigidbody2D.velocity = direction.normalized * speed;

        if (Vector2.Distance(currentWayPoint, this.transform.position) < 0.2f)
        {
            currentWayPoint = wayPointsManager.GetRandomPoint();
        }

        if (ghostVision.isTouching)
        {
            ghostState = GhostState.ChasePlayer;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("MainCamera"))
        {
            active = true;
        }

        if (collision.gameObject.tag.Equals("MainCamera") && ghostState == GhostState.Inactive)
        {
            ghostState = GhostState.Patrol;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("MainCamera"))
        {
            active = false;
            rigidbody2D.velocity = Vector2.zero;
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
