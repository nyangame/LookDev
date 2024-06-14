using UnityEngine;

/// <summary>焼く処理に必要なパラメーターをもつ構造体 </summary>
public struct BakeParameter
{
    #region 焼き色処理のパラメーター
    /// <summary>焼き台の半径 </summary>
    float _bakeFieldRadius;
    /// <summary>生地の色 </summary>
    Color _doughColor;
    /// <summary>焼き色 </summary>
    Color _senbeiColor;
    /// <summary>焦げた時の色 </summary>
    Color _burntColor;
    /// <summary>焼き色になるまでの時間 </summary>
    float _senbeiBakeTime;
    /// <summary>焦げるまでの時間 </summary>
    float _burntTime;
    #endregion

    #region 膨らむ処理のパラメーター
    /// <summary>膨らむ位置数 </summary>
    int _bulgePositionCount;
    /// <summary>膨らみにかかる時間 </summary>
    float _bulgeTime;
    /// <summary>膨らむ半径の最大値 </summary>
    float _maxBulgeRadius;
    /// <summary>膨らむ半径の最小値</summary>
    float _minBulgeRadius;
    /// <summary>膨らむ高さの最大値 </summary>
    float _maxBulgeHeight;
    /// <summary>膨らむ高さの最小値 </summary>
    float _minBulgeHeight;
    #endregion

    #region 厚み処理 パラメーター
    /// <summary>せんべいの厚み </summary>
    float _profound;
    /// <summary>側面の湾曲係数 </summary>
    float _sideCoefficient; 

    #endregion

    #region プロパティ
    /// <summary>焼き台の半径 </summary>
    public float BakeFieldRadius { get => _bakeFieldRadius; }

    /// <summary>生地の色 </summary>
    public Color DoughColor { get => _doughColor; }

    /// <summary>焼き色 </summary>
    public Color SenbeiColor { get => _senbeiColor; }

    /// <summary>焦げた時の色 </summary>
    public Color BurntColor { get => _burntColor; }

    /// <summary>焼き色になるまでの時間 </summary>
    public float SenbeBakeTime { get => _senbeiBakeTime; }

    /// <summary>焦げるまでの時間 </summary>
    public float BurntTime { get => _burntTime; }

    /// <summary>膨らむ位置数 </summary>
    public int BulgePositionCount { get => _bulgePositionCount; }

    /// <summary>膨らみにかかる時間 </summary>
    public float BulgeTime { get => _bulgeTime; }

    /// <summary>膨らむ半径の最大値 </summary>
    public float MaxBulgeRadius { get => _maxBulgeRadius; }
    /// <summary>膨らむ半径の最小値 </summary>
    public float MinBulgeRadius { get => _minBulgeRadius; }

    /// <summary>膨らむ高さの最大値 </summary>
    public float MaxBulgeHeight { get => _maxBulgeHeight; }

    /// <summary>膨らむ高さの最小値 </summary>
    public float MinBulgeHeight { get => _minBulgeHeight; }

    /// <summary>せんべいの厚み </summary>
    public float Profound { get => _profound; }

    /// <summary>側面の湾曲係数 </summary>
    public float SideCoefficient { get => _sideCoefficient; }

    #endregion

    public BakeParameter(float bakeFieldRadius, Color dough, Color senbei, Color burnt, float senbeiBakeTime, float burntTime, int positionCount, float bulgeTime, float maxBulgeRadius, float minBulgeRadius, float maxBulegHeight, float minBulegHeight, float profound, float coefficient)
    {
        _bakeFieldRadius = bakeFieldRadius;
        _doughColor = dough;
        _senbeiColor = senbei;
        _burntColor = burnt;
        _senbeiBakeTime = senbeiBakeTime;
        _burntTime = burntTime;
        _bulgePositionCount = positionCount;
        _bulgeTime = bulgeTime;
        _maxBulgeRadius = maxBulgeRadius;
        _minBulgeRadius = minBulgeRadius;
        _maxBulgeHeight = maxBulegHeight;
        _minBulgeHeight = minBulegHeight;
        _profound = profound;
        _sideCoefficient = coefficient;
    }
}
