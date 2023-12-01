using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    private static GameOverScreen _instance;
    [SerializeField] CanvasFade canvasFade;

    public static GameOverScreen instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameOverScreen>();
                if (_instance == null)
                {
                    var go = Resources.Load("UI/GameOverScreen") as GameObject;
                    go = Instantiate(go, Vector3.zero, Quaternion.identity);
                    _instance = go.GetComponent<GameOverScreen>();
                }
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    public void ShowScreen()
    {
        //CanvasFade.FadeIn();
    }

    public void Continue()
    {
        SceneHelper.instance.ReloadScene( );
        GameManager.instance.HideGameOver();
        Destroy(this.gameObject);
    }

    public void NoContinue()
    {
        AudioManager.instance.PlayMusic(null);
        GameManager.instance.HideGameOver();
        SceneHelper.instance.LoadScene(SceneId.TitleScreen);
        Destroy(this.gameObject);
    }
}
