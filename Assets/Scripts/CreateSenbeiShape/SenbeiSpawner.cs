using UnityEngine;
using UnityEngine.Serialization;

namespace CreateSenbei
{
    /// <summary>焼くせんべいを生成するクラス </summary>
    public class SenbeiSpawner : MonoBehaviour
    {
        #region SerializeField
        [SerializeField]
        SenbeiType _senbeiType = SenbeiType.Circle;
        [SerializeField, Header("焼き台")]
        Transform _bakeField = default;
        [SerializeField, Header("せんべい種の頂点データ"), Tooltip("Sceneに配置されているものをアタッチ")]
        SenbeiDataHandler _senbeiDataHandler = default;
        [ElementNames(new string[] { "Circle", "Heart", "Square", "Star", "Triangle" })]
        [FormerlySerializedAs("_senbeiModel")] [SerializeField, Header("せんべいプレハブ")]
        SenbeiController[] _senbeiControllers = default;
        [SerializeField, Header("押し瓦処理を行うクラス")]
        OsigawaraProcessClass _osigawaraProcess = default;
        [Header("=============焼き関連関連=============")]
        [SerializeField, Header("生地の色")]
        Color _doughColor = default;
        [SerializeField, Header("焼き色")]
        Color _senbeiColor = default;
        [SerializeField, Header("焦げた時の色")]
        Color _burntColor = default;
        [SerializeField, Header("焼き色になるまでの時間")]
        float _senbeiBakeTime = 0f;
        [SerializeField, Header("焦げるまでの時間")]
        float _burntTime = 0f;
        [SerializeField, Header("焼き台の半径")]
        float _bakeFieldRadius = 1.5f;
        [Header("==============膨らみ関連================")]
        [SerializeField, Header("膨らむ位置数")]
        int _bulgePositionCount = 0;
        [SerializeField, Header("膨らみにかかる時間")]
        float _bulgeTime = 1f;
        [SerializeField, Tooltip("膨らむ半径の最大値"), Min(0.01f)]
        float _maxBulgeRadius = 0.15f;
        [SerializeField, Tooltip("膨らむ半径の最小値"), Min(0.01f)]
        float _minBulgeRadius = 0.05f;
        [SerializeField, Tooltip("膨らむ高さの最大値"), Min(0.01f)]
        float _maxBulgeHeight = 0.5f;
        [SerializeField, Tooltip("膨らむ高さの最小値"), Min(0.01f)]
        float _minBulgeHeight = 0.2f;
        [Header("==============厚み関連================")]
        [SerializeField, Tooltip("厚みの最大値")]
        float _maxProfound = 2f;
        [SerializeField, Tooltip("厚みの最小値")]
        float _minProfound = 0.5f;
        [SerializeField, Tooltip("側面の最大係数")]
        float _maxSideCoefficient = 0.03f;
        [SerializeField, Tooltip("側面の最小係数")]
        float _minSideCoefficient = 0.03f;
        #endregion

        /// <summary>焼かれているせんべい </summary>
        SenbeiController _currentBakeSenbei = default;

        public SenbeiController CurrentBakeSenbei {
            get
            {
                _currentBakeSenbei.SetCurrentVertex();
                return _currentBakeSenbei;
            } 
            set => _currentBakeSenbei = value; }

        void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (_currentBakeSenbei)
                {
                    return;
                }
                SpawnSenbei(_senbeiType.ToString());
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                if (_currentBakeSenbei)
                {
                    Destroy(_currentBakeSenbei.gameObject);
                }
            }
        }

        public void SetResumeSenbeiData(SenbeiModel senbeiModel)
        {
            var bakePos = new Vector3(_bakeField.position.x, _bakeField.position.y + 0.05f, _bakeField.position.z);
            _currentBakeSenbei = Instantiate(_senbeiControllers[(int)_senbeiType], bakePos, Quaternion.identity);
            _currentBakeSenbei.ResumeSenbei(senbeiModel);

            //焦げ処理の為にメッシュを登録
            Mesh surfaceMesh = _currentBakeSenbei.SurfaceMeshFilter.mesh;
            Mesh backMesh = _currentBakeSenbei.BackMeshFilter.mesh;    
            StructVertexColorShaderController.RegisterMesh(surfaceMesh, backMesh);  

            _osigawaraProcess.SetBakeSenbei(senbeiModel.CurrentVertexData, _currentBakeSenbei); 
        }
        
        /// <summary>せんべいを生成する </summary>
        /// <param name="vertexDataName">頂点データの登録名</param>
        void SpawnSenbei(string vertexDataName)
        {
            var bakePos = new Vector3(_bakeField.position.x, _bakeField.position.y + 0.05f, _bakeField.position.z);
            var bakeParameter = CreateBakeParameter();
            var bakeVertexData = GetBakeVertexData(vertexDataName);

            _currentBakeSenbei = Instantiate(_senbeiControllers[(int)_senbeiType], bakePos, Quaternion.identity);
            SenbeiModel senbeiModel = new SenbeiModel(bakeVertexData, _bakeField.position, bakeParameter);
            senbeiModel.SurfaceVertexBakeTimes = new float[bakeVertexData.SurfaceVertices.Length];
            senbeiModel.BackVertexBakeTimes = new float[bakeVertexData.BackVertices.Length];
            _currentBakeSenbei.InitializeSenbei(senbeiModel);

            //焦げ処理の為にメッシュを登録
            var surfaceMesh = _currentBakeSenbei.transform.GetChild(2).GetComponent<MeshFilter>().mesh;
            var backMesh = _currentBakeSenbei.transform.GetChild(0).GetComponent<MeshFilter>().mesh;    
            StructVertexColorShaderController.RegisterMesh(surfaceMesh, backMesh);  

            _osigawaraProcess.SetBakeSenbei(bakeVertexData, _currentBakeSenbei);    //押し瓦の設定を行う
        }

        /// <summary>焼きに必要なパラメーターを作成する </summary>
        /// <returns>焼きパラメーター</returns>
        BakeParameter CreateBakeParameter()
        {
            var profound = Random.Range(_minProfound, _maxProfound);
            var coefficient = Random.Range(_minSideCoefficient, _maxSideCoefficient);
            var bakeParameter = new BakeParameter(_bakeFieldRadius,_doughColor, _senbeiColor, _burntColor, _senbeiBakeTime, _burntTime, _bulgePositionCount, _bulgeTime, _maxBulgeRadius, _minBulgeRadius, _maxBulgeHeight, _minBulgeHeight, profound, coefficient);
            return bakeParameter;
        }

        /// <summary>焼くせんべいの頂点データを取得する </summary>
        /// <param name="vertexDataName">頂点データの登録名</param>
        VertexData GetBakeVertexData(string registerName)
        {
            var vertexData = (VertexData)_senbeiDataHandler.GetVertexData(registerName);
            return vertexData;
        }
    }
}

/// <summary>せんべいの形 </summary>
public enum SenbeiType
{
    Circle = 0,
    Heart = 1,
    Square = 2,
    Star = 3,
    Triangle = 4
}
