using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDemo : MonoBehaviour, ITargetCombat
{
    [SerializeField] int health =80;

    [SerializeField] DamageFeedbackEffect damageFeedbackEffect;

    public void TakeDamage(int damagePoints)
    {
        damageFeedbackEffect.PlayDamageEffect();
        health -= damagePoints;
    }
}
