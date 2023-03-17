using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public GameObject A;
    public GameObject B;
    public GameObject C;
    public GameObject D;

    [Range(0f, 1f)]
    public float T = 0.0f;

    private void OnDrawGizmos()
    {

        Vector3 PtA = A.transform.position;
        Vector3 PtB = B.transform.position;
        Vector3 PtC = C.transform.position;
        Vector3 PtD = D.transform.position;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(PtA, PtB);
        Gizmos.DrawLine(PtB, PtC);
        Gizmos.DrawLine(PtC, PtD);

        // Lerp..... :) :
        Vector3 PtX = (1 - T) * PtA + T * PtB;
        Vector3 PtY = (1 - T) * PtB + T * PtC;
        Vector3 PtZ = (1 - T) * PtC + T * PtD;

        //Draw spheres at points x,y,z
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(PtX, 0.1f);
        Gizmos.DrawSphere(PtY, 0.1f);
        Gizmos.DrawSphere(PtZ, 0.1f);

        //Draw lines from x to y and y to z
        Gizmos.DrawLine(PtX, PtY);
        Gizmos.DrawLine(PtY, PtZ);

        //NEXT LERP!!
        Vector3 PtR = (1 - T) * PtX + T * PtY;
        Vector3 PtS = (1 - T) * PtY + T * PtZ;

        //Draw spheres
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(PtR, 0.1f);
        Gizmos.DrawSphere(PtS, 0.1f);

        //Draw line from r to s
        Gizmos.DrawLine(PtR, PtS);

        //once more
        Vector3 PtO = (1 - T) * PtR + T * PtS;
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(PtO, 0.1f);

        //Draw a curve!
        Vector3 PtT = PtA;
        for(float i =0; i < 1; i += 0.05f)
        {
            Vector3 PtXT = (1 - i) * PtA + i * PtB;
            Vector3 PtYT = (1 - i) * PtB + i * PtC;
            Vector3 PtZT = (1 - i) * PtC + i * PtD;

            Vector3 PtRT = (1 - i) * PtXT + i * PtYT;
            Vector3 PtST = (1 - i) * PtYT + i * PtZT;

            Vector3 PtT2 = (1 - i) * PtRT + i * PtST;
            Gizmos.DrawLine(PtT, PtT2);
            PtT = PtT2;
        }
        Gizmos.DrawLine(PtT, PtD);
    }

}
