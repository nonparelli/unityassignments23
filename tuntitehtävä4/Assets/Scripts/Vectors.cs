using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Vectors : MonoBehaviour
{
    // FUCK i have this script on my home computer
    //private Vector3 Origo = new Vector3(0f, 0f, 0f);
    public GameObject A;
    public GameObject B;
    public float scalarDot;
    public float axis_length = 2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDrawGizmos()
    {
        Vector3 origin = Vector3.zero;

        // ACQUIRE DOT PRODUCT
        Vector2 vecA = A.transform.position;
        Vector2 vecB = B.transform.position;

        //float vecAlen = vecA.magnitude;
        //vecAlen = Mathf.Sqrt(vecA.x * vecA.x + vecA.y * vecA.y);
        //Vector2 vecN = vecA/vecAlen;
        Vector2 vecNA = vecA.normalized;
        Vector2 vecNB = vecB.normalized;
        scalarDot = Vector2.Dot(vecNA, vecNB);

        //DRAW X AND Y AXIS
        DrawVector(new Vector3(-axis_length, 0, 0), new Vector3(axis_length, 0, 0), Color.red);
        DrawVector(new Vector3(0, -axis_length, 0), new Vector3(0, axis_length, 0), Color.green);

        // DRAW DISC
        Gizmos.color = Color.white;
        Handles.DrawWireDisc(origin, Vector3.forward, 1.0f);

        // Draw Vector A
        DrawVector(origin, A.transform.position, Color.black);
        Gizmos.color = Color.black;

        // draw normalized vector from a
        DrawVector(origin, vecNA, Color.blue);
        // draw normalized vector from b
        DrawVector(origin, vecNB, Color.blue);

        // Draw Vector B
        DrawVector(origin, B.transform.position, Color.black);
        Gizmos.color = Color.black;


        // Vector projection: Dot(vecN,vecB)*vecN
        //Vector2 vecProj = vecNA * scalarDot;
        // Draw vector projection?
        //DrawVector(origin, vecProj, Color.Lerp(Color.clear, Color.magenta, 0.5f));
    }
    void DrawVector(Vector3 from, Vector3 to, Color c)
    {
        Color curr = Gizmos.color;
        Gizmos.color = c;
        Gizmos.DrawLine(from, to);

        // Compute a location from "to towards from with 30degs"
        Vector3 loc = -(to - from);
        loc = Vector3.ClampMagnitude(loc, 0.1f);
        Quaternion rot30 = Quaternion.Euler(0, 0, 30);
        Vector3 loc1 = rot30 * loc;
        rot30 = Quaternion.Euler(0, 0, -30);
        Vector3 loc2 = rot30 * loc;
        Gizmos.DrawLine(to, to + loc1);
        Gizmos.DrawLine(to, to + loc2);
        Gizmos.color = curr;

    }
}
