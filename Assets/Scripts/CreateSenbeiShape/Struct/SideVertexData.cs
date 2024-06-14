using UnityEngine;

/// <summary>側面の頂点を保持する構造体 </summary>
[System.Serializable]
public struct SideVertexData
{
    [SerializeField, Tooltip("側面の頂点配列(一列分)")]
    Vector3[] _lineVertices;

    [SerializeField, Tooltip("lineVerticesの添え字配列")]
    int[] _vertexIndexes;

    #region プロパティ
    /// <summary>側面の頂点配列 </summary>
    public Vector3[] LineVertices { get => _lineVertices; }

    /// <summary>lineVerticesの添え字配列 </summary>
    public int[] VertexIndexes { get => _vertexIndexes; }
    #endregion

    public SideVertexData(Vector3[] vertices, int[] Indexes)
    {
        _lineVertices = vertices;
        _vertexIndexes = Indexes;
    }
}

