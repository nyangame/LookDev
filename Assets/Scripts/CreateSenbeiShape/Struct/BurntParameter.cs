using System.Collections.Generic;
using UnityEngine;

/// <summary>焦げ処理に使用する構造体</summary>
public struct BurntParameter
{
    /// <summary>
    /// 焼き色のVector3
    /// x=R, y=G, z=Bで代入しています
    /// </summary>
    Vector3 _bakeColor;

    /// <summary>
    /// 表面データ
    /// <para>Key:各頂点</para>
    /// <para>Value:焼き時間(網に接触している時間)</para>
    /// </summary>
    Dictionary<Vector3, float> _surfaceData;

    /// <summary>
    /// 裏面データ
    /// <para>Key:各頂点</para>
    /// <para>Value:焼き時間(網に接触している時間)</para>
    /// </summary>
    Dictionary<Vector3, float> _backData;

    #region プロパティ
    /// <summary>
    /// 表面データ
    /// <para>Key:各頂点のy値</para>
    /// <para>Value:焼き時間(網に接触している時間)</para>
    /// </summary>
    public Dictionary<Vector3, float> BackData { get => _backData; }

    /// <summary>
    /// 裏面データ
    /// <para>Key:各頂点のy値</para>
    /// <para>Value:焼き時間(網に接触している時間)</para>
    /// </summary>
    public Dictionary<Vector3, float> SurfaceData { get => _surfaceData; }

    /// <summary>
    /// 焼き色のVector3
    /// x=R, y=G, z=Bで代入しています
    /// </summary>
    public Vector3 BakeColor { get => _bakeColor; }
    #endregion

    public BurntParameter(Dictionary<Vector3, float> surface, Dictionary<Vector3, float> back, Color bakeColor)
    {
        _bakeColor = new Vector3(bakeColor.r, bakeColor.g, bakeColor.b);
        _surfaceData = surface;
        _backData = back;
    }
}
