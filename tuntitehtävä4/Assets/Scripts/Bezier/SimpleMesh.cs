using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMesh : MonoBehaviour
{
    [Range(3, 255)]
    public int N = 8;
    public float Radius = 1.0f;

    private float TAU = 2 * Mathf.PI;

    private void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        // Circular Mesh
        List<Vector3> verts = new List<Vector3>();
        verts.Add(Vector3.zero); // Add the center point of the "circle"
        //Vector3 v = Vector3.up * Radius;
        //verts.Add(v); // Add the first (zeroeth) vertex, which is just upwards
        for (int i = 0; i < N; i++)
        {
            float theta = TAU * i / N; // angle of current iteration
            Debug.Log("Angle: " + theta + ", which in deg is: " + 360f * theta / TAU);
            Vector3 v = new Vector3(Mathf.Cos(theta),
                                    Mathf.Sin(theta),
                                    0);
            verts.Add(v * Radius);
        }
        mesh.SetVertices(verts);
        List<int> tri_indices = new List<int>();
        for (int i = 1; i < N; i++)
        {
            tri_indices.Add(0);
            tri_indices.Add(i);
            tri_indices.Add(i + 1);
        }
        tri_indices.Add(0);
        tri_indices.Add(N);
        tri_indices.Add(1);
        mesh.SetTriangles(tri_indices, 0);

        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = mesh;

        /*
        Vector3[] verts = {
        new Vector3 (-1f, 1f, 0f),
        new Vector3(1f, 1f, 0f),
        new Vector3(-1f, -1f, 0f),
        new Vector3(1f, -1f, 0f),
        };

        int[] tri_indices = {
        2, 1, 0,
        2, 3, 1
        };

        mesh.vertices = verts;
        mesh.SetTriangles(tri_indices, 0);
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = mesh;
        */
    }
    void Start()
    {

        GenerateMesh();

    }

    private void OnValidate()
    {
        GenerateMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }


}
