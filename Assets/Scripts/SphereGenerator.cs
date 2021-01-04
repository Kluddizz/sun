using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGenerator : MonoBehaviour
{
  public int recursionLevel = 1;
  public float radius = 1.0f;
  public Material material;

  void Start()
  {
    // Create components to be able to render the icosphere mesh.
    MeshFilter filter = gameObject.AddComponent<MeshFilter>();
    MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();

    // Generate the mesh.
    filter.sharedMesh = MeshGenerator.GenerateIcoSphereMesh(recursionLevel, radius);

    // Assign the material to the mesh.
    renderer.material = material;
  }

  void OnDrawGizmos()
  {
    Gizmos.color = new Color(0, 0, 0, 0.5f);
    Mesh mesh = MeshGenerator.GenerateIcoSphereMesh(recursionLevel, radius);
    Gizmos.DrawWireMesh(mesh, 0, transform.position, Quaternion.identity, Vector3.one * radius);
  }
}
