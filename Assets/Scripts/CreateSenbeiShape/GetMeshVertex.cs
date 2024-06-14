using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>頂点を取得するクラス </summary>
public class GetMeshVertex : MonoBehaviour
{
    #region 変数
    /// <summary>円周上の頂点を取得時に使用 </summary>
    const float TOLERANCE = 0.001f;

    [SerializeField, Header("登録名")]
    string _registerName = default;
    [SerializeField, Header("登録するオブジェクト(表面)")]
    MeshFilter _surfaceMeshFilter = default;
    [SerializeField, Header("登録するオブジェクト(裏面)")]
    MeshFilter _backMeshFilter = default;
    [SerializeField, Header("側面オブジェクト")]
    MeshFilter _sideMeshFilter = default;
    [SerializeField, Header("側面一列の頂点数")]
    int _sideVertexCount = 0;
    [SerializeField]
    SenbeiVertexData _senbeiVertexData = default;

    List<Vector3> _surfaceVertices = new List<Vector3>();

    #endregion

    /// <summary>側面の頂点を取得する処理 </summary>
    /// <param name="surface">表面の円周上の頂点</param>
    /// <param name="back">裏面の円周上の頂点</param>
    List<SideVertexData> GetSideVertexData(List<Vector3> surface)
    {
        var list = new List<SideVertexData>();
        var sideVertice = _sideMeshFilter.sharedMesh.vertices;

        for (var i = 0; i < surface.Count; i++)
        {
            var v = surface[i];
            var y = 0f;
            var registerVertices = new Vector3[_sideVertexCount];
            var registerIndexes = new int[_sideVertexCount];
            var index = 0;

            for (var k = 0; k < 50; k++)
            {
                var rayPoint = new Vector3(v.x * 1.1f, v.y - y, v.z * 1.1f);
                var endPoint = new Vector3(v.x, v.y - y, v.z);
                var dir = endPoint - rayPoint;
                RaycastHit hit;

                if (Physics.Raycast(rayPoint, dir, out hit))
                {
                    var p = _sideMeshFilter.sharedMesh.vertices.OrderBy(v => Vector3.Distance(v, hit.point)).FirstOrDefault();

                    if (!registerVertices.Contains(p))
                    {
                        registerVertices[index] = p;
                        registerIndexes[index] = Array.IndexOf(_sideMeshFilter.sharedMesh.vertices, p);

                        if (index == 6) { break; }
                   
                        index++;
                    }
                }

                y += 0.001f;
            }

            list.Add(new SideVertexData(registerVertices, registerIndexes));
        }

        return list;
    }

    /// <summary>円周上の頂点を取得する </summary>
    /// <param name="side">どちらの面を対象にするか</param>
    int[] GetCircumferenceVerices(Vector3[] vertices, Side side)
    {
        var containsIndexes = new List<int>();

        for (var i = 0; i < vertices.Length; i++)
        {
            var vertex = vertices[i];
            var addVertex = _sideMeshFilter.sharedMesh.vertices.OrderBy(v => Vector3.Distance(v, vertex)).First();

            if (Vector3.Distance(vertex, addVertex) < TOLERANCE)
            {
                var index = Array.IndexOf(vertices, vertex);
                containsIndexes.Add(index);

                if (side == Side.Front)
                {
                    _surfaceVertices.Add(vertex);
                }
            }
        }

        return containsIndexes.ToArray();
    }

    /// <summary>
    /// せんべいの頂点データを登録する
    /// 表面と裏面にオブジェクトが分かれているオブジェクトを登録する
    /// </summary>
    public void RegistarSenbeiObject()
    {
        if (_surfaceMeshFilter == null)
        {
            Debug.Log("表面のオブジェクトを指定してください");
            return;
        }

        if (_backMeshFilter == null)
        {
            Debug.Log("裏面のオブジェクトを指定してください");
            return;
        }

        var surfaceVertices = _surfaceMeshFilter.sharedMesh.vertices;
        var backVertices = _backMeshFilter.sharedMesh.vertices;
        var furthestVertex = surfaceVertices.OrderBy(v => Vector2.Distance(v, _surfaceMeshFilter.transform.position)).LastOrDefault();   //原点から最も遠い頂点
        var radius = new SenbeiLength((Vector2.Distance(furthestVertex, _surfaceMeshFilter.transform.position)));   //半径

        var surfaceCircumferenceVerices = GetCircumferenceVerices(surfaceVertices, Side.Front);     //円周上の頂点
        var backCircumferenceVerices = GetCircumferenceVerices(backVertices, Side.Back);

        var sideVertexList = GetSideVertexData(_surfaceVertices);   //側面

        _senbeiVertexData.AddVertexData(_registerName, radius, surfaceVertices, backVertices, surfaceCircumferenceVerices, backCircumferenceVerices, sideVertexList);
        _senbeiVertexData.SaveVertexData(_senbeiVertexData);

        _surfaceVertices.Clear();
    }
}