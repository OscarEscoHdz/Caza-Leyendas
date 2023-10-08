using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class DamageFeedbackEffect : MonoBehaviour
{
    [SerializeField] float blinkSpeed = 10;
    private Renderer renderer;

    private bool playBlinkSpeed;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    public void PlayDamageEffect() 
    {
        StartCoroutine(_PlayDamageEffect());
    }

    public IEnumerator _PlayDamageEffect() 
    {
        renderer.material.SetFloat("_FlashAmount", 1);
        yield return new WaitForSeconds(0.3f);

        renderer.material.SetFloat("_FlashAmount", 0);
    }


    public void PlayBlinkDamageEffect()
    {
        playBlinkSpeed = true;
        StartCoroutine(_PlayBlinkDamageEffect());
    }

    public void StopBlinkDamageEffect()
    {
        playBlinkSpeed = false;
        renderer.material.SetFloat("_FlashAmount", 0);
    }


    private IEnumerator _PlayBlinkDamageEffect()
    {
        float cosValue = 0;

        while (playBlinkSpeed) 
        {
            cosValue = Mathf.Cos(Time.time * blinkSpeed);
            renderer.material.SetFloat("_FlashAmount", cosValue < 0 ? 0 : cosValue);
            yield return !playBlinkSpeed;
        }
    }

}
