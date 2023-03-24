using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mesh2D", menuName = "ScriptableObjects/Mesh2D", order = 1)]
public class Mesh2D : ScriptableObject
{
    [System.Serializable]
    public class Vertex
    {
        public Vector2 point;
        public Vector2 normal;
        //public Vector2 u;
    }

    public Vertex[] Vertices;
    public int[] lineIndices;

}
