using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshSmoothMovement))]
public class MeshSmoothMovementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MeshSmoothMovement m_Component = target as MeshSmoothMovement;

        if (GUILayout.Button("変更"))
        {
            m_Component.SmoothMeshFilter();
        }
        if (GUILayout.Button("リセット"))
        {
            m_Component.ResetMesh();
        }
    }
}
