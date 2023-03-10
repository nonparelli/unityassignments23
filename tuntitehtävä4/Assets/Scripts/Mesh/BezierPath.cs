using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BezierPath : MonoBehaviour
{
    public BezierPoint[] points; 
    // Start is called before the first frame update
    private void OnDrawGizmos()
    {

        for (int i= 0; i < points.Length-1; i++)
        {
            Handles.DrawBezier(points[i].Anchor.position,
                               points[i+1].Anchor.position,
                               points[i].control1.position,
                               points[i+1].control0.position, Color.blue, null, 2f);
        }
        Handles.DrawBezier(points[points.Length-1].Anchor.position,
                   points[0].Anchor.position,
                   points[points.Length - 1].control1.position,
                   points[0].control0.position, Color.blue, null, 2f);
    }
}
