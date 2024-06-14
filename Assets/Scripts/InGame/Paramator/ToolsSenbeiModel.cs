using System;
using UnityEngine;

/// <summary>
/// 道具に必要な情報を渡すためのデータクラス
/// </summary>
public class ToolsSenbeiModel
{
    /// <summary>七輪のポジション</summary>
    public readonly Transform Transform;

    /// <summary>せんべいの半径</summary>
    public readonly SenbeiLength Radius;

    public ToolsSenbeiModel(Transform transform,SenbeiLength senbeiLength)
    {
        if (transform == null)
        {
            throw new ArgumentNullException($"ToolsSenbeiModelに渡ってきた値がnullです{typeof(Transform)}");
        }
        Transform = transform;
        this.Radius = senbeiLength;
    }
}
