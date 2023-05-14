using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BezierPath : MonoBehaviour
{
    [SerializeField]
    Mesh2D road2D;

    [SerializeField]
    bool continuous; // Use to draw last segment

    [Range(0f, 1f)]
    public float t = 0.0f, v = 0.0f;
    [Range(1, 30)]
    public int segments = 1;
    public BezierPoint[] points;
    [SerializeField]
    private Mesh meshy;

    // vertices
    List<Vector3> verts = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    List<int> tri_indices = new List<int>();
    // Start is called before the first frame update
    private void OnValidate()
    {
        GenerateMesh();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            Handles.DrawBezier(points[i].Anchor.position,
                               points[i + 1].Anchor.position,
                               points[i].control1.position,
                               points[i + 1].control0.position, Color.white, null, 2f);
        }

        // The moving bit
        Vector3 tPos = GetBezierPosition(t, points[0], points[1]);
        Vector3 tDir = GetBezierDirection(t, points[0], points[1]);

        // Draw the position on the curve
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(tPos, 0.25f);

        // Try to get the rotation

        Quaternion rot = Quaternion.LookRotation(tDir);
        Handles.PositionHandle(tPos, rot); // The moving thing i think ??

        // Draw road segments
        // DRAWS THE GUIDE WIREFFRAME
        for (int i = 0; i < points.Length - 1; i++)
            DrawBezierPart(points[i], points[i + 1]);

        if(continuous)
            DrawBezierPart(points[points.Length-1],points[0]);

        // V placement should count ALL of the bezier... hmmmmm
        GetPositionAt();

        // onvalidate is not enough
        GenerateMesh();
    }

    private void DrawBezierPart(BezierPoint point0, BezierPoint point1)
    {
        for (int n = 0; n < segments; n++)
        {
            float tTest = n / (float)segments;

            Vector3 tPos = GetBezierPosition(tTest, point0, point1);
            Vector3 tDir = GetBezierDirection(tTest, point0, point1);
            Quaternion rot = Quaternion.LookRotation(tDir);

            float tTestNext = (n + 1) / (float)segments;
            Vector3 tPosNext = GetBezierPosition(tTestNext, point0, point1);
            Vector3 tDirNext = GetBezierDirection(tTestNext, point0, point1);
            Quaternion rotNext = Quaternion.LookRotation(tDirNext);
            for (int i = 0; i < road2D.Vertices.Length; i++)
            {
                Vector3 roadpoint = road2D.Vertices[i].point;
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(tPos + (rot * roadpoint), 0.25f);
                Gizmos.DrawSphere(tPosNext + (rotNext * roadpoint), 0.25f);
                Gizmos.color = Color.white;
                Gizmos.DrawLine(tPos + rot * roadpoint, tPosNext + rotNext * roadpoint);
            }
            Vector3 roadpointNext = tPos + rot * road2D.Vertices[0].point;
            for (int i = 0; i < road2D.Vertices.Length; i++)
            {
                Vector3 roadpoint = tPos + rot * road2D.Vertices[i].point;
                Gizmos.DrawLine(roadpointNext, roadpoint);
                roadpointNext = roadpoint;
            }

            // Do the last damn one
            tPos = GetBezierPosition(1f, point0, point1);
            tDir = GetBezierDirection(1f, point0, point1);
            rot = Quaternion.LookRotation(tDir);
            roadpointNext = tPos + rot * road2D.Vertices[0].point;
            for (int i = 0; i < road2D.Vertices.Length; i++)
            {
                Vector3 roadpoint = tPos + rot * road2D.Vertices[i].point;
                Gizmos.DrawLine(roadpointNext, roadpoint);
                roadpointNext = roadpoint;
            }
        }

    }

    void GenerateMesh()
    {
        // Clear all lists before generation
        verts.Clear();
        uvs.Clear();
        tri_indices.Clear();

        for (int i = 0; i < points.Length - 1; i++) {
            GenerateMeshForBezierPart(points[i], points[i + 1]);
            GenerateTrianglesForBezierPart(i);
        }
        if (continuous)
        {
            GenerateMeshForBezierPart(points[points.Length-1], points[0]);
            GenerateTrianglesForBezierPart(points.Length-1);
        }

        if(meshy != null)
           meshy.Clear();
        else
            meshy=new Mesh();
        meshy.SetVertices(verts);
        //mesh.triangles = tri_indices;
        meshy.SetTriangles(tri_indices, 0);
        // normals
        meshy.RecalculateNormals();
        meshy.SetUVs(0, uvs);
        GetComponent<MeshFilter>().sharedMesh = meshy;

    }
    void GenerateMeshForBezierPart(BezierPoint point0, BezierPoint point1)
    {
        for (int n = 0; n <= segments; n++)
        { 
            // T-value of current segment
            float tt = n / (float)segments;

            Vector3 tPos = GetBezierPosition(tt, point0, point1);
            Vector3 tDir = GetBezierDirection(tt, point0, point1);
            Quaternion rot = Quaternion.LookRotation(tDir);

            // Loop through our road slices
            for (int index = 0; index < road2D.Vertices.Length; index++)
            {
                // local point
                Vector3 roadpoint = road2D.Vertices[index].point;
                // local to world-transform
                Vector3 worldpoint = tPos + rot * roadpoint;
                // Add this world point to our verts
                verts.Add(transform.InverseTransformPoint(worldpoint));
                //add uv coord
                uvs.Add(new Vector2(roadpoint.x / 10.0f + 0.5f, tt));
            }
        }
        
    }
    void GenerateTrianglesForBezierPart(int part)
    {
        // triangles
        // How many lines
        int num_lines = road2D.lineIndices.Length / 2;
            int offset = part * (segments + 1) * road2D.Vertices.Length;
            for (int n = 0; n < segments; n++)
            {
                for (int line = 0; line < num_lines; line++)
                {
                    // current slice
                    int curr_first = n * road2D.Vertices.Length +
                                        road2D.lineIndices[2 * line] +
                                        offset;
                    int curr_second = n * road2D.Vertices.Length +
                                         road2D.lineIndices[2 * line + 1] +
                                         offset;
                    // next slice
                    int next_first = curr_first + road2D.Vertices.Length;
                    int next_second = curr_second + road2D.Vertices.Length;
                    // first tri
                    tri_indices.Add(curr_first);
                    tri_indices.Add(next_first);
                    tri_indices.Add(curr_second);
                    // second tri
                    tri_indices.Add(curr_second);
                    tri_indices.Add(next_first);
                    tri_indices.Add(next_second);
                }
            }
 
    }

    void DrawRoadSegment(float seg)
    {
        Vector3 tPos = GetBezierPosition(seg, points[0], points[1]);
        Vector3 tDir = GetBezierDirection(seg, points[0], points[1]);

        Gizmos.color = Color.red;

        Quaternion rot = Quaternion.LookRotation(tDir);
        //Handles.PositionHandle(tPos, rot);

        Vector3 previous = tPos + (rot * road2D.Vertices[0].point);
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
        Vector3 tPos2 = GetBezierPosition(seg - 1f / segments, points[0], points[1]);
        Vector3 tDir2 = GetBezierDirection(seg - 1f / segments, points[0], points[1]);

        Gizmos.color = Color.white;

        Quaternion rot = Quaternion.LookRotation(tDir);
        Quaternion rot2 = Quaternion.LookRotation(tDir2);

        //Handles.PositionHandle(tPos, rot);

        //Vector3 previous = tPos2 + (rot2 * road2D.Vertices[0].point);
        for (int i = 0; i < road2D.Vertices.Length; i++)
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
        Vector3 PtZ = (1 - t) * bp2.control0.position + t * bp2.Anchor.position;

        Vector3 PtR = (1 - t) * PtX + t * PtY;
        Vector3 PtS = (1 - t) * PtY + t * PtZ;

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

    Vector3 GetPositionAt()
    {
        // uses v to place and the amount of points + continuos to calculate amount of segments and which segment to place to
        int p = points.Length-1; // This is the amount of Curves beteen all bezier points
        if (continuous)
        {
            p++; // add the missing segment
        }
        float s = 1f / (float)p; // this is the threshold for skipping between curves.

        // Get how many times s fits in v ? so divide then floor? to see which segment we're on?? T
        int c = Mathf.FloorToInt(Mathf.Clamp(v / s,0,p-1));
        // We need to scale V somehow to our curvecount??
        // The moving bit
        Vector3 tPos = new Vector3();
        Vector3 tDir = new Vector3();
        if (c < points.Length - 1)
        {
            tPos = GetBezierPosition(v / s - c, points[c], points[c + 1]);
            tDir = GetBezierDirection(v / s - c, points[c], points[c + 1]);
        }
        else
        {
            tPos = GetBezierPosition(v / s - c, points[points.Length-1], points[0]);
            tDir = GetBezierDirection(v / s - c, points[points.Length-1], points[0]);
        }
        // Draw the position on the curve
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(tPos, 0.25f);

        // Try to get the rotation

        Quaternion rot = Quaternion.LookRotation(tDir);
        Handles.PositionHandle(tPos, rot); // The moving thing i think ??
        return Vector3.forward;
    }
}
