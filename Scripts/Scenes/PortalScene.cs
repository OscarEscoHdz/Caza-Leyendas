using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PortalScene : MonoBehaviour
{
    [SerializeField] SceneId levelToLoad;
    [SerializeField] Transform spawnPosition;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(TagId.Player.ToString()))
        {
            SceneHelper.instance.LoadScene(levelToLoad);
        }
    }
    public SceneId SceneToLoad()
    {
        return levelToLoad;
    }

    public Vector2 GetSpawnPosition()
    {
        return spawnPosition.position;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < 10; i++) {
            Gizmos.DrawWireCube(this.transform.position, new Vector3(i * 0.3f, i * 0.3f, 0));
        }
        Handles.Label((Vector2)this.transform.position + Vector2.up* 2, "LEVEL: " + levelToLoad.ToString());
        Gizmos.DrawWireSphere(spawnPosition.position, 0.3f);
    }
#endif
}
