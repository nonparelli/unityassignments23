using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BezierPath : MonoBehaviour
{
    [SerializeField]
    Mesh2D road2D;

    [Range(0f, 1f)]
    public float t = 0.0f;
    public BezierPoint[] points; 
    // Start is called before the first frame update
    private void OnDrawGizmos()
    {

        for (int i= 0; i < points.Length-1; i++)
        {
            Handles.DrawBezier(points[i].Anchor.position,
                               points[i+1].Anchor.position,
                               points[i].control1.position,
                               points[i+1].control0.position, Color.white, null, 2f);
        }
        // Don't need this bit rn
        /*
        Handles.DrawBezier(points[points.Length-1].Anchor.position,
                   points[0].Anchor.position,
                   points[points.Length - 1].control1.position,
                   points[0].control0.position, Color.blue, null, 2f);
        */

        Vector3 tPos = GetBezierPosition(t, points[0], points[1]);
        Vector3 tDir = GetBezierDirection(t, points[0], points[1]);

        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(tPos,0.5f);
        /*
        Gizmos.color = Color.blue;
        MyGizmos.DrawVectorDir(tPos,tDir,Gizmos.color);
        */
        Quaternion rot = Quaternion.LookRotation(tDir);
        Handles.PositionHandle(tPos,rot);

        for(int i = 0; i < road2D.Vertices.Length; i++)
        {
            Vector3 roadpoint = road2D.Vertices[i].point;
            Gizmos.DrawSphere(tPos + (rot * roadpoint), 0.25f);

        }
        //Gizmos.DrawSphere(tPos + (rot * (2*Vector3.right)), 0.25f);
        //Gizmos.DrawSphere(tPos + (rot * Vector3.left), 0.25f);
    }

    Vector3 GetBezierPosition(float t, BezierPoint bp1, BezierPoint bp2)
    {
        Vector3 PtX = (1 - t) * bp1.Anchor.position + t * bp1.control1.position;
        Vector3 PtY = (1 - t) * bp1.control1.position + t * bp2.control0.position;
        Vector3 PtZ = (1-t)*bp2.control0.position + t * bp2.Anchor.position;

        Vector3 PtR = (1 - t) * PtX + t * PtY;
        Vector3 PtS = (1-t)*PtY+t* PtZ;

        return (1 - t) * PtR + t * PtS;
    }

    Vector3 GetBezierDirection(float t, BezierPoint bp1, BezierPoint bp2)
    {
        Vector3 PtX = (1 - t) * bp1.Anchor.position + t * bp1.control1.position;
        Vector3 PtY = (1 - t) * bp1.control1.position + t * bp2.control0.position;
        Vector3 PtZ = (1 - t) * bp2.control0.position + t * bp2.Anchor.position;

        Vector3 PtR = (1 - t) * PtX + t * PtY;
        Vector3 PtS = (1 - t) * PtY + t * PtZ;

        return (PtS - PtR).normalized;
    }
}
