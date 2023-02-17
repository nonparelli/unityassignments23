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

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            Gizmos.DrawSphere(hit.point, 0.05f);
            Vector3 dir = Vector3.Reflect((hit.point - transform.position), hit.normal);
            for (BounceCounter = 1; BounceCounter < MaxBounces; BounceCounter++)
            {
                Vector3 origin = hit.point;
                Physics.Raycast(hit.point, dir, out hit, Mathf.Infinity);
                Debug.DrawLine(origin, hit.point, Color.red);
                Gizmos.DrawSphere(hit.point, 0.05f);
                dir = Vector3.Reflect((hit.point - origin), hit.normal);

            }
        }
    }
}
