using UnityEngine;
using UnityEditor;

/// <summary>inspector上でメッシュ作成させる </summary>
[CustomEditor(typeof(MeshCreator))]
public class MeshCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var meshCreator = (MeshCreator)target;

        if (GUILayout.Button("Meshを作成"))
        {
            meshCreator.MeshCreate();
        }
    }
}
