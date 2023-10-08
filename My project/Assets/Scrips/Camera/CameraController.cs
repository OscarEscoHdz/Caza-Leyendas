using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BoundaryRange 
{
    public float min;
    public float max;
}

public class CameraController : MonoBehaviour
{

    [SerializeField] TagId target;

    [SerializeField] float offsetZ;

    [SerializeField] BoundaryRange boundaryX;
    [SerializeField] BoundaryRange boundaryY;

    private GameObject targetGameObject;

    Vector3 vel;



    void Start()
    {
        targetGameObject = GameObject.FindGameObjectWithTag(target.ToString());
        
    }

    
    void Update()
    {
         if (targetGameObject) 
         {
             var targetPosition = new Vector3(
                 Mathf.Clamp(targetGameObject.transform.position.x, boundaryX.min, boundaryX.max),
                 Mathf.Clamp(targetGameObject.transform.position.y, boundaryY.min, boundaryY.max),
                 -10
                 );
             this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref vel, 0.3f);

         }
        /*transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);*/
        
    }
}
