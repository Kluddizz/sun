using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class VectorField : MonoBehaviour
{
    public float width = 1.0f;
    public float height = 1.0f;
    public float depth = 1.0f;
    public int numberVectorsPerDimension = 10;
    public MagneticPotential positivePole;
    public MagneticPotential negativePole;
    public float poleStrength = 1.0f;
    public float strengthFactor = 1.0f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public Texture3D CreateTexture()
    {
      int size = numberVectorsPerDimension;
      FieldVector[,,] vectorField = InitializeVectors(new Vector3Int(size, size, size));

      Color[] colors = new Color[size * size * size];
      Texture3D texture = new Texture3D(size, size, size, TextureFormat.RGBA32, true);
      float r = 1 / (size - 1);

      int x = vectorField.GetLength(0);
      int y = vectorField.GetLength(1);
      int z = vectorField.GetLength(2);

      for (int i = 0; i < z; i++)
      {
        for (int j = 0; j < y; j++)
        {
          for (int k = 0; k < x; k++)
          {
            FieldVector v = vectorField[k, j, i];
            colors[i + (j * size) + (k * size * size)] = new Color(v.direction.x, v.direction.y, v.direction.z, 1.0f);
          }
        }
      }

      texture.SetPixels(colors);
      texture.Apply();
      return texture;
    }

    private Vector3 GetMagneticDirection(Vector3 magneticFieldPosition)
    {
        Vector3 length = positivePole.position - negativePole.position;
        Vector3 moment = poleStrength * length;

        Vector3 center = negativePole.position + (length / 2.0f);
        Vector3 pos = magneticFieldPosition - center;

        float r = pos.magnitude;
        Vector3 rn = pos.normalized;
        Vector3 field = (float)(10e-7) * ((3.0f * rn * Vector3.Dot(moment, rn) - moment) / (Mathf.Pow(r, 3)));

        return field;
    }

    private FieldVector[,,] InitializeVectors(Vector3Int dimensions)
    {
        float stepX = width / (dimensions.x - 1);
        float stepY = height / (dimensions.y - 1);
        float stepZ = depth / (dimensions.z - 1);
        FieldVector[,,] vectors = new FieldVector[dimensions.z, dimensions.y, dimensions.x];

        for (int z = 0; z < dimensions.z; z++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                for (int x = 0; x < dimensions.x; x++)
                {
                    Vector3 position = new Vector3(
                        (-width/2 + x * stepX) * width,
                        (-height/2 + y * stepY) * height,
                        (-depth/2 + z * stepZ) * depth
                    );

                    Vector3 magneticForce = GetMagneticDirection(position);
                    Vector3 direction = magneticForce.normalized;
                    float strength = magneticForce.magnitude;

                    vectors[z, y, x] = new FieldVector(position, direction, strength);
                }
            }
        }

        return vectors;
    }

    void DrawLineGizmos(float w, float h, float d, float[] positions)
    {
        for (int i = 0; i < positions.Length / 3 / 2; i++)
        {
            Vector3 from = new Vector3(
                positions[i * 3 * 2 + 0]*w,
                positions[i * 3 * 2 + 1]*h,
                positions[i * 3 * 2 + 2]*d
            );

            Vector3 to = new Vector3(
                positions[i * 3 * 2 + 3]*w,
                positions[i * 3 * 2 + 4]*h,
                positions[i * 3 * 2 + 5]*d
            );

            Gizmos.DrawLine(from, to);
        }
    }

    void DrawBoundsGizmo()
    {
        DrawLineGizmos(width, height, depth, new float[] {
            // Face Front
            -0.5f, 0.5f, 0.5f,
             0.5f, 0.5f, 0.5f,
             0.5f, 0.5f, 0.5f,
             0.5f, -0.5f, 0.5f,
             0.5f, -0.5f, 0.5f,
            -0.5f, -0.5f, 0.5f,
            -0.5f, -0.5f, 0.5f,
            -0.5f, 0.5f, 0.5f,

            // Face Back
            -0.5f, 0.5f, -0.5f,
             0.5f, 0.5f, -0.5f,
             0.5f, 0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, 0.5f, -0.5f,

            // Lines Top
             0.5f, 0.5f, -0.5f,
             0.5f, 0.5f, 0.5f,
            -0.5f, 0.5f, -0.5f,
            -0.5f, 0.5f, 0.5f,

            // Lines Bottom
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f, 0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f, 0.5f
        });
    }

    void DrawVectorGizmos()
    {
        FieldVector[,,] vectors = InitializeVectors(new Vector3Int(15, 15, 15));

        foreach (FieldVector v in vectors) {
            // Gizmos.color = Color.Lerp(new Color(0.0f, 1.0f, 0.0f), new Color(1.0f, 0.0f, 0.0f), v.strength * 1000.0f * strengthFactor);
            Gizmos.color = new Color(v.direction.x, v.direction.y, v.direction.z);
            DrawArrowGizmo(v.position, v.direction, 0.05f);
        }
    }

    void DrawArrowGizmo(Vector3 position, Vector3 direction, float scale)
    {
        Matrix4x4 m = Gizmos.matrix;

        if (direction != Vector3.zero) {
          Gizmos.matrix = Matrix4x4.TRS(position, Quaternion.LookRotation(direction, Vector3.back), Vector3.one * scale);
        } else {
          Gizmos.matrix = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one * scale);
        }

        Gizmos.DrawLine(
            new Vector3(0.0f, 0.0f, -0.5f),
            new Vector3(0.0f, 0.0f, 0.5f)
        );

        Gizmos.DrawLine(
            new Vector3(0.0f, 0.0f, 0.5f),
            new Vector3(0.25f, 0.0f, 0.0f)
        );

        Gizmos.DrawLine(
            new Vector3(0.0f, 0.0f, 0.5f),
            new Vector3(-0.25f, 0.0f, 0.0f)
        );

        Gizmos.matrix = m;
    }

    void DrawPotentialGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(positivePole.position, 0.05f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(negativePole.position, 0.05f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        DrawBoundsGizmo();
        DrawVectorGizmos();
        DrawPotentialGizmos();
    }
}
