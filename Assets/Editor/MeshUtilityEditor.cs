using UnityEngine;
using UnityEditor;

/// <summary>Scene上にMeshのパラメーターを表示する拡張くらす</summary>
[CustomEditor(typeof(MeshUtility))]
public class MeshUtilityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var drawMeshParam = (MeshUtility)target;

        if (GUILayout.Button("表示"))
        {
            drawMeshParam.ScenePreview();
        }
    }
}
