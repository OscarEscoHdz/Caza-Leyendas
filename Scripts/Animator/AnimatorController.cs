using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationId
{
    Idle = 0,
    Run = 1,
    PreparedJump = 2,
    Jump = 3,
    Attack = 4,
    Hurt = 5,
    UsePowerUp = 6,
    Rise = 7,
    Walk = 8,
    Follow = 9
}

public class AnimatorController : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Pause()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animator.speed = 0;
    }
    public void Unpause()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animator.speed = 1;
    }

    public void Play(AnimationId animationId)
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animator.Play(animationId.ToString());
    }
}

