using UnityEngine;

public class AimingCast : MonoBehaviour
{
    public GameObject tank;

    private void Start()
    {
    }
    private void OnDrawGizmos()
    {
        Vector3 lookfrom = transform.position;
        Vector3 direction = transform.forward;

        RaycastHit hit;

        Physics.Raycast(lookfrom, direction, out hit, Mathf.Infinity);
        Debug.DrawRay(lookfrom, direction * hit.distance);
        //This is the hit normal
        MyGizmos.DrawVectorDir(hit.point, hit.normal, Color.green);

        //This should be perpendicular to normal and player looking direction
        Vector3 hitRight = Vector3.Cross(hit.normal, direction);
        MyGizmos.DrawVectorDir(hit.point, hitRight, Color.red);

        // Now we cross THOSE
        Vector3 hitForward = Vector3.Cross(hitRight, hit.normal);
        MyGizmos.DrawVectorDir(hit.point, hitForward, Color.blue);

        // Move tank to spot
        tank.transform.position = hit.point;
        // Set tank transforms
        tank.transform.right = hitRight;
        tank.transform.forward = hitForward;

        // This is the class version
        //Vector3 lookfrom = transform.position;
        //Vector3 direction = transform.forward;
        //Ray ray = new Ray(lookfrom, direction);
        //if (Physics.Raycast(ray, out RaycastHit hit))
        //{
        //    Gizmos.color = Color.magenta;
        //    Gizmos.DrawLine(lookfrom, hit.point);

        //    DrawVectorDir(hit.point, hit.normal, Color.green);

        //    Vector3 right = Vector3.Cross(direction, hit.normal);
        //    DrawVectorDir(hit.point,right,Color.red);
        //}
    }
}
