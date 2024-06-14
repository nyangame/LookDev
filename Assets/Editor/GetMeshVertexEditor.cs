using UnityEngine;
using UnityEditor;

/// <summary>GetMeshVertexクラス内の処理を呼び出す拡張クラス</summary>
[CustomEditor(typeof(GetMeshVertex))]
public class GetMeshVertexEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var meshVertex = (GetMeshVertex)target;

        if (GUILayout.Button("オブジェクトの頂点データを登録する"))
        {
            meshVertex.RegistarSenbeiObject();
        }
    }
}
