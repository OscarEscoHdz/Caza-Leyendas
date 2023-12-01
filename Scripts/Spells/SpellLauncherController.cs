using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLauncherController : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float force;

    public void Launch(Vector2 direction)
    {
        GameObject go = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity);
        go.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
        go.GetComponent<ProjectileController>().SetDirection(direction);
    }
}
