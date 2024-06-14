using UnityEngine;

/// <summary>せんべいの膨らみデータ </summary>
public struct BulgeData
{
    /// <summary>円の中心</summary>
    Vector3 _center;
    /// <summary>半径 </summary>
    SenbeiLength _senbeiLength;

    /// <summary>膨らませる頂点の添え字配列</summary>
    int[] _vertexIndexes;

    /// <summary>膨らんだ時の頂点のy位置(高さ) </summary>
    float[] _bulgeHeghts;

    #region プロパティ
    /// <summary>円の中心</summary>
    public Vector3 Center { get => _center; }

    /// <summary>半径 </summary>
    public SenbeiLength SenbeiLength { get => _senbeiLength; }

    /// <summary>膨らませる頂点の添え字配列</summary>
    public int[] VertexIndexes { get => _vertexIndexes; }

    /// <summary>膨らんだ時の頂点のy位置(高さ) </summary>
    public float[] BulgeHeghts { get => _bulgeHeghts; }
    #endregion

    public BulgeData(Vector3 center, SenbeiLength senbeiLength, int[] vertexIndexes, float[] bulgeHeghts)
    {
        _center = center;
        _senbeiLength = senbeiLength;
        _vertexIndexes = vertexIndexes;
        _bulgeHeghts = bulgeHeghts;
    }
}