using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using CreateSenbei;
using System.Linq;

public class ProfoundTest : MonoBehaviour
{
    /// <summary>円周上の頂点を取得時に使用 </summary>
    const float TOLERANCE = 0.001f;

    [SerializeField]
    string _type;
    [SerializeField]
    MeshFilter _surface;
    [SerializeField]
    MeshFilter _side;
    [SerializeField]
    MeshFilter _back;
    [SerializeField]
    SenbeiDataHandler _handler;

    VertexData _vertexData;
    int _lineCount = 0;
    int _vertexCount = 0;

    void Start()
    {
        _vertexData = (VertexData)_handler.GetVertexData(_type);

        _side.sharedMesh = MeshUtility.CopyMesh(_side.sharedMesh);
        _surface.sharedMesh = MeshUtility.CopyMesh(_surface.sharedMesh);
        _back.sharedMesh = MeshUtility.CopyMesh(_back.sharedMesh);

        var vertices = _side.sharedMesh.vertices;

        var surface = new Vector3[_vertexData.SurfaceCircumIndexes.Length];
        var back = new Vector3[_vertexData.SurfaceCircumIndexes.Length];

        for (var i = 0; i < surface.Length; i++)
        {
            surface[i] = _surface.sharedMesh.vertices[_vertexData.SurfaceCircumIndexes[i]];
            back[i] = _back.sharedMesh.vertices[_vertexData.BackCircumIndexes[i]];
        }

        for (var i = 0; i < _vertexData.SideVertexList.Count; i++)
        {
            var lineVertex = _vertexData.SideVertexList[i].LineVertices;
         
            for (var k = 0; k < lineVertex.Length; k++)
            {
                var vertex = lineVertex[k];
                var index = Array.IndexOf(_side.sharedMesh.vertices, vertex);
                var nomal = (new Vector3(vertex.x, 0, vertex.z) - new Vector3(_side.transform.position.x, 0, _side.transform.position.z)).normalized;

                var lastPoint = vertex + nomal * Mathf.Cos(Mathf.Abs(3 - k) / 4f * Mathf.PI) * 0.05f;

                vertices[index] = lastPoint;
            }
        }

        _side.mesh.vertices = vertices;

        _side.mesh.RecalculateBounds();     //境界箱
        _side.mesh.RecalculateNormals();    //法線
        _side.mesh.RecalculateTangents();   //接線

    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        var lineVertex = _vertexData.SideVertexList[_lineCount].LineVertices;
    //        var vertex = lineVertex[_vertexCount];
    //        var end = new Vector3(vertex.x * 1.3f, vertex.y, vertex.z * 1.3f);

    //        Debug.DrawLine(vertex, end, Color.green, 10f);

    //        _vertexCount++;

    //        if (_vertexCount == 7)
    //        {
    //            _vertexCount = 0;
    //            _lineCount++;
    //        }

    //        Debug.Log("hit");
    //    }

    //    if (Input.GetMouseButtonDown(2))
    //    {
    //        var lineVertex = _vertexData.SideVertexList[_lineCount].LineVertices;

    //        for (var k = 0; k < lineVertex.Length; k++)
    //        {
    //            var vertex = lineVertex[k];
    //            var end = new Vector3(vertex.x * 1.3f, vertex.y, vertex.z * 1.3f);
    //            Debug.DrawLine(vertex, end, Color.green, 10f);
    //            Debug.Log("hit");
    //        }
    //    }
    //}
}
