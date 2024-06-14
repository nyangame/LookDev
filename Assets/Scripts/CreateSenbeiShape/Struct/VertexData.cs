using UnityEngine;
using System.Collections.Generic;

/// <summary>頂点配列を保持する構造体 </summary>
[System.Serializable]
public struct VertexData
{
    /// <summary>取得した頂点データをもつオブジェクトの名前 </summary>
    [SerializeField, Header("登録名")]
    string _objectName;

    [SerializeField, Header("半径")]
    float _senbeiLengthValue;

    /// <summary>表面の頂点配列</summary>
    [SerializeField, Header("表面の頂点配列")]
    Vector3[] _surfaceVertices;

    /// <summary>裏面の頂点配列</summary>
    [SerializeField, Header("裏面の頂点配列")]
    Vector3[] _backVertices;

    [SerializeField, Header("円周を構成する表面の頂点の添え字")]
    int[] _surfaceCircumferenceIndexes;

    [SerializeField, Header("円周を構成する裏面の頂点の添え字")]
    int[] _backCircumferenceIndexes;

    [SerializeField, Header("側面の頂点リスト")]
    List<SideVertexData> _sideList;

    private SenbeiLength _senbeiLength;

    #region プロパティ
    /// <summary>取得した頂点データをもつオブジェクトの名前 </summary>
    public string ObjectName { get => _objectName; }

    /// <summary>半径 </summary>
    public SenbeiLength SenbeiLength
    {
        get
        {
            if (_senbeiLength == null)
            {
                _senbeiLength = new SenbeiLength(_senbeiLengthValue);
            }
            return _senbeiLength;
        }
    }

    /// <summary>表面の頂点配列</summary>
    public Vector3[] SurfaceVertices { get => _surfaceVertices; }

    /// <summary>裏面の頂点配列</summary>
    public Vector3[] BackVertices { get => _backVertices; }

    /// <summary>円周上の頂点(表面) </summary>
    public int[] SurfaceCircumIndexes { get => _surfaceCircumferenceIndexes; }

    /// <summary>円周上の頂点(裏面) </summary>
    public int[] BackCircumIndexes { get => _backCircumferenceIndexes; }

    /// <summary>側面の頂点リスト </summary>
    public List<SideVertexData> SideVertexList { get => _sideList; }
    #endregion

    public VertexData(string objectName, SenbeiLength senbeiLength, Vector3[] surfaceVertices, Vector3[] backVertices, int[] surfaceCircumference, int[] backCircumference, List<SideVertexData> sideList)
    {
        _objectName = objectName;
        _senbeiLength = senbeiLength;
        _surfaceVertices = surfaceVertices;
        _backVertices = backVertices;
        _surfaceCircumferenceIndexes = surfaceCircumference;
        _backCircumferenceIndexes = backCircumference;
        _senbeiLengthValue = senbeiLength.Value;
        _sideList = sideList;
    }
}

