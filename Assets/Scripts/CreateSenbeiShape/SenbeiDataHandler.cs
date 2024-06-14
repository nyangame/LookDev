using UnityEngine;

/// <summary>せんべいの種のデータを扱う為のクラス/summary>
public class SenbeiDataHandler : MonoBehaviour
{
    [SerializeField, Header("各せんべいの頂点データ")]
    SenbeiVertexData _senbeiData = default;

    /// <summary>
    /// 頂点データが既に登録されているか調べ、登録されていればデータを返します
    /// <para>データが登録されていなければnullを返します</para>
    /// </summary>
    /// <param name="registerName">調べたいオブジェクトの登録名</param>
    /// <returns>登録なし=null </returns>
    public VertexData? GetVertexData(string registerName)
    {
       return _senbeiData.GetVertexData(registerName);
    }

    /// <summary>データが登録されているか調べる </summary>
    /// <param name="registerName">調べたい名前</param>
    /// <returns>true=存在　false=存在しない</returns>
    public bool ChackExist(string registerName)
    {
        return _senbeiData.ChackExist(registerName);
    }
}
