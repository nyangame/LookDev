using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// せんべいの種の頂点データを保存するスクリプタブルオブジェクト 
/// <para><see cref="SenbeiDataHandler"/>で使用します</para>
/// </summary>
[CreateAssetMenu]
public class SenbeiVertexData : ScriptableObject
{
    /// <summary> 各せんべいの種の頂点配列をもつ構造体のリスト  </summary>
    [SerializeField, Header("登録されている頂点データのリスト")]
    List<VertexData> _vertexDataList = default;

    /// <summary>
    /// 取得した頂点データを保存する 
    /// <para><see cref="SaveVertexData"/>と一緒に使用してください</para>
    /// </summary>
    /// <param name="registerName">登録名</param>
    /// <param name="sufaceVertices">表面の頂点配列</param>
    /// <param name="backVertices">裏面の頂点配列</param>
    public void AddVertexData(string registerName, SenbeiLength radius, Vector3[] sufaceVertices, Vector3[] backVertices, int[] surfaceCircumference, int[] backCircumference, List<SideVertexData> sideList)
    {
        var data = new VertexData(registerName, radius, sufaceVertices, backVertices, surfaceCircumference, backCircumference, sideList);

        var lastData = GetVertexData(registerName);

        if (lastData != null)
        {
            _vertexDataList.Remove((VertexData)lastData);
        }

        _vertexDataList.Add(data);

        Debug.Log("頂点データを登録しました。");
    }

    /// <summary>リストに追加されているデータを削除する </summary>
    /// <param name="registerName">削除したいデータ名</param>
    public void RemoveVertexData(string registerName)
    {
        var data = GetVertexData(registerName);

        if (data == null)
        {
            Debug.LogError("指定された名前で登録されているデータは存在しません");
            return;
        }

        _vertexDataList.Remove((VertexData)data);
        Debug.Log("データを削除しました");
    }

    /// <summary>リストに追加されている全てのデータを削除する </summary>
    public void RemoveAllVertexData()
    {
        _vertexDataList.Clear();
        Debug.Log("全ての頂点データを削除しました。");
    }

    /// <summary>
    /// 頂点データが既に登録されているか調べ、登録されていればデータを返します
    /// <para>データが登録されていなければnullを返します</para>
    /// </summary>
    /// <param name="registerName">調べたいオブジェクトの登録名</param>
    /// <returns>登録なし=null </returns>
    public VertexData? GetVertexData(string registerName)
    {
        foreach (var data in _vertexDataList)
        {
            if (data.ObjectName == registerName)
            {
                return data;
            }
        }

        return null;
    }

    /// <summary>データが登録されているか調べる </summary>
    /// <param name="registerName">調べたい名前</param>
    public bool ChackExist(string registerName)
    {
        foreach (var data in _vertexDataList)
        {
            if (data.ObjectName == registerName)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>スクリプタブルオブジェクトの変更を保存する </summary>
    /// <param name="vertexData">保存データ</param>
    public void SaveVertexData(SenbeiVertexData vertexData)
    {
        EditorUtility.SetDirty(vertexData);
        AssetDatabase.SaveAssets();

        Debug.Log("スクリプタブルの変更を保存しました。");
    }
}