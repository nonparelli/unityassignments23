using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BezierPoint : MonoBehaviour
{
    public Transform control0; // First control point
    public Transform control1; // Second control point

    public Transform Anchor { get { return gameObject.transform; } }

    void OnValidate()
    {
        // update parent too?? somehow ??
    }

    private void OnDrawGizmos()
    {
        if (control0.hasChanged)
        {
            // move control 1 to opposite rotation
            Debug.Log("Control 0 changed.");
            adjustControlPoints(0);
            control1.transform.hasChanged = false;
            control0.transform.hasChanged = false;
        }
        else if (control1.hasChanged)
        {
            Debug.Log("Control 1 changed.");
            adjustControlPoints(1);
            control0.transform.hasChanged = false;
            control1.transform.hasChanged = false;
        }

        Gizmos.color = Color.black;
        Gizmos.DrawLine(control0.position, Anchor.position);
        Gizmos.DrawLine(control1.position, Anchor.position);

    }
    // Line up beziers somehow :/
    private void adjustControlPoints(int rulingIndex)
    {

        Vector3 vec_a_to_c0 = control0.position - Anchor.position;
        Vector3 vec_a_to_c1 = control1.position - Anchor.position;

        if (rulingIndex == 0)
        {
            control1.position = Anchor.position - vec_a_to_c0.normalized * vec_a_to_c1.magnitude;
        }
        else
        {
            control0.position = Anchor.position - vec_a_to_c1.normalized * vec_a_to_c0.magnitude;
        }

        //this.NeedToUpdateMesh = true;
    }
}
