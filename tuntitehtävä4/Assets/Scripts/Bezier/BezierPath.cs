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
    [Range(1,30)]
    public int segments = 1;
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
        //Step 1: construct a road segment
        Vector3 tPos = GetBezierPosition(t, points[0], points[1]);
        Vector3 tDir = GetBezierDirection(t, points[0], points[1]);

        Gizmos.color = Color.red;

        Quaternion rot = Quaternion.LookRotation(tDir);
        Handles.PositionHandle(tPos, rot);

        Gizmos.DrawSphere(tPos, 0.25f);

        for (float i = 0; i < 1f; i+=1f / segments)
            DrawRoadSegment(i);
        DrawRoadSegment(1f);
        //Step 2: draw lines between them? uhhhh
        for (float i = 1f / segments; i < 1f; i += 1f / segments)
            DrawRoadLines(i);
        DrawRoadLines(1f);
    }
    void DrawRoadSegment(float seg)
    {
        Vector3 tPos = GetBezierPosition(seg, points[0], points[1]);
        Vector3 tDir = GetBezierDirection(seg, points[0], points[1]);

        Gizmos.color = Color.red;

        Quaternion rot = Quaternion.LookRotation(tDir);
        //Handles.PositionHandle(tPos, rot);

        Vector3 previous =tPos + (rot * road2D.Vertices[0].point);
        for (int i = 0; i < road2D.Vertices.Length; i++)
        {
            Vector3 roadpoint = road2D.Vertices[i].point;
            Gizmos.DrawSphere(tPos + (rot * roadpoint), 0.25f);
        }
        //Draw lines
        Gizmos.color = Color.white;
        for (int i = 0; i < road2D.Vertices.Length; i++)
        {
            Vector3 roadpoint = road2D.Vertices[i].point;
            Vector3 current = tPos + (rot * roadpoint);
            Gizmos.DrawLine(previous, current);
            previous = current;
        }
    }
    void DrawRoadLines(float seg)
    {
        Vector3 tPos = GetBezierPosition(seg, points[0], points[1]);
        Vector3 tDir = GetBezierDirection(seg, points[0], points[1]);
        Vector3 tPos2 = GetBezierPosition(seg- 1f / segments, points[0], points[1]);
        Vector3 tDir2 = GetBezierDirection(seg- 1f / segments, points[0], points[1]);

        Gizmos.color = Color.white;

        Quaternion rot = Quaternion.LookRotation(tDir);
        Quaternion rot2 = Quaternion.LookRotation(tDir2);

        //Handles.PositionHandle(tPos, rot);

        //Vector3 previous = tPos2 + (rot2 * road2D.Vertices[0].point);
        for (int i = 0; i< road2D.Vertices.Length; i++)
        {
            Vector3 roadpoint = road2D.Vertices[i].point;
            Vector3 roadpoint2 = road2D.Vertices[i].point;
            Vector3 prev = tPos2 + (rot2 * roadpoint2);
            Vector3 next = tPos + (rot * roadpoint);
            Gizmos.DrawLine(prev, next);
        }

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
