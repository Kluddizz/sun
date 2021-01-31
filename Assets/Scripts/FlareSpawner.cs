using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.VFX;
using UnityEngine;

public class FlareSpawner : MonoBehaviour
{
  public int numberFlares = 10;
  public float radius = 1.0f;
  public float delaySeconds;
  public float skipDelayForParticles;
  public GameObject flarePrefab;

  private List<Vector3> positions = new List<Vector3>();

  void Start()
  {
      delaySeconds = 0.5f;
      StartCoroutine(SpawnParticles());
  }

  IEnumerator SpawnParticles()
    {
        positions = FibonacciSphere.GeneratePoints(numberFlares, radius - 0.025f);
        float currentFlares = 0f;

        foreach (Vector3 p in positions)
        {
            currentFlares++;
            Quaternion rotation = Quaternion.LookRotation(-p, Vector3.up);
            GameObject flareInstance = GameObject.Instantiate(flarePrefab, p, rotation);
            flareInstance.transform.parent = gameObject.transform;

            yield return new WaitForSeconds(delaySeconds);
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
