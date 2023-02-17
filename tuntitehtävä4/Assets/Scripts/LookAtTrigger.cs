using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEngine.UI.Image;
using Unity.VisualScripting;

public class LookAtTrigger : MonoBehaviour
{
    public GameObject LookAtDirection;
    public GameObject LookAtTarget;

    [Range(0f,1f)]
    public float LookAtThreshold;
    public float scalarDot;

    public float degrees;

    //Create a vector projection of target in line with lookat direction
    // once the distance between the projection and the direction is small enough, target is seen
    private void OnDrawGizmos()
    {

        // This whole tihng only works if our LookAtTrigger is at 0,0.... otherwise the calculations freak out........
        // Probably because stuff is still being treated as relative to 0,0 instead of the LookAtTrigger.
        // We would need to set stuff as this trigger's children and use localpositions instead..
        //LookAtDirection.transform.SetParent(transform, true);
        //LookAtTarget.transform.SetParent(transform, true);
        // THIS LOOKS MESSY BC I TRIED TO FIX THIS POROBLEM :(
        // I wanted to do it without having to parent the objects
        Vector2 vecDir = LookAtDirection.transform.position;
        Vector2 vecTarget = LookAtTarget.transform.position;

        float vecDirMagnitude = Mathf.Sqrt(vecDir.x * vecDir.x + vecDir.y * vecDir.y);
        float vecTargetMagnitude = Mathf.Sqrt(vecTarget.x * vecTarget.x + vecTarget.y * vecTarget.y);

        Vector2 vecDirN = vecDir / vecDirMagnitude;
        Vector2 vecTargetN = vecTarget / vecTargetMagnitude;

        // HUH what the heck is wrong wasn't i doing it right
        vecDirN = vecDir.normalized;
        vecTargetN = vecTarget.normalized;

        scalarDot = (vecDirN.x * vecTargetN.x) + (vecDirN.y * vecTarget.y);

        //Are my calcs just fucked??? why does it not work manually but works with these
        scalarDot = Vector2.Dot(vecDirN, vecTargetN);

        // If target is outside of seeing sector we do NOT color red
        if (scalarDot > LookAtThreshold && vecDirMagnitude > vecTargetMagnitude)   
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }


        Gizmos.DrawSphere(LookAtTarget.transform.position,0.05f);

        // How do we draw the lookat sector?
        // Use Handles.DrawSolidArc probably
        // but we need to find out the angle?!
        // we have our minimum scalarDot value, can we use that to find the degrees?
        // a*b = ||a|| ||b|| cos X
        // cos angle = a*b/|a||b|
        // so
        // cos angle = dot product / a.magnitude * b.magnitude, so scalarDot..?
        // so angle should be arccos(scalarDot) ?
        degrees = Mathf.Rad2Deg*Mathf.Acos(LookAtThreshold);

        Handles.color = Color.Lerp(Color.magenta, Color.clear, 0.9f);
        //We have two of these for both sides because im too lazy to calculate a real starting position for this vector...
        Debug.Log(LookAtDirection.transform.position);
        Handles.DrawSolidArc(transform.position,Vector3.forward, LookAtDirection.transform.localPosition, degrees,Vector2.Distance(transform.position, LookAtDirection.transform.position));
        Handles.DrawSolidArc(transform.position, Vector3.forward, LookAtDirection.transform.localPosition, -degrees, Vector2.Distance(transform.position, LookAtDirection.transform.position));
    }
}
