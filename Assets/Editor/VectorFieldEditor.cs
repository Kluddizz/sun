using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VectorField))]
public class VectorFieldEditor : Editor
{
  SerializedProperty positivePoleProperty;
  SerializedProperty negativePoleProperty;

  void OnEnable()
  {
    positivePoleProperty = serializedObject.FindProperty("positivePole");
    negativePoleProperty = serializedObject.FindProperty("negativePole");
  }

  public override void OnInspectorGUI()
  {
    VectorField field = (VectorField) target;
    field.width = EditorGUILayout.FloatField("Width", field.width);
    field.height = EditorGUILayout.FloatField("Height", field.height);
    field.depth = EditorGUILayout.FloatField("Depth", field.depth);
    field.numberVectorsPerDimension = EditorGUILayout.IntField("Number Vectors", field.numberVectorsPerDimension);
    field.poleStrength = EditorGUILayout.FloatField("Pole Strength", field.poleStrength);

    serializedObject.Update();
    EditorGUILayout.PropertyField(positivePoleProperty, new GUIContent("Positive Pole"));
    EditorGUILayout.PropertyField(negativePoleProperty, new GUIContent("Negative Pole"));
    serializedObject.ApplyModifiedProperties();

    if (GUILayout.Button("Create Asset")) {
      Texture3D texture = field.CreateTexture();
      UnityEditor.AssetDatabase.CreateAsset(texture, "Assets/MagneticField.asset");
    }
  }
}
