using System;
using UnityEngine;

/// <summary>Meshに関する処理をまとめたクラス</summary>
public class MeshUtility : MonoBehaviour
{
    [SerializeField, Header("対象のオブジェクト")]
    GameObject[] _targetObjects = default;
    [SerializeField, Header("lineの表示時間"), Min(0.1f)]
    float _duration = 0.1f;
    [SerializeField, Header("lineの長さ"), Min(0.1f)]
    float _lineLength = 0.1f;
    [SerializeField, Header("法線を表示 赤線")]
    bool _viewNomals = false;
    [SerializeField, Header("接線を表示 青線")]
    bool _viewTangents = false;
    [SerializeField, Header("従法線を表示 緑線")]
    bool _viewBinormals = false;

    /// <summary>実行中以外に線を表示する</summary>
    public void ScenePreview()
    {
        if (_viewNomals && _viewBinormals && _viewTangents)     //全て表示
        {
            Array.ForEach(_targetObjects, t => DrawLines(t, _duration, _lineLength));
            Debug.Log("法線, 接線,従法線を表示しました");
            return;
        }

        if (_viewNomals)    //法線を表示
        {
            Array.ForEach(_targetObjects, t => DrawVertexPosition(t, _duration, _lineLength));
            Debug.Log("法線を表示しました");
        }

        if (_viewTangents)  //接線を表示
        {
            Array.ForEach(_targetObjects, t => DrawTangents(t, _duration, _lineLength));
            Debug.Log("接線を表示しました");
        }

        if (_viewBinormals) //従法線を表示
        {
            Array.ForEach(_targetObjects, t => DrawBinormals(t, _duration, _lineLength));
            Debug.Log("従法線を表示しました");
        }
    }

    /// <summary>オブジェクトの頂点と法線を描画する 
    /// <para>赤線で描画</para>
    /// </summary>
    /// <param name="target">対象オブジェクト</param>
    /// <param name="duration">lineの表示時間</param>
    /// <param name="lineLength">lineの長さ</param>
    public static void DrawVertexPosition(GameObject target, float duration, float lineLength = 0.1f)
    {
        if (!target.TryGetComponent(out MeshFilter meshFilter))
        {
            Debug.LogError("対象のオブジェクトにMeshFilterをアタッチしてください。");
            return;
        }

        var vertices = meshFilter.sharedMesh.vertices;

        for (var i = 0; i < vertices.Length; i++)
        {
            var vertex = vertices[i];
            var nomal = meshFilter.sharedMesh.normals[i];
            var startPos = target.transform.TransformPoint(vertex);
            var endPos = target.transform.TransformPoint(nomal * lineLength + vertex);
            Debug.DrawLine(startPos, endPos, Color.red, duration);
        }
    }

    /// <summary>
    /// オブジェクトの一部頂点と法線を描画する
    /// <para>赤線で描画</para>
    /// </summary>
    /// <param name="target">対象オブジェクト</param>
    /// <param name="drawVertices">描画する頂点配列</param>
    /// <param name="duration">lineの表示時間</param>
    /// <param name="lineLength">lineの長さ</param>
    public static void DrawVertexPosition(GameObject target, Vector3[] drawVertices, float duration, float lineLength = 0.1f)
    {
        if (!target.TryGetComponent(out MeshFilter meshFilter))
        {
            Debug.LogError("対象のオブジェクトにMeshFilterをアタッチしてください。");
            return;
        }

        for (var i = 0; i < drawVertices.Length; i++)
        {
            var vertex = drawVertices[i];
            var nomal = meshFilter.sharedMesh.normals[i];
            var startPos = target.transform.TransformPoint(vertex);
            var endPos = target.transform.TransformPoint(nomal * lineLength + vertex);
            Debug.DrawLine(startPos, endPos, Color.red, duration);
        }
    }

    /// <summary>オブジェクトの接線を描画する 
    /// <para>青線で描画</para>
    /// </summary>
    /// <param name="target">対象オブジェクト</param>
    /// <param name="duration">lineの表面時間</param>
    /// <param name="lineLength">lineの長さ</param>
    public static void DrawTangents(GameObject target, float duration, float lineLength = 0.1f)
    {
        if (!target.TryGetComponent(out MeshFilter mesh))
        {
            Debug.LogError("対象のオブジェクトにMeshFilterをアタッチしてください。");
            return;
        }

        var tangents = mesh.sharedMesh.tangents;
        var vertices = mesh.sharedMesh.vertices;

        for (var i = 0; i < tangents.Length; i++)
        {
            var vertex = vertices[i];
            var tangent = tangents[i];

            var startPos = target.transform.TransformPoint(vertex);
            var endPos = target.transform.TransformPoint(new Vector3(tangent.x, tangent.y, tangent.z) * lineLength + vertex);

            Debug.DrawLine(startPos, endPos, Color.blue, duration);
        }
    }

    /// <summary>オブジェクトの従法線を描画する
    /// <para>緑線で描画</para>
    /// </summary>
    /// <param name="target">対象オブジェクト</param>
    /// <param name="duration">lineの表面時間</param>
    /// <param name="lineLength">lineの長さ</param>
    public static void DrawBinormals(GameObject target, float duration, float lineLength = 0.1f)
    {
        if (!target.TryGetComponent(out MeshFilter mesh))
        {
            Debug.LogError("対象のオブジェクトにMeshFilterをアタッチしてください。");
            return;
        }

        var tangents = mesh.sharedMesh.tangents;
        var vertices = mesh.sharedMesh.vertices;
        var nomals = mesh.sharedMesh.normals;

        for (var i = 0; i < tangents.Length; i++)
        {
            var vertex = vertices[i];
            var tangent = tangents[i];
            var nomal = nomals[i];

            var startPos = target.transform.TransformPoint(vertex);
            var endPos = target.transform.TransformPoint(Vector3.Cross(nomal, new Vector3(tangent.x, tangent.y, tangent.z)) * tangent.w  * lineLength + vertex);

            Debug.DrawLine(startPos, endPos, Color.green, duration);
        }
    }

    /// <summary>
    /// オブジェクトの法線,接線,従法線を描画する 
    /// <para>法線=赤線　接線=青線　従法線=緑線</para>
    /// </summary>
    public static void DrawLines(GameObject target, float duration, float lineLength = 0.1f)
    {
        if (!target.TryGetComponent(out MeshFilter mesh))
        {
            Debug.LogError("対象のオブジェクトにMeshFilterをアタッチしてください。");
            return;
        }

        var tangents = mesh.sharedMesh.tangents;
        var vertices = mesh.sharedMesh.vertices;
        var nomals = mesh.sharedMesh.normals;

        for (var i = 0; i < tangents.Length; i++)
        {
            var vertex = vertices[i];
            var tangent = tangents[i];
            var nomal = nomals[i];

            var startPos = target.transform.TransformPoint(vertex);

            var nEndPos = target.transform.TransformPoint(nomal * lineLength + vertex);
            var tEndPos = target.transform.TransformPoint(new Vector3(tangent.x, tangent.y, tangent.z) * lineLength + vertex);
            var bEndPos = target.transform.TransformPoint(Vector3.Cross(nomal, new Vector3(tangent.x, tangent.y, tangent.z)) * tangent.w * lineLength + vertex);

            Debug.DrawLine(startPos, nEndPos, Color.red, duration);      //法線
            Debug.DrawLine(startPos, tEndPos, Color.blue, duration);     //接線
            Debug.DrawLine(startPos, bEndPos, Color.green, duration);    //従法線
        }
    }

    /// <summary>オリジナルのメッシュをコピーする </summary>
    /// <returns>コピーしたメッシュを返す</returns>
    public static Mesh CopyMesh(Mesh originalMesh)
    {
        var mesh = new Mesh();

        mesh.vertices = originalMesh.vertices;
        mesh.uv = originalMesh.uv;
        mesh.uv2 = originalMesh.uv2;
        mesh.uv3 = originalMesh.uv3;
        mesh.uv4 = originalMesh.uv4;
        mesh.triangles = originalMesh.triangles;

        mesh.bindposes = originalMesh.bindposes;
        mesh.boneWeights = originalMesh.boneWeights;
        mesh.bounds = originalMesh.bounds;
        mesh.colors = originalMesh.colors;
        mesh.colors32 = originalMesh.colors32;
        mesh.normals = originalMesh.normals;
        mesh.subMeshCount = originalMesh.subMeshCount;
        mesh.tangents = originalMesh.tangents;

        return mesh;
    }
}
