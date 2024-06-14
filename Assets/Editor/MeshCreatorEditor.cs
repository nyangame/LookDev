using UnityEngine;
using UnityEditor;

/// <summary>inspector��Ń��b�V���쐬������ </summary>
[CustomEditor(typeof(MeshCreator))]
public class MeshCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var meshCreator = (MeshCreator)target;

        if (GUILayout.Button("Mesh���쐬"))
        {
            meshCreator.MeshCreate();
        }
    }
}
