using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierControlPoints : MonoBehaviour
{

    public Color color = Color.red;
    public bool beingMoved;
    [HideInInspector]
    public Vector3 cachedPosition;

    void Start()
    {
        cachedPosition = transform.position;
    }
    
    void OnDrawGizmos()
    {

        cachedPosition = transform.position;

        // Draw control point
        Gizmos.color = color;
        Gizmos.DrawSphere(cachedPosition, 0.04f);

    }

}