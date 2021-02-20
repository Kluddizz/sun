using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraMovement : MonoBehaviour
{

    public Transform near;
    public Transform far;

    private float zoom = 1;
    public float zoomSpeed = 30f;
    public Material sunMaterial;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = far.position;
    }

    // Update is called once per frame
    void Update()
    {
        zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        zoom = Mathf.Clamp(zoom, 0f, 1f);
        transform.position = Vector3.Slerp(near.position, far.position, zoom);

        sunMaterial.SetFloat("_ZoomVal", zoom);
    }

    
}
