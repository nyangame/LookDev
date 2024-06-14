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

        if (GUILayout.Button("�ύX"))
        {
            m_Component.SmoothMeshFilter();
        }
        if (GUILayout.Button("���Z�b�g"))
        {
            m_Component.ResetMesh();
        }
    }
}
