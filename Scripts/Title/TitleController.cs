using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{

    public bool showOptions = false;

    public List<GameObject> optionButtons;
    public GameObject pressEnterButton;

    [SerializeField] AudioClip enterSfx;
    [SerializeField] AudioClip buttonSfx;

    private void LateUpdate()
    {
        if (!showOptions && Input.GetKeyDown(KeyCode.Return))
        {
            showOptions = true;
            pressEnterButton.SetActive(false);
            foreach (var button in optionButtons)
            {
                button.SetActive(true);
            }
            AudioManager.instance.PlaySfx(enterSfx);
        }
    }


    public void ExitGame()
    {
        AudioManager.instance.PlaySfx(buttonSfx);

        Application.Quit();
    }

    public void StartNewGame()
    {
        AudioManager.instance.PlaySfx(buttonSfx);
        SceneHelper.instance.LoadScene(SceneId.Level1_0);
    }
}
