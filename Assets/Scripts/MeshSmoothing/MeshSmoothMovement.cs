using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshFilter))]
public class MeshSmoothMovement : MonoBehaviour
{
    [System.Serializable]
    enum FilterType
    {
        Laplacian, 
        HC
    };
    MeshFilter _filter;
    MeshFilter filter
    {
        get
        {
            if (_filter == null)
            {
                _filter = GetComponent<MeshFilter>();
            }
            return _filter;
        }
    }
    [SerializeField] Mesh basemesh = null;
    [Space]
    [SerializeField, Range(0f, 1f)] float intensity = 0.5f;
    [SerializeField] FilterType type;
    [SerializeField, Range(0, 20)] int times = 3;
    [SerializeField, Range(0f, 1f)] float hcAlpha = 0.5f;
    [SerializeField, Range(0f, 1f)] float hcBeta = 0.5f;

    void Start()
    {
        SmoothMeshFilter();
    }

    public void SmoothMeshFilter()
    {
        var mesh = filter.mesh;
        filter.mesh = ApplyNormalNoise(mesh);

        switch (type)
        {
            case FilterType.Laplacian:
                filter.mesh = MeshSmoothing.LaplacianFilter(filter.mesh, times);
                break;
            case FilterType.HC:
                filter.mesh = MeshSmoothing.HCFilter(filter.mesh, times, hcAlpha, hcBeta);
                break;
        }
    }

    public void ResetMesh()
    {
        if (basemesh == null)
        {
            Debug.LogError("ベースメッシュが設定されていません");
            return;
        }
        _filter.mesh = basemesh;
    }

    Mesh ApplyNormalNoise(Mesh mesh)
    {

        var vertices = mesh.vertices;
        var normals = mesh.normals;
        for (int i = 0, n = mesh.vertexCount; i < n; i++)
        {
            vertices[i] = vertices[i] + normals[i] * Random.value * intensity;
        }
        mesh.vertices = vertices;

        return mesh;
    }
}
