using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private bool gameOver = false;
    public static GameManager instance
    {

        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    var go = new GameObject("GameManager");
                    go.AddComponent<GameManager>();
                    _instance = go.GetComponent<GameManager>();

                }
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;

        }

    }


    public void HideGameOver()
    {
        gameOver = false;
    }

    public void UpdateHealt(int health)
    {
        HUDScreen.instance.UpdateHealt(health);
        if(health <= 0 &&! gameOver)
        {
            gameOver = true;
            GameOverScreen.instance.ShowScreen();
        }
    }


    public void UpdateCoins(int coins)
    {
        HUDScreen.instance.UpdateCoins(coins);
    }


    public void UpdatePowerUp(int amount)
    {
        HUDScreen.instance.UpdatePowerUp(amount);
    }
    public void UpdatePowerUp(Sprite icon, int amount)
    {
        HUDScreen.instance.UpdatePowerUp(icon, amount);
    }
}
