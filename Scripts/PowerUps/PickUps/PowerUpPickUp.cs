using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUp : MonoBehaviour
{
    [SerializeField] PowerUpId powerUpId;
    [SerializeField] TagId playerTag;
    [SerializeField] TagId groundTag;

    [SerializeField] AudioClip pickSfx;
    [SerializeField] int maxAmount = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.tag.Equals(groundTag.ToString()))
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if (collision.gameObject.tag.Equals(playerTag.ToString()))
        {
            var amount = Random.Range(5, maxAmount);
            GameManager.instance.UpdatePowerUp(GetComponentInChildren<SpriteRenderer>().sprite, amount);

            HeroController.instance.ChangePowerUp(powerUpId, amount);
            AudioManager.instance.PlaySfx(pickSfx);
            Destroy(this.gameObject);
        }
    }
}
