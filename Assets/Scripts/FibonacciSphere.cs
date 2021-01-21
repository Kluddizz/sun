using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FibonacciSphere {
  
  public static List<Vector3> GeneratePoints(int count, float radius) {
    List<Vector3> points = new List<Vector3>();
      
    float gr = (Mathf.Sqrt(5.0f) + 1.0f) / 2.0f;
    float ga = (2.0f - gr) * (2.0f * Mathf.PI);

    for (int i = 1; i <= count; ++i) {
        float lat = Mathf.Asin(-1.0f + 2.0f * i / (count + 1));
        float lon = ga * i;

        float x = Mathf.Cos(lon) * Mathf.Cos(lat) * radius;
        float y = Mathf.Sin(lon) * Mathf.Cos(lat) * radius;
        float z = Mathf.Sin(lat) * radius;

        points.Add(new Vector3(x, y, z));
    }
    
    return points;
  }
}