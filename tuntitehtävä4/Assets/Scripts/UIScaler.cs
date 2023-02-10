using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UIScaler : MonoBehaviour
{
    [Range(100,1200)]
    public int canvas_h, canvas_w;
    [Range(10, 1200)]
    public int box_h, box_w;

    private void OnDrawGizmos()
    {
        //Handles.DrawWireCube();  // TOO EASY!!!

        //DrawBox(1024, 768);
        DrawBox(canvas_w, canvas_h);
        //DrawBox(640, 480);
        DrawCenteredBox(box_w, box_h);
    }

    private void DrawCenteredBox(int width, int height)
    {
        Gizmos.DrawLine(transform.position + new Vector3((canvas_w - width) / 2, (canvas_h - height) / 2), transform.position + new Vector3((canvas_w - width) / 2, (canvas_h - height) / 2 + height));
        Gizmos.DrawLine(transform.position + new Vector3((canvas_w - width) / 2, (canvas_h - height) / 2), transform.position + new Vector3((canvas_w - width) / 2 + width, (canvas_h - height) / 2));
        Gizmos.DrawLine(transform.position + new Vector3((canvas_w - width) / 2, (canvas_h - height) / 2 + height), transform.position + new Vector3((canvas_w - width) / 2 + width, (canvas_h - height) / 2 + height));
        Gizmos.DrawLine(transform.position + new Vector3((canvas_w - width) / 2 + width, (canvas_h - height) / 2), transform.position + new Vector3((canvas_w - width) / 2 + width, (canvas_h - height) / 2 + height));

    }

    private void DrawBox(int width, int height)
    {
        Gizmos.DrawLine(transform.position + new Vector3(0, 0), transform.position + new Vector3(0, height));
        Gizmos.DrawLine(transform.position + new Vector3(0, 0), transform.position + new Vector3(width, 0));
        Gizmos.DrawLine(transform.position + new Vector3(0, height), transform.position + new Vector3(width, height));
        Gizmos.DrawLine(transform.position + new Vector3(width, height), transform.position + new Vector3(width, 0));
    }

}
