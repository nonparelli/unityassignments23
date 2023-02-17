using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int MaxBounces;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        int BounceCounter = 0;
        RaycastHit hit;
        // While BounceCounter < MaxBounces
        // Check for hit
        // Then cast new ray alongside normal of collider
        //The first ray is cast, hit is outputted
        // Fuck the z wanders of
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity))
        {
            //Draw first laser
            Debug.DrawLine(transform.position, hit.point, Color.red);
            //Draw hit location
            Gizmos.DrawSphere(hit.point, 0.05f);
            //Find bounce direction
            Vector3 bounceDir = Vector3.Reflect((hit.point - transform.position), hit.normal);
            // Now we cast the rest of the hits
            //WHY DOES THIS BREAK ON THE FIFTH CAST??
            for (BounceCounter = 1; BounceCounter < MaxBounces; BounceCounter++)
            {
                //Set starting point
                Vector3 origin = hit.point;
                //We cast new ray to bounceDir
                Physics.Raycast(origin, bounceDir, out hit, Mathf.Infinity);
                //We draw new line from origin to hit location
                Debug.DrawLine(origin, hit.point, Color.red);
                //Draw hit location
                Gizmos.DrawSphere(hit.point, 0.05f);
                //Calculate new bounde direction
                bounceDir = Vector3.Reflect((hit.point - origin), hit.normal);

            }
        }
    }
}
