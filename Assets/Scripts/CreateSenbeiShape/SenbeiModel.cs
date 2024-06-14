using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CreateSenbei
{
    /// <summary>せんべいデータクラス </summary>
    public class SenbeiModel
    {
        #region 変数
        [FormerlySerializedAs("currentBakeSide")]
        [FormerlySerializedAs("_currentBakeSurface")]
        /// <summary>現在焼かれている面 </summary>
        Side _currentBakeSide = Side.Back;
        /// <summary>焼きに必要なパラメーター </summary>
        BakeParameter _bakeParameter = default;
        /// <summary>焦げ処理時に使用するパラメーター </summary>
        BurntParameter _burntParameter = default;
        /// <summary>現在焼かれているせんべいの頂点データ</summary>
        VertexData _currentVertexData = default;
        /// <summary>焼き台の中心 </summary>
        Vector3 _bakeFieldCenter = Vector3.zero;
        bool _burntStart = false;
        bool _isSetBulge = false;
        bool _isStopBaking = false;
        float _bulegeElapsedTime = 0f;
        float _bakeElapsedTime = 0f;
        /// <summary>膨らみ倍率 </summary>
        float _bulegeMag = 0f;
        /// <summary>押し瓦使用の膨らみ倍率 </summary>
        float _oshigawaraMag = 0.01f;
        /// <summary>表面 各頂点の焼き時間</summary>
        float[] _surfaceVertexBakeTimes = default;
        /// <summary>裏面 各頂点の焼き時間</summary>
        float[] _backVertexBakeTimes = default;

        private Vector3[] _frontVertices;
        private Vector3[] _backVertices;
        private Vector3[] _sideVertices;

        SenbeiLength _radius;
        /// <summary>
        /// 膨らむ頂点データをもつ 
        /// <para>key:表面</para>
        /// <para>Value:裏面</para>
        /// </summary>
        Dictionary<BulgeData, BulgeData> _bulegeData = new Dictionary<BulgeData, BulgeData>();
        #endregion

        #region プロパティ
        /// <summary>現在焼かれている面 </summary>
        public Side CurrentBakeSide { get => _currentBakeSide; set => _currentBakeSide = value; }
        /// <summary>焼きに必要なパラメーター </summary>
        public BakeParameter BakeParameter { get => _bakeParameter; set => _bakeParameter = value; }
        /// <summary>焦げ処理時に使用するパラメーター </summary>
        public BurntParameter BurntParameter { get => _burntParameter; set => _burntParameter = value; }
        /// <summary>現在焼かれているせんべいの頂点データ</summary>
        public VertexData CurrentVertexData { get => _currentVertexData; set => _currentVertexData = value; }
        /// <summary>焼き台の中心 </summary>
        public Vector3 BakeFieldCenter { get => _bakeFieldCenter; set => _bakeFieldCenter = value; }
        public bool IsStopBaking { get => _isStopBaking; set => _isStopBaking = value; }
        public bool BurntStart { get => _burntStart; set => _burntStart = value; }
        public bool IsSetBulge { get => _isSetBulge; set => _isSetBulge = value; }
        public float BulegeElapsedTime { get => _bulegeElapsedTime; set => _bulegeElapsedTime = value; }
        public float BakeElapsedTime { get => _bakeElapsedTime; set => _bakeElapsedTime = value; }
        /// <summary>膨らみ倍率 </summary>
        public float BulegeMag { get => _bulegeMag; set => _bulegeMag = value; }
        /// <summary>押し瓦使用の膨らみ倍率 </summary>
        public float OshigawaraMag { get => _oshigawaraMag; set => _oshigawaraMag = value; }
        /// <summary>表面 各頂点の焼き時間</summary>
        public float[] SurfaceVertexBakeTimes { get => _surfaceVertexBakeTimes; set => _surfaceVertexBakeTimes = value; }
        /// <summary>裏面 各頂点の焼き時間</summary>
        public float[] BackVertexBakeTimes { get => _backVertexBakeTimes; set => _backVertexBakeTimes = value; }
        /// <summary>
        /// 膨らむ頂点データをもつ 
        /// <para>key:表面</para>
        /// <para>Value:裏面</para>
        /// </summary>
        public Dictionary<BulgeData, BulgeData> BulegeData { get => _bulegeData; set => _bulegeData = value; }
        
        public Vector3[] FrontVertices { get => _frontVertices; set => _frontVertices = value; }
        public Vector3[] BackVertices { get => _backVertices; set => _backVertices = value; }
        public Vector3[] SideVertices { get => _sideVertices; set => _sideVertices = value; }
        public SenbeiLength Radius { get => _radius; set => _radius = value; }
      
        #endregion

        public SenbeiModel(VertexData senbeiData, Vector3 bakeField, BakeParameter bakeParameter)
        {
            BakeFieldCenter = bakeField;
            CurrentVertexData = senbeiData;
            BakeParameter = bakeParameter;

            SurfaceVertexBakeTimes = new float[senbeiData.SurfaceVertices.Length];
            BackVertexBakeTimes = new float[senbeiData.BackVertices.Length];
        }
    }
}