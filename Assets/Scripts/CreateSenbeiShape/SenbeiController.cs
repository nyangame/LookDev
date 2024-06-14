using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CreateSenbei
{
    /// <summary>せんべいを操作するクラス </summary>
    public class SenbeiController : MonoBehaviour
    {
        [SerializeField, Tooltip("せんべいモデル(データクラス)")]
        SenbeiModel _senbeiModel;
        [SerializeField, Header("表面オブジェクトのメッシュ")]
        MeshFilter _surfaceMeshFilter = default;
        [SerializeField, Header("裏面オブジェクトのメッシュ")]
        MeshFilter _backMeshFilter = default;
        [SerializeField, Header("側面オブジェクトのメッシュ")]
        MeshFilter _sideMeshFilter = default;

        public MeshFilter SurfaceMeshFilter => _surfaceMeshFilter;
        public MeshFilter BackMeshFilter => _backMeshFilter;
        
        Renderer _surfaceRenderer = default;
        Renderer _backRenderer = default;
        Renderer _sideRenderer = default;

        public bool IsStopBaking { get => _senbeiModel.IsStopBaking; set => _senbeiModel.IsStopBaking = value; }

        public SenbeiLength Radius => _senbeiModel.Radius;

        public SenbeiModel SenbeiModel => _senbeiModel;
        
        void Update()
        {
            if (!_senbeiModel.IsSetBulge || _senbeiModel.IsStopBaking) { return; }

            ProfoundProcess();
            var surfaceVertices = _surfaceMeshFilter.mesh.vertices;
            var backVertices = _backMeshFilter.mesh.vertices;
            _senbeiModel.BulegeMag = Mathf.Lerp(0, 1, _senbeiModel.BulegeElapsedTime / _senbeiModel.BakeParameter.BulgeTime);   //膨らみ倍率を計算
            foreach (var bulegeData in _senbeiModel.BulegeData)
                {
                    var surface = bulegeData.Key;
                    var back = bulegeData.Value;
                    surfaceVertices = SetVertexheight(surface.VertexIndexes, surfaceVertices, surface.BulgeHeghts, Side.Front); //表面
                    backVertices = SetVertexheight(back.VertexIndexes, backVertices, back.BulgeHeghts, Side.Back);             //裏面
                }

                RecalculateMesh(_surfaceMeshFilter.mesh, surfaceVertices);
                RecalculateMesh(_backMeshFilter.mesh, backVertices);

                _senbeiModel.BulegeElapsedTime += Time.deltaTime;
            _senbeiModel.BakeElapsedTime += Time.deltaTime;

            CalculateVertexBakeTimes();
            CreateBurntParameter();

            StructVertexColorShaderController.UpdateVertexColorJob(_senbeiModel.BurntParameter, 100f);
        }

        /// <summary>せんべいの種が生成された時の初期化処理 </summary>
        public void InitializeSenbei(SenbeiModel senbeiModel)
        {
            _senbeiModel = senbeiModel;
            foreach (var bulegeData in _senbeiModel.BulegeData)
            {
                var surface = bulegeData.Key;
                var back = bulegeData.Value;
                _senbeiModel.FrontVertices = SetVertexheight(surface.VertexIndexes, _senbeiModel.FrontVertices, surface.BulgeHeghts, Side.Front); //表面
                _senbeiModel.BackVertices = SetVertexheight(back.VertexIndexes, _senbeiModel.BackVertices, back.BulgeHeghts, Side.Back);             //裏面
            }
            SetBulge();
        }

        public void ResumeSenbei(SenbeiModel senbeiModel)
        {
            _senbeiModel = senbeiModel;
            foreach (var bulegeData in _senbeiModel.BulegeData)
            {
                var surface = bulegeData.Key;
                var back = bulegeData.Value;
                _senbeiModel.FrontVertices = SetVertexheight(surface.VertexIndexes, _senbeiModel.FrontVertices, surface.BulgeHeghts, Side.Front); //表面
                _senbeiModel.BackVertices = SetVertexheight(back.VertexIndexes, _senbeiModel.BackVertices, back.BulgeHeghts, Side.Back);             //裏面
            } 
            
            RecalculateMesh(_surfaceMeshFilter.mesh,_senbeiModel.FrontVertices);
            RecalculateMesh(_backMeshFilter.mesh,_senbeiModel.BackVertices);
            RecalculateMesh(_sideMeshFilter.mesh,_senbeiModel.SideVertices);

            //せんべいの厚みを表現
            ProfoundProcess();
        }

        /// <summary>
        /// せんべいをひっくり返す ひっくり返す時は呼んでください
        /// </summary>
        public void ReverseSenbei()
        {
            _senbeiModel.CurrentBakeSide = (_senbeiModel.CurrentBakeSide == Side.Front) ? Side.Back : Side.Front;
        }

        /// <summary> 焼かれている面を取得する</summary>
        public MeshFilter GetBakeSurface()
        {
            if (_senbeiModel.CurrentBakeSide == Side.Front)     //表面が焼かれている
            {
                return _surfaceMeshFilter;
            }
            else
            {
                return _backMeshFilter;
            }
        }

        /// <summary>現在の表面を取得する </summary>
        public MeshFilter GetCurrentSurface()
        {
            if (_senbeiModel.CurrentBakeSide == Side.Front)     //表面が焼かれている
            {
                return _backMeshFilter;
            }
            else
            {
                return _surfaceMeshFilter;
            }
        }

        /// <summary>
        /// 膨らむ倍率を変更する　押し瓦使用時に呼ばれる
        /// 現在の膨らみ倍率から引数分引きます
        /// </summary>
        public void ChangeBulgeMag(float reduceValue, List<int> indexes)
        {
            var tmp = _senbeiModel.BulegeElapsedTime;
            _senbeiModel.BulegeElapsedTime -= reduceValue;
            _senbeiModel.BulegeMag = Mathf.Lerp(0, 1, _senbeiModel.BulegeElapsedTime / _senbeiModel.BakeParameter.BulgeTime);
            var vertices = GetCurrentSurface().sharedMesh.vertices;

            foreach (var bulegeData in _senbeiModel.BulegeData)
            {
                BulgeData data = (_senbeiModel.CurrentBakeSide == Side.Front) ? bulegeData.Value : bulegeData.Key;

                for (var i = 0; i < data.VertexIndexes.Length; i++)
                {
                    var dataIndex = data.VertexIndexes[i];

                    if (!indexes.Contains(dataIndex)) { continue; }
                   
                    var height = data.BulgeHeghts[i];

                    if (_senbeiModel.CurrentBakeSide == Side.Front)
                    {
                        vertices[dataIndex].y = (height * _senbeiModel.BulegeMag) + _senbeiModel.CurrentVertexData.BackVertices[dataIndex].y;
                        data.BulgeHeghts[i] -= Mathf.Min(_senbeiModel.CurrentVertexData.BackVertices[dataIndex].y, height + reduceValue * _senbeiModel.OshigawaraMag);
                    }
                    else
                    {
                        vertices[dataIndex].y = (height * _senbeiModel.BulegeMag) + _senbeiModel.CurrentVertexData.SurfaceVertices[dataIndex].y;
                        data.BulgeHeghts[i] -= Mathf.Min(_senbeiModel.CurrentVertexData.SurfaceVertices[dataIndex].y, height - reduceValue * _senbeiModel.OshigawaraMag);
                    }
                }
            }

            if (_senbeiModel.CurrentBakeSide == Side.Front)
            {
                RecalculateMesh(_backMeshFilter.mesh, vertices);
            }
            else
            {
                RecalculateMesh(_surfaceMeshFilter.mesh, vertices);
            }

            _senbeiModel.BulegeElapsedTime = tmp;
        }
        public void SetCurrentVertex()
        {
            _senbeiModel.FrontVertices = _surfaceMeshFilter.mesh.vertices;
            _senbeiModel.BackVertices = _backMeshFilter.mesh.vertices;
            _senbeiModel.SideVertices = _sideMeshFilter.mesh.vertices;
        }
        /// <summary>頂点を代入し再計算を行う </summary>
        /// <param name="target">対象のメッシュ</param>
        /// <param name="vertices">代入する頂点配列</param>
        void RecalculateMesh(Mesh target, Vector3[] vertices)
        {
            target.vertices = vertices;
            target.RecalculateBounds();     //境界箱
            target.RecalculateNormals();    //法線
            target.RecalculateTangents();   //接線
        }

        #region 厚み表現の処理
        /// <summary>せんべいの厚みを表現する処理 </summary>
        void ProfoundProcess()
        {
            //厚みを出す処理
            var surfaceVertices = _surfaceMeshFilter.sharedMesh.vertices;
            var backVertices = _backMeshFilter.sharedMesh.vertices;

            for (var i = 0; i < surfaceVertices.Length; i++)
            {
                var surfaceFristHeight = _senbeiModel.CurrentVertexData.SurfaceVertices[i].y;
                var backFristHeight = _senbeiModel.CurrentVertexData.BackVertices[i].y;

                surfaceVertices[i].y = Mathf.Lerp(surfaceFristHeight, surfaceFristHeight + _senbeiModel.BakeParameter.Profound, _senbeiModel.BakeElapsedTime / _senbeiModel.BakeParameter.SenbeBakeTime);
                backVertices[i].y = -Mathf.Lerp(backFristHeight, backFristHeight + _senbeiModel.BakeParameter.Profound, _senbeiModel.BakeElapsedTime / _senbeiModel.BakeParameter.SenbeBakeTime);
            }

            _surfaceMeshFilter.sharedMesh.vertices = surfaceVertices;
            _backMeshFilter.sharedMesh.vertices = surfaceVertices;
            RecalculateMesh(_surfaceMeshFilter.mesh, surfaceVertices);
            RecalculateMesh(_backMeshFilter.mesh, backVertices);

            //側面を湾曲させる処理
            var sideVertiecs = _sideMeshFilter.mesh.vertices;
            var coefficient = Mathf.Lerp(0, _senbeiModel.BakeParameter.SideCoefficient, _senbeiModel.BakeElapsedTime / _senbeiModel.BakeParameter.SenbeBakeTime);
            var centerIndex = 3;

            for (var i = 0; i < _senbeiModel.CurrentVertexData.SideVertexList.Count; i++)
            {
                var lineVertices = _senbeiModel.CurrentVertexData.SideVertexList[i].LineVertices;
                var sideIndexes = _senbeiModel.CurrentVertexData.SideVertexList[i].VertexIndexes;
                var surfaceVertex = _surfaceMeshFilter.mesh.vertices[_senbeiModel.CurrentVertexData.SurfaceCircumIndexes[i]];
                var backVertex = _backMeshFilter.mesh.vertices[_senbeiModel.CurrentVertexData.BackCircumIndexes[i]];

                sideVertiecs[sideIndexes[0]].y = surfaceVertex.y;
                sideVertiecs[sideIndexes[sideIndexes.Length - 1]].y = backVertex.y;

                for (var k = 0; k < sideIndexes.Length; k++)
                {
                    if (k == 0 || k == sideIndexes.Length - 1) { continue; }

                    var index = sideIndexes[k];
                    var vertex = lineVertices[k];
                    var nomal = _sideMeshFilter.mesh.normals[index];

                    var lastPoint = vertex + nomal * Mathf.Cos(Mathf.Abs(centerIndex - k) / 4f * Mathf.PI) * coefficient;

                    sideVertiecs[index] = lastPoint;
                }
            }

            RecalculateMesh(_sideMeshFilter.mesh, sideVertiecs);
        }

        #endregion

        #region 焼き関連の処理

        /// <summary>焼かれている面の各頂点ごとの焼き時間を求める </summary>
        void CalculateVertexBakeTimes()
        {
            var vartices = GetBakeSurface().sharedMesh.vertices;    //焼かれている面の頂点を取得

            for (var i = 0; i < vartices.Length; i++)
            {
                var targetVertex = vartices[i];

                if (_senbeiModel.CurrentBakeSide == Side.Front)
                {
                    _senbeiModel.SurfaceVertexBakeTimes[i] += Time.deltaTime * targetVertex.y;
                }
                else
                {
                    _senbeiModel.BackVertexBakeTimes[i] += Time.deltaTime * Mathf.Abs(targetVertex.y);
                }
            }
        }

        /// <summary>マテリアルの色を更新する </summary>
        void UpdateMaterialColor(Color start, Color end, float t)
        {
            _surfaceRenderer.material.color = Color.Lerp(start, end, t);  //表面
            _backRenderer.material.color = Color.Lerp(start, end, t);     //裏面
            _sideRenderer.material.color = Color.Lerp(start, end, t);     //側面
        }

        /// <summary>焼き色に関する処理 </summary>
        void BakeColor()
        {
            var senbeiPos = new Vector2(transform.position.x, transform.position.z);
            var bakeFieldPos = new Vector2(_senbeiModel.BakeFieldCenter.x, _senbeiModel.BakeFieldCenter.z);

            var mag = Mathf.Clamp01(_senbeiModel.BakeParameter.BakeFieldRadius - Vector2.Distance(senbeiPos, bakeFieldPos));     //倍率

            _senbeiModel.BakeElapsedTime += Time.deltaTime * mag;

            if (!_senbeiModel.BurntStart)
            {
                UpdateMaterialColor(_senbeiModel.BakeParameter.DoughColor, _senbeiModel.BakeParameter.SenbeiColor, _senbeiModel.BakeElapsedTime / _senbeiModel.BakeParameter.SenbeBakeTime);
            }
            else
            {
                UpdateMaterialColor(_senbeiModel.BakeParameter.SenbeiColor, _senbeiModel.BakeParameter.BurntColor, _senbeiModel.BakeElapsedTime / _senbeiModel.BakeParameter.BurntTime);
            }
            if (_surfaceRenderer.material.color == _senbeiModel.BakeParameter.SenbeiColor)
            {
                _senbeiModel.BurntStart = true;
                _senbeiModel.BakeElapsedTime = 0;
            }
        }

        /// <summary>焦げパラメーターを作成する </summary>
        void CreateBurntParameter()
        {
            var surface = GetBurntParam(_senbeiModel.CurrentVertexData.SurfaceVertices.Length, _surfaceMeshFilter.sharedMesh.vertices, _senbeiModel.SurfaceVertexBakeTimes);
            var back = GetBurntParam(_senbeiModel.CurrentVertexData.BackVertices.Length, _backMeshFilter.sharedMesh.vertices, _senbeiModel.BackVertexBakeTimes);

            _senbeiModel.BurntParameter = new BurntParameter(surface, back, _senbeiModel.BakeParameter.SenbeiColor);
        }

        /// <summary>焦げの構造体のパラメーターを作成 </summary>
        Dictionary<Vector3, float> GetBurntParam(int count, Vector3[] vertices, float[] times)
        {
            var data = new Dictionary<Vector3, float>();

            for (var i = 0; i < count; i++)   //表面
            {
                var vertex = vertices[i];
                var baketime = times[i];

                data.Add(vertex, baketime);
            }

            return data;
        }

        #endregion

        #region 膨らみ関連の処理

        /// <summary>膨らむ位置を決める </summary>
        void SetBulge()
        {
            for (var i = 0; i < _senbeiModel.BakeParameter.BulgePositionCount;)
            {
                var randIndex = UnityEngine.Random.Range(0, _senbeiModel.CurrentVertexData.SurfaceVertices.Length);
                var surfaceCenter = _senbeiModel.CurrentVertexData.SurfaceVertices[randIndex];
                var backCenter = _senbeiModel.CurrentVertexData.BackVertices.OrderBy(v => Vector3.Distance(v, surfaceCenter)).FirstOrDefault();   //取得した表面の頂点にもっと近い頂点を取得する

                if (GetCircleVertices(_senbeiModel.CurrentVertexData.SurfaceVertices, _senbeiModel.CurrentVertexData.BackVertices, surfaceCenter, backCenter))
                {
                    i++;
                }
            }

            _senbeiModel.IsSetBulge = true;
        }

      
        /// <summary>取得した頂点で膨らみのy位置を計算する </summary>
        /// <param name="indexes">取得した頂点の添え字配列</param>
        /// <param name="center">膨らませる中心</param>
        BulgeData VertexWithinCircleBulge(int[] indexes, Vector3[] vertices, Vector3 center, SenbeiLength radius, float height, Side side)
        {
            var rayCenter = new Vector2(center.x, center.z);
            var heightArray = new float[indexes.Length];

            for (var i = 0; i < indexes.Length; i++)
            {
                var index = indexes[i];
                var vertex = vertices[index];

                var senbeiPos = new Vector2(vertex.x, vertex.z);
                var dist = Vector2.Distance(senbeiPos, rayCenter);
                var dome = Mathf.Sqrt(radius.Value * radius.Value - dist * dist); // 円弧の半径
                var y = (vertex.y + dome) * height; // y座標を計算

                if (side == Side.Front)
                {
                    heightArray[i] = y;
                }
                else
                {
                    heightArray[i] = -y * 1.5f;
                }
            }

            var bulgeData = new BulgeData(center, radius, indexes, heightArray);

            return bulgeData;
        }

        /// <summary>膨らんだ時のy値を取得し、頂点に設定する </summary>
        Vector3[] SetVertexheight(int[] indexes, Vector3[] vertices, float[] heights, Side side)
        {
            for (var i = 0; i < indexes.Length; i++) //表面
            {
                var index = indexes[i];
                var height = heights[i];

                if (side == Side.Front)
                {
                    vertices[index].y = (height * _senbeiModel.BulegeMag) + _senbeiModel.CurrentVertexData.SurfaceVertices[index].y;
                }
                else
                {
                    vertices[index].y = (height * _senbeiModel.BulegeMag) + _senbeiModel.CurrentVertexData.BackVertices[index].y;
                }

            }

            return vertices;
        }

        /// <summary>膨らませる頂点の添え字を取得する </summary>
        /// <param name="getVertices">膨らませる頂点配列</param>
        /// <param name="targetVerices">面オブジェクト頂点配列</param>
        /// <returns>取得した添え字配列</returns>
        int[] GetVertexIndex(Vector3[] getVertices, Vector3[] targetVerices)
        {
            var getIndexes = new int[getVertices.Length];

            for (var i = 0; i < getVertices.Length; i++)    //添え字の取得
            {
                var index = Array.IndexOf(targetVerices, getVertices[i]);
                getIndexes[i] = index;
            }

            return getIndexes;
        }

        /// <summary>膨らませる頂点を取得する </summary>
        /// <param name="surface">表面の頂点配列</param>
        /// <param name="back">裏面の頂点配列</param>
        /// <param name="surfaceCenter">表面の中心点</param>
        /// <param name="backCenter">裏面の中心点</param>
        /// <returns>true=膨らませるデータを作成</returns>
        bool GetCircleVertices(Vector3[] surface, Vector3[] back, Vector3 surfaceCenter, Vector3 backCenter)
        {
            var randRadiusValue = UnityEngine.Random.Range(_senbeiModel.BakeParameter.MinBulgeRadius, _senbeiModel.BakeParameter.MaxBulgeRadius);
            SenbeiLength randSenbeiLength = new SenbeiLength(randRadiusValue);
            _senbeiModel.Radius = randSenbeiLength;
            var randHeight = UnityEngine.Random.Range(_senbeiModel.BakeParameter.MinBulgeHeight, _senbeiModel.BakeParameter.MaxBulgeHeight);

            float dist = Vector3.Distance(surfaceCenter, transform.position); //膨らませる中心からせんべい種の中心までの距離

            if (_senbeiModel.CurrentVertexData.SenbeiLength.Value - dist <= randSenbeiLength.Value) { return false; }   //膨らませる半径がせんべいの端を超えていないか調べる

            foreach (var bulegeData in _senbeiModel.BulegeData) //膨らませる円が他の円と被っていないか調べる
            {
                SenbeiLength distance = new SenbeiLength(Vector3.Distance(bulegeData.Key.Center, surfaceCenter));
                var sumDistance = bulegeData.Key.SenbeiLength.Value + randSenbeiLength.Value;

                if (distance.Value <= sumDistance) { return false; }
            }

            var getSurface = surface.Where(v => Mathf.Pow(v.x - surfaceCenter.x, 2) + Mathf.Pow(v.z - surfaceCenter.z, 2) <= Mathf.Pow(randSenbeiLength.Value, 2)).ToArray();
            var getBack = back.Where(v => Mathf.Pow(v.x - backCenter.x, 2) + Mathf.Pow(v.z - backCenter.z, 2) <= Mathf.Pow(randSenbeiLength.Value, 2)).ToArray();

            var getSurfaceIndexes = GetVertexIndex(getSurface, _senbeiModel.CurrentVertexData.SurfaceVertices);
            var getBackIndexes = GetVertexIndex(getBack, _senbeiModel.CurrentVertexData.BackVertices);

            var surfaceData = VertexWithinCircleBulge(getSurfaceIndexes, _senbeiModel.CurrentVertexData.SurfaceVertices, surfaceCenter, randSenbeiLength, randHeight, Side.Front);
            var backData = VertexWithinCircleBulge(getBackIndexes, _senbeiModel.CurrentVertexData.BackVertices, backCenter, randSenbeiLength, randHeight, Side.Back);

            _senbeiModel.BulegeData.Add(surfaceData, backData);

            return true;
        }

        #endregion
    }
}


