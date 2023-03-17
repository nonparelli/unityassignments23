using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BezierPoint : MonoBehaviour
{

    public Transform control0; // First control point
    public Transform control1; // Second control point

    public Transform Anchor { get { return gameObject.transform; } }
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Transform anchor = gameObject.transform;

        Gizmos.color = Color.black;
        Gizmos.DrawLine(control0.position, anchor.position);
        Gizmos.DrawLine(control1.position, anchor.position);
    }
}
