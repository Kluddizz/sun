using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.VFX;
using UnityEngine;

public class FlareSpawner : MonoBehaviour
{
  public int numberFlares = 10;
  public float radius = 1.0f;
  public VisualEffect flareEffect;

  private List<Vector3> positions = new List<Vector3>();

  void Start()
  {
    positions = FibonacciSphere.GeneratePoints(numberFlares, radius);

    foreach (Vector3 p in positions)
    {
      Quaternion rotation = Quaternion.LookRotation(p, Vector3.up);
      VisualEffect effect = Object.Instantiate<VisualEffect>(flareEffect, p, rotation);

      new Thread(() => {
        Thread.Sleep(5000);
        effect.Play();
      }).Start();
    }
  }

  void OnDrawGizmos()
  {
    List<Vector3> positions = new List<Vector3>();
    positions = FibonacciSphere.GeneratePoints(numberFlares, radius);

    foreach (Vector3 v in positions)
    {
      Gizmos.color = Color.red;
      Gizmos.DrawSphere(v, 0.01f);
    }
  }
}
