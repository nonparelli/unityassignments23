using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RadialTrigger : MonoBehaviour
{
    public float Radius;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnDrawGizmos()
    {
        Handles.color = Color.Lerp(Color.magenta, Color.clear, 0.9f);
        //Check if target is within radius of this object
        //if the distance between this and target is below radius, its triggered
        // Mathf.Sqrt(vecA.x * vecA.x + vecA.y * vecA.y) <- length of vector
       Handles.DrawSolidDisc(transform.position, Vector3.forward,Radius);
        Vector3 dist = transform.position - target.transform.position;
        if (Mathf.Sqrt(dist.x * dist.x + dist.y * dist.y) < Radius)
        {
            Gizmos.color = Color.red;

        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawSphere(target.transform.position, 0.05f);
    }
}
