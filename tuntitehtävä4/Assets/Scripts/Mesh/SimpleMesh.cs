using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMesh : MonoBehaviour
{
    [Range(3, 255)]
    public int N = 8;
    [Range(0, 5)]
    public float Radius = 1.0f;
    [Range(2, 10)]
    public float OuterRadius = 2.0f;

    public bool isDonut = false;

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
            Vector3 v = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
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
    private void GenerateDonut()
    {
        Mesh mesh = new Mesh();

        // Vertices
        List<Vector3> verts = new List<Vector3>();
        // UVs
        List<Vector3> uvs = new List<Vector3>();

        // Normals
        List<Vector3> normals = new List<Vector3>();


        for (int i = 0; i < N; i++)
        {
            float theta = TAU * i / N; // angle of current iteration
            Debug.Log("Angle: " + theta + ", which in deg is: " + 360f * theta / TAU);
            Vector3 v = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
            verts.Add(v * Radius);
            verts.Add(v * OuterRadius);

            // Better UVs...
            // Midpoint (as a vector) + current vector
            Vector2 mid = new Vector2(0.5f, 0.5f);
            Vector2 s = v * 0.5f;
            uvs.Add(mid+s*(Radius/OuterRadius)); // Inner UV
            uvs.Add(mid +s); // Outer UV
            // Not great UVs :(
            /*
            uvs.Add(new Vector2(i / (float)N,0)); // Inner UV
            uvs.Add(new Vector2(i / (float)N, 1)); // Outer UV
            */

            normals.Add(Vector3.forward);
            normals.Add(Vector3.forward);
        }
        mesh.SetVertices(verts); // Verts


        List<int> tri_indices = new List<int>();
        for (int i = 0; i < N - 1; i++)
        {
            int InnerFirst = 2 * i;
            int OuterFirst = InnerFirst + 1;
            int InnerSecond = OuterFirst + 1;
            int OuterSecond = InnerSecond + 1;
            //first tri
            tri_indices.Add(InnerFirst);
            tri_indices.Add(OuterFirst);
            tri_indices.Add(OuterSecond);
            // second tri
            tri_indices.Add(InnerFirst);
            tri_indices.Add(OuterSecond);
            tri_indices.Add(InnerSecond);
        }
        int InFirst = 2 * (N - 1);
        int OutFirst = InFirst + 1;
        int InSecond = 0;
        int OutSecond = 1;

        tri_indices.Add(InFirst);
        tri_indices.Add(OutFirst);
        tri_indices.Add(OutSecond);
        tri_indices.Add(InFirst);
        tri_indices.Add(OutSecond);
        tri_indices.Add(InSecond);

        mesh.SetTriangles(tri_indices, 0); // Trinagles

        //mesh.RecalculateNormals(); // Normals
        mesh.SetNormals(normals);

        mesh.SetUVs(0, uvs); // UVs

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
    void Start()
    {
        if (isDonut)
            GenerateDonut();
        else
            GenerateMesh();
    }

    private void OnValidate()
    {
        if(isDonut)
            GenerateDonut();
        else
         GenerateMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }


}
