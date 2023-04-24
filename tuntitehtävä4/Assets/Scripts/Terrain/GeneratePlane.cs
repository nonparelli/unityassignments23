using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlane : MonoBehaviour
{
    [Range(1.0f, 1000.0f)]
    public float Size = 100.0f;
    [Range(2, 255)]
    public int Segments = 10;

    [Range(0f, 20f)]
    public float Amplitude = 1f;

    [Range(1f, 10f)]
    public float Factor1,Factor2,Factor3,Factor4;
    public bool FlattenBelowZero;
    private Mesh mesh;

    private void OnValidate()
    {
        if (mesh == null) { 
        mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
        GenerateMesh();
    }

    void GenerateMesh()
    {
        mesh.Clear();

        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();
        float x = 0f;
        float y = 0f;
        float delta = Size / (float)Segments;

        // Generate verts
        for (int seg_x = 0; seg_x <= Segments; seg_x ++)
        {
            x = (float)seg_x * delta;
            for (int seg_y = 0; seg_y <= Segments; seg_y ++)
            {
                y = (float)seg_y * delta;
                // noise
                float z1 = Factor1*(Mathf.PerlinNoise(x /39f, y /39f) - 0.5f);
                float z2 = Factor2*(Mathf.PerlinNoise(x / 17f, y / 17f) - 0.5f);
                float z3 = Factor3*(Mathf.PerlinNoise(x / 7f, y / 7f) - 0.5f);
                float z4 = Factor4*(Mathf.PerlinNoise(x, y) - 0.5f);

                float z = z1 + z2 + z3 + z4;
                //float z = (Mathf.PerlinNoise(x, y) - 0.5f);
                if (FlattenBelowZero && z < 0f)
                    z = 0f;
                verts.Add(new Vector3(x, z*Amplitude, y)); // add current position in plane
                uvs.Add(new Vector2(x, y));
            }
        }

        //Generate tri indices
        for(int seg_x = 0;seg_x<Segments;seg_x++)
        {
            for (int seg_y = 0; seg_y < Segments; seg_y++)
            {
                int index = seg_x * (Segments + 1) + seg_y;
                int index_lower = index + 1;
                int index_next_col = index + (Segments + 1);
                int index_next_col_lower = index_next_col + 1;


                tris.Add(index);
                tris.Add(index_lower);
                tris.Add(index_next_col);

                
                tris.Add(index_next_col_lower);
                tris.Add(index_next_col);
                tris.Add(index_lower);


            }
        }

        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        //mesh.RecalculateNormals();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
