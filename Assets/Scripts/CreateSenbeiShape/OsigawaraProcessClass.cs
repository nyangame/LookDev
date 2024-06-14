using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

/// <summary>押し瓦の処理を行うクラス </summary>
public class OsigawaraProcessClass : MonoBehaviour
{
    /// <summary>円の角度 </summary>
    const float CIRCLE_ANGLE = 360f;

    [SerializeField, Header("押し瓦の円を表示")]
    bool _isView = false;
    [SerializeField, Header("円周上のray数"), Min(10)]
    int _circleRayCount = 360;
    [SerializeField, Header("押し瓦の半径"), Min(0.1f)]
    float _radius = 0.3f;
    [SerializeField, Header("押し瓦使用時のy変化量")]
    float _usePower = 0.2f;
    [SerializeField, Header("膨らませるまでの待機時間")]
    float _delayBakeTime = 0.3f;
    [SerializeField, Header("ラインの表示時間")]
    float _lineDuration = 0.2f;
    /// <summary>現在焼かれているせんべい </summary>
    CreateSenbei.SenbeiController _currentBakeSenbei = default;
    [SerializeField, Tooltip("rayを飛ばす為に使用")]
    Camera _cam = default;

    /// <summary>現在焼かれているせんべいの頂点データ </summary>
    VertexData _currentVertexData = default;

    /// <summary>一定時間焼く処理を止める </summary>
    public async UniTask WaitBakeStart()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_delayBakeTime));
        _currentBakeSenbei.IsStopBaking = false;
    }

    /// <summary>取得した円内の頂点を動かす </summary>
    void VertexWithinCircleMove(List<int> indexes)
    {
        _currentBakeSenbei.ChangeBulgeMag(_usePower, indexes);
        indexes.Clear();
    }

    /// <summary>
    /// 押し瓦使用時の処理を行う
    /// 押し瓦使用時に呼んでください
    /// </summary>
    /// <param name="osigawaraCenter">押し瓦の中心</param>
    /// <param name="radius">押し瓦の半径</param>
    public void OsigawaraProcess(Vector3 osigawaraCenter)
    {
        var getVerticesIndex = new List<int>();
        var angle = CIRCLE_ANGLE / _circleRayCount;

        if (_isView)
        {
            Debug.DrawLine(osigawaraCenter, new Vector3(osigawaraCenter.x, osigawaraCenter.y + 0.5f, osigawaraCenter.z), Color.blue, _lineDuration);  //押し瓦の中心を描画

            for (var i = 0; i <= _circleRayCount; i++) //円周を示す為のLine位置を計算
            {
                var pos = angle * i * Mathf.Deg2Rad;

                float x = _radius * Mathf.Cos(pos) + osigawaraCenter.x;
                float z = _radius * Mathf.Sin(pos) + osigawaraCenter.z;
                float y = 5;
                var rayPos = new Vector3(x, y, z);

                Debug.DrawLine(rayPos, new Vector3(x, -y, z), Color.blue, _lineDuration);
            }
        }

        for (var i = 0; i < _currentVertexData.SurfaceVertices.Length; i++)     //円内に含まれるか判別
        {
            var vertex = _currentVertexData.SurfaceVertices[i];
            var length = Mathf.Pow(vertex.x - osigawaraCenter.x, 2) + Mathf.Pow(vertex.z - osigawaraCenter.z, 2);

            if (length <= Mathf.Pow(_radius, 2))    //頂点を取得
            {
                getVerticesIndex.Add(i);
            }
        }

        VertexWithinCircleMove(getVerticesIndex);
    }

    /// <summary>
    /// 焼かれるせんべいを取得する 
    /// <para>新しくせんべいを焼く場合はこの関数を呼んでください</para>
    /// </summary>
    /// <param name="senbeiVertexData">焼くせんべい頂点データ</param>
    /// <param name="bakeSenbei">焼かれるせんべいのオブジェクト</param>
    public void SetBakeSenbei(VertexData senbeiVertexData, CreateSenbei. SenbeiController bakeSenbei)
    {
        _currentVertexData = senbeiVertexData;
        _currentBakeSenbei = bakeSenbei;
    }
}
