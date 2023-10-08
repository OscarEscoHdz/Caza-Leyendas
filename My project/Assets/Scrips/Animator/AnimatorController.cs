using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationID 
{ 
    Idle=0,
    Correr = 1,
    PrepareJump = 2,
    Jump = 3,
    Attack = 4,
    Hurt = 5,

}

public class AnimatorController : MonoBehaviour
{

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void Play(AnimationID animationID)
    {
        animator.Play(animationID.ToString());
    }

}