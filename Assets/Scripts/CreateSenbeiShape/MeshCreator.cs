using UnityEditor;
using UnityEngine;

/// <summary>メッシュを作成するクラス</summary>
public class MeshCreator : MonoBehaviour
{
    const string SAVE_PATH = "Assets/PerlinNoise/Meshes/";

    [SerializeField, Header("作成したいMeshFilterをアタッチ")]
    MeshFilter _target = default;

    [SerializeField, Header("作成したメッシュの名前を変更したければ設定してください")]
    string _meshName = default;


    [ContextMenu("CreateMesh")]
    public void MeshCreate()
    {
#if UNITY_EDITOR
        if(_target == null)
        {
            Debug.LogWarning("Targetを設定してください");
            return;
        }

        var meshName = _meshName;

        if (meshName == "")
        {
            meshName= _target.name;
        }
      
        var savePath = SAVE_PATH + meshName + ".asset";
        AssetDatabase.CreateAsset(_target.mesh, savePath);
        AssetDatabase.SaveAssets();

        Debug.Log("Meshを作成しました。");
#endif
    }
}
