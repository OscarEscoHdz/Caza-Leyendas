using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class SceneHelper : MonoBehaviour
{
    private static SceneHelper _instance;


    public SceneId previousScene;
    public static SceneHelper instance
    {
        get
        {

            if (_instance == null)
            {
                _instance = FindObjectOfType<SceneHelper>();

                if (_instance == null)
                {
                    var go = new GameObject("SceneHelper");
                    go.AddComponent<SceneHelper>();

                    _instance = go.GetComponent<SceneHelper>();
                }
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    public void ReloadScene()
    {
        Enum.TryParse(SceneManager.GetActiveScene().name, out SceneId sceneId);
        StartCoroutine(_LoadScene(sceneId));
    }

    public void LoadScene(SceneId sceneId)
    {

        StartCoroutine(_LoadScene(sceneId));
    }

    private IEnumerator _LoadScene(SceneId sceneId)
    {

        Enum.TryParse(SceneManager.GetActiveScene().name, out previousScene);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId.ToString());
        Camera.main.GetComponent<CameraController>().FreezeCamera();
        HeroController.instance.UpdatePosition(new Vector2(-15, 0));

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        var list = FindObjectsOfType<PortalScene>().ToList();
        if(list != null)
        {
            var spawnPosition = list.Find(x => x.SceneToLoad() == previousScene).GetSpawnPosition();
            Debug.Log("spawnPosition" + spawnPosition);
            HeroController.instance.UpdatePosition(spawnPosition);
            
        }


        //Camera.main.GetComponent<CameraController>().UpdatePosition(spawnPosition);

    }
}
