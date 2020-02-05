using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class triangle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Handles.DrawLine(Vector3.zero, Vector3.right * 10);

        Handles.DrawLine(Vector3.zero, Vector3.left * 10);

        Handles.DrawLine(Vector3.right * 10, Vector3.left * 10);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
