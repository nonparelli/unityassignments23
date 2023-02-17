using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WorldToLocal : MonoBehaviour
{
    public GameObject WorldPoint;

    public float Local_x;
    public float Local_y;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(WorldPoint.transform.position, 0.05f);

        Vector2 v = WorldPoint.transform.position - transform.position;

        // Compute local coords using dot product
        Local_x = Vector2.Dot(v, transform.right);
        Local_y = Vector2.Dot(v, transform.up);

        //Draw vector from local world to worldpoint
        DrawVector(transform.position, WorldPoint.transform.position, Color.yellow);

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
