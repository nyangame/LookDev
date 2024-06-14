using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SenbeiVertexData))]
public class SenbeiVertexDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("インスペクターでは操作を行わないでください", MessageType.Info);

        DrawDefaultInspector();
    }
}
