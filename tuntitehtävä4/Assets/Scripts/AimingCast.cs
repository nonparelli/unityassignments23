using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class AimingCast : MonoBehaviour
{
    bool instantiatedTank = false;
    public GameObject tank;

    private void Start()
    {
    }
    private void OnDrawGizmos()
    {

        RaycastHit hit;
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(hit.point, 0.05f);
        DrawVector(hit.point, hit.point+hit.normal, Color.magenta);

        //This should be perpendicular to normal and player looking direction
        Vector3 upHit = (hit.point - transform.position);
        Vector3 hitsNormal = Vector3.Cross(hit.normal, upHit);
        DrawVector(hit.point,hit.point+hitsNormal, Color.cyan);

        // Now we cross THOSE
        Vector3 hitForward = Vector3.Cross(hitsNormal, hit.normal);
        DrawVector(hit.point, hit.point + hitForward, Color.blue);

        // What is the looked looking at?
        // if we project our looking vector according to the direction of our eye....?
        //float lookingScalar = Vector3.Dot(hitForward, transform.position + transform.right);
        //Vector3 trueLook = hitForward*lookingScalar;
        //DrawVector(hit.point, hit.point + trueLook, Color.black);

        //WHAT ARE OUR AXES?!
        DrawVector(transform.position, transform.position+transform.right, Color.green);
        DrawVector(transform.position, transform.position + transform.up, Color.red);
        DrawVector(transform.position, transform.position + transform.forward, Color.red);

        // Move tank to spot
        tank.transform.position = hit.point;
        // Create new transform for object....
        tank.transform.right = hitForward;

        // We need
        // Right
        // Up
        // Forward

        // WHOOPS my directions are screwed i guess....
        // Forward should be right i guess?
        // Right should be perpendicular to the magenta vector and player.forward
        //This is the Side of the tank i guess?
        //Vector3 tankRight = Vector3.Cross(hit.normal, transform.position+transform.right);
        tank.transform.forward = hitsNormal;
        // Forward should be perpendicular to Right and Up of tank
        //tank.transform.up = hit.normal;
        Vector3 tankForward = Vector3.Cross(tank.transform.forward, tank.transform.up);
        tank.transform.right = tankForward;

    }

    private void DrawVector(Vector3 from, Vector3 to, Color c)
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
