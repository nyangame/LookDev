using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>パーリンノイズで膨らみ・反りを作成するクラス</summary>
public class PerlinNoise : MonoBehaviour
{
    //[SerializeField]
    //SenbeiModel[] _senbeis = default;
    //[SerializeField, Header("ノイズの大きさ"), Range(0.01f, 0.9f)]
    //float _noiseScale = 0f;
    //[SerializeField, Header("高さ")]
    //float _height = 0f;
    //[SerializeField, Tooltip("rayを飛ばし頂点を取得するクラス")]
    //GetMeshVertex _getmeshVertex = default;

    //IEnumerator Start()
    //{
    //    yield return null;
    //    yield return null;
      
    //    foreach (var senbei in _senbeis)
    //    {
    //        senbei.TargetVertices = AllVertexPerlinNoise(senbei.SharedMeshVerticesLength);
    //        senbei.SurfacePerliNoiseList = CreateSurfaceUsingPerlinNoise();
    //        senbei.SurfaceVertexIndexList = _getmeshVertex.RayHitSurfaceVertexIndexList;
    //        senbei.IsMove = true;
    //    }
    //}

    ///// <summary>全ての頂点に対してパーリンノイズをかける </summary>
    ///// <returns>膨らみ・反りの頂点</returns>
    //Vector3[] AllVertexPerlinNoise(int length)
    //{
    //    var vertices = new Vector3[length];

    //    for (var i = 0; i < length; i++)
    //    {
    //        for (var j = 0; j < length; j++)
    //        {
    //            var v = CreatePerliNoise(i, j);
    //            vertices[i] = v;
    //        }
    //    }

    //    return vertices;
    //}

    ///// <summary>パーリンノイズを使用しせんべい表面を作成する</summary>
    ///// <returns></returns>
    //List<Vector3> CreateSurfaceUsingPerlinNoise()
    //{
    //    var vertices = new List<Vector3>();

    //    var z = 0;

    //    for (var i = 0; i < _getmeshVertex.RayHitSurfaceVertices.Count; i++)
    //    {
    //        var v = CreatePerliNoise(i, z);
    //        vertices.Add(v);

    //        z++;

    //        if (100 < z)
    //        {
    //            z = 0;
    //        }
    //    }

    //    return vertices;
    //}

    ///// <summary>パーリンノイズの値を取得し、位置を返す </summary>
    //Vector3 CreatePerliNoise(int x, int z)
    //{
    //    var pos = Vector3.zero;

    //    float xValue = x * _noiseScale;
    //    float yValue = z * _noiseScale;

    //    float perlinValue = Mathf.PerlinNoise(xValue, yValue);

    //    float height = _height * perlinValue;

    //    pos = new Vector3(x, height, z);

    //    return pos;
    //}
}
