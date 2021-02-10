using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraMovement : MonoBehaviour
{

    private Camera cam;
    [SerializeField] private float targetZoom;
    public float zoomFactor = 1f;
    private float upperBounds = 100f;
    private float lowerBounds = 0.1f;
    public float zoomSpeed = 5;
    public Material sunMaterial;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        targetZoom = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");
        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, lowerBounds, upperBounds);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

        sunMaterial.SetFloat("_ZoomVal", targetZoom);
    }
}
