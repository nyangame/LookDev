using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using Unity.Burst;

/// <summary>
/// 頂点カラーをJobsystemで計算して（差し替え）マテリアルに適応するクラス
/// </summary>
[System.Serializable]
public class StructVertexColorShaderController : MonoBehaviour
{
    private static StructVertexColorShaderController _instance;// = new StructVertexColorShaderController();

    private void Awake()
    {
        _instance = new StructVertexColorShaderController();
    }

    public static StructVertexColorShaderController Instance
    {
        get
        {
            return _instance;
        }
    }

    static Mesh _surfmesh; //surface
    static Mesh _hideMesh; // hide

    /// <summary>
    /// JobSystemのジョブ
    /// </summary>
    [BurstCompile]
    public struct VertexColorChange : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Vector3> input1;
        [ReadOnly]
        public NativeArray<float> input2;
        public NativeArray<Color> result;
        [ReadOnly]
        public NativeArray<Color> inCol;

        public void Execute(int i)
        {

            //割合で色の変更（途中）
            //result[i] = input1[i] * input2[i];
            //result[i] = new Color((1 - (input1[i].y + 0.01f) * input2[i] * 0.1f), (1 - (input1[i].y + 0.01f) * input2[i] * 0.1f),
                //(1 - (input1[i].y + 0.01f) * input2[i] * 0.1f), 1);
            result[i] = new Color(
                Mathf.Clamp01((1 + input2[i]) * (1 - (1 -inCol[i].r) * (input2[i] + 0.001f))) - (input1[i].y * input2[i] * 0.01f),
                Mathf.Clamp01((1 + input2[i]) * (1 - (1 - inCol[i].g)* (input2[i] + 0.001f))) - (input1[i].y * input2[i] * 0.01f),
                Mathf.Clamp01((1 + input2[i]) * (1 - (1 - inCol[i].b) * (input2[i] + 0.001f))) - (input1[i].y * input2[i] * 0.01f),
                1);
        }
    }

    static NativeArray<Vector3> input;
    static NativeArray<float> value;
    static NativeArray<Color> resultmem;
    static NativeArray<Color> inputColor;

    /// <summary>
    /// メッシュの登録　（初期化時に使用）
    /// </summary>
    /// <param name="surfmesh">表面メッシュ</param>
    /// <param name="hidemesh">裏面メッシュ</param>
    public static void RegisterMesh(Mesh surfmesh, Mesh hidemesh)
    {
        _surfmesh = surfmesh;
        _hideMesh = hidemesh;
        InitVertexColorJob();
    }
    /// <summary>
    /// メッシュデータの初期化用関数。（メッシュ登録後に途中で初期化したい時）
    /// </summary>
    /// <param name="surfmesh">表面メッシュ</param>
    /// <param name="hidemesh">裏面メッシュ</param>
    public static void InitVertexColorJob()
    {

        float[] initdata = new float[_surfmesh.vertexCount];
        float[] initdata2 = new float[_hideMesh.vertexCount];
        Vector3[] initvec = new Vector3[_surfmesh.vertexCount];
        Vector3[] initvec2 = new Vector3[_hideMesh.vertexCount];
        Vector3 BaseColor = Vector3.zero;

        for (int i = 0; i < _surfmesh.vertexCount; i++)
        {
            initdata[i] = 0f;
        }
        for (int i = 0; i < _hideMesh.vertexCount; i++)
        {
            initdata2[i] = 0f;
        }

        UpdateJob(_surfmesh, initvec, initdata, 0, Vector3.zero);
        UpdateJob(_hideMesh, initvec2, initdata2, 0, Vector3.zero);
    }

    /// <summary>
    /// メッシュデータの入れ込み　JobSystemの使用 （StructVertexColorShaderControllerクラス専用関数）
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="valueTime"></param>
    static void UpdateJob(Mesh mesh, Vector3[] key, float[] valueTime, float Ratio, Vector3 col)
    {

        input = new NativeArray<Vector3>(mesh.vertexCount, Allocator.TempJob);
        value = new NativeArray<float>(mesh.vertexCount, Allocator.TempJob);
        resultmem = new NativeArray<Color>(mesh.vertexCount, Allocator.TempJob);
        inputColor = new NativeArray<Color>(mesh.vertexCount, Allocator.TempJob);

        Color inCol = new Color(col.x, col.y, col.z, 1);

        for (int i = 0; i < mesh.vertexCount; i++)
        {
            input[i] = key[i] * Ratio; // para
            value[i] = valueTime[i];  //para

            inputColor[i] = inCol;
        }

        VertexColorChange vertexJob = new VertexColorChange()
        {
            input1 = input,
            input2 = value,
            inCol = inputColor,
            result = resultmem
        };

        //Debug.Log("ジョブ完了");

        JobHandle handle = vertexJob.Schedule(mesh.vertexCount, 32);
        JobHandle.ScheduleBatchedJobs();
        handle.Complete();

        var resultCaps = resultmem.ToArray();

        Debug.Log(mesh.name + "_" + resultCaps[0] + "色出力された場合はジョブ完了");

        mesh.colors = resultCaps;


        input.Dispose();
        value.Dispose();
        resultmem.Dispose();
    }

    /// <summary>
    /// Shaderの更新
    /// </summary>
    /// <param name="param">渡したい構造体</param>
    /// <param name="ratio">0以上の少数。黒に近づける割合</param>
    public static void UpdateVertexColorJob(BurntParameter param, float ratio)
    {
        Vector3[] keysurf = param.SurfaceData.Keys.ToArray();
        Vector3[] keyhide = param.BackData.Keys.ToArray();

        float[] paramsurf = param.SurfaceData.Values.ToArray();
        float[] paramhide = param.BackData.Values.ToArray();

        for (int i = 0; i < keyhide.Length; i++)
        {
            keyhide[i] *= -1;
        }

        UpdateJob(_hideMesh, keyhide, paramhide, ratio, param.BakeColor);
        UpdateJob(_surfmesh, keysurf, paramsurf, ratio, param.BakeColor);

        Debug.Log(paramsurf[0] + " surfTime");
        Debug.Log(paramhide[0] + " HideTime");

        Debug.Log(keysurf[0] + " surfTime");
        Debug.Log(keyhide[0] + " hideTime");
    }
}
