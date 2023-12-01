using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public bool showOptions = false;

    public List<GameObject> optionButtons;
    public GameObject pressEnterButton;

    private void LateUpdate()
    {
        if(!showOptions && Input.GetKeyDown(KeyCode.Return))
        {
            showOptions = true;
            pressEnterButton.SetActive(false);
            foreach(var button in optionButtons)
            {
                button.SetActive(true);
            }
        }
    }

    public void ExitGame()
    {

    }

    public void StartNewGame()
    {
        SceneHelper.instance.LoadScene(SceneId.Level1_1);
    }



}
