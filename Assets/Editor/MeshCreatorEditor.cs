using UnityEngine;
using UnityEditor;

/// <summary>inspectorã‚ÅƒƒbƒVƒ…ì¬‚³‚¹‚é </summary>
[CustomEditor(typeof(MeshCreator))]
public class MeshCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var meshCreator = (MeshCreator)target;

        if (GUILayout.Button("Mesh‚ğì¬"))
        {
            meshCreator.MeshCreate();
        }
    }
}
