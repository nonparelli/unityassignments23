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
    [Range(1, 30)]
    public int segments = 1;
    public BezierPoint[] points;

    private Mesh meshy;
    // Start is called before the first frame update
    private void OnValidate()
    {
        meshy = new Mesh();
        GenerateMesh(meshy);

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

        for (int n = 0; n < segments; n++)
        {
            float tTest = n / (float)segments;

            tPos = GetBezierPosition(tTest, points[0], points[1]);
            tDir = GetBezierDirection(tTest, points[0], points[1]);
            rot = Quaternion.LookRotation(tDir);

            float tTestNext = (n + 1) / (float)segments;
            Vector3 tPosNext = GetBezierPosition(tTestNext, points[0], points[1]);
            Vector3 tDirNext = GetBezierDirection(tTestNext, points[0], points[1]);
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
            tPos = GetBezierPosition(1f, points[0], points[1]);
            tDir = GetBezierDirection(1f, points[0], points[1]);
            rot = Quaternion.LookRotation(tDir);
            roadpointNext = tPos + rot * road2D.Vertices[0].point;
            for (int i = 0; i < road2D.Vertices.Length; i++)
            {
                Vector3 roadpoint = tPos + rot * road2D.Vertices[i].point;
                Gizmos.DrawLine(roadpointNext, roadpoint);
                roadpointNext = roadpoint;
            }
        }

        /*
        for (float i = 0; i < 1f; i += 1f / segments)
            DrawRoadSegment(i);
        DrawRoadSegment(1f);
        //Step 2: draw lines between them? uhhhh
        for (float i = 1f / segments; i < 1f; i += 1f / segments)
            DrawRoadLines(i);
        DrawRoadLines(1f);
        */
    }
    void GenerateMesh(Mesh input)
    {
        Mesh mesh = input;

        // vertices
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        // Go through each segment
        for (int n = 0; n <= segments; n++)
        {
            // T-value of current segment
            float tt = n / (float)segments;

            Vector3 tPos = GetBezierPosition(tt, points[0], points[1]);
            Vector3 tDir = GetBezierDirection(tt, points[0], points[1]);
            Quaternion rot = Quaternion.LookRotation(tDir);

            // Loop through our road slices
            for (int index = 0; index < road2D.Vertices.Length; index++)
            {
                // local point
                Vector3 roadpoint = road2D.Vertices[index].point;
                // local to world-transform
                Vector3 worldpoint = tPos + rot * roadpoint;
                // Add this world point to our verts
                verts.Add(worldpoint);
                //add uv coord
                uvs.Add(new Vector2(roadpoint.x / 10.0f + 0.5f,tt));
            }

        }

        // triangles
        int num_lines = road2D.lineIndices.Length / 2;

        // how many tris * 3
        // int[] tri_indices = new int[num_lines * 2 * segments * 3];

        //Debug.Log("Triangles: " + tri_indices.Length / 3);
        List<int> tri_indices = new List<int>();
        // go through each but keep last segment
        for (int n = 0; n < segments; n++){
            for (int line = 0; line < num_lines; line++){
                // current slice
                int curr_first = n * road2D.Vertices.Length +
                                    road2D.lineIndices[2 * line];
                int curr_second = n * road2D.Vertices.Length +
                                     road2D.lineIndices[2 * line + 1];

                // next slice
                int next_first = curr_first + road2D.Vertices.Length;
                int next_second = curr_second + road2D.Vertices.Length;

                //int tri_index = n * num_lines * 2 + line * 6;
                // first tri
                tri_indices.Add(curr_first);
                tri_indices.Add(next_first);
                tri_indices.Add(curr_second);
                //tri_indices[tri_index] = curr_first;
                //tri_indices[tri_index + 1] = next_first;
                //tri_indices[tri_index + 2] = curr_second;

                // second tri
                tri_indices.Add(curr_second);
                tri_indices.Add(next_first);
                tri_indices.Add(next_second);
                //tri_indices[tri_index + 3] = curr_second;
                //tri_indices[tri_index + 4] = next_first;
                //tri_indices[tri_index + 5] = next_second;
            }
        }
        mesh.Clear();

        mesh.SetVertices(verts);
        //mesh.triangles = tri_indices;
        mesh.SetTriangles(tri_indices, 0);
        // normals
        mesh.RecalculateNormals();
        mesh.SetUVs(0,uvs);
        GetComponent<MeshFilter>().sharedMesh = mesh;

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
}
