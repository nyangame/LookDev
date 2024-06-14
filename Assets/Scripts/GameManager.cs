using System;
using System.Collections.Generic;
using System.Threading;
using CreateSenbei;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("せんべい関係")]
    [SerializeField,Tooltip("せんべい生成クラス")] private SenbeiSpawner _senbeiSpawner;
    [Header("トング関係")]
    [SerializeField, Tooltip("トング使用クラス")] private TongController _tongController;
    [Header("押し瓦関係")]
    [SerializeField, Tooltip("押し瓦使用クラス")] private OshigawaraCotroller _oshigawaraCotroller;
    [SerializeField, Tooltip("押し瓦内部データクラス")] private OsigawaraProcessClass _osigawaraProcess;

    [Header("UI関連")]
    [SerializeField,Tooltip("リアクション管理クラス")]
    private ReactionManager _reactionManager;
    [SerializeField,Tooltip("スキルツリー管理クラス")]
    private SkillTree _skillTree;

    private Timer _timer;

    private Dictionary<NodeKind, Action> _skillTreeNodeActions = new Dictionary<NodeKind, Action>();

    private CancellationTokenSource _cancellationTokenSource; 

    private void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource(); 
        _reactionManager.Initialize();
        _skillTree.Initialize();
        SetEvent();
        _skillTree.SetEvent(_skillTreeNodeActions);
        _skillTree.SetActiveView(false);
        _timer = new Timer(_reactionManager.Interval);

        if (ApplicationManager.Instance.SenbeiModel != null)
        {
            _senbeiSpawner.SetResumeSenbeiData(ApplicationManager.Instance.SenbeiModel);
        }
    }

    private void SetEvent()
    {
        foreach (var nodeKind in Enum.GetValues(typeof(NodeKind)))
        {
            _skillTreeNodeActions.Add((NodeKind)nodeKind, () =>
            {
                Debug.Log(nodeKind);
            });
        }
    }

    private async void Update()
    {
        // Todo プラットフォーム対応
        if (Input.GetButtonDown("Fire1"))
        {
            ToolsSenbeiModel toolsSenbeiModel = new ToolsSenbeiModel(_senbeiSpawner.CurrentBakeSenbei.transform,
            _senbeiSpawner.CurrentBakeSenbei.Radius);
            try
            {
                await _tongController.ReverceSenbei(toolsSenbeiModel,_cancellationTokenSource.Token);
            }
            catch (OperationCanceledException  exception)
            {
                Debug.Log("キャンセルされました");
            }
            _senbeiSpawner.CurrentBakeSenbei.ReverseSenbei();
        }
        
        if (Input.GetButtonDown("Fire2"))   //押し瓦を使用する
        {
            _senbeiSpawner.CurrentBakeSenbei.IsStopBaking = true;

            RaycastHit hit;

            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) { return; }  //せんべいに衝突したか調べる

            var meshCo = hit.collider as MeshCollider;

            if (meshCo == null || meshCo.sharedMesh == null) { return; }

            _osigawaraProcess.OsigawaraProcess(hit.point);
            Debug.Log("おしはじめ");
            // せんべいの情報クラスのインスタンスを生成して渡す
            var senbeiModel = new ToolsSenbeiModel(_senbeiSpawner.CurrentBakeSenbei.transform, _senbeiSpawner.CurrentBakeSenbei.Radius);
            try
            {
                await _oshigawaraCotroller.PushSenbei(senbeiModel,_cancellationTokenSource.Token);
                await UniTask.WaitUntil(() => Input.GetButtonUp("Fire2"),cancellationToken:this.GetCancellationTokenOnDestroy());
                await _oshigawaraCotroller.StopPushSenbei(_cancellationTokenSource.Token);
            }
            catch (OperationCanceledException  exception)
            {
                Debug.Log("キャンセルされました");
            }
            _senbeiSpawner.CurrentBakeSenbei.IsStopBaking = false;
            Debug.Log("押し終わり");

        }

        if (_timer.RunTimer())
        {
            try
            {
                await _reactionManager.ReactionAsync(_cancellationTokenSource.Token);
            }
            catch (OperationCanceledException  exception)
            {
                Debug.Log("キャンセルされました");
            }
            
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _skillTree.SetActiveView(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            _skillTree.SetActiveView(false);

        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            IsPause(Time.timeScale == 1f);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            ApplicationManager.Instance.SenbeiModel = _senbeiSpawner.CurrentBakeSenbei.SenbeiModel;
            _cancellationTokenSource.Cancel();
            SceneManager.LoadScene("Test");
        }
        
    }

    private void IsPause(bool isPause)
    {
        Time.timeScale = isPause?0f:1f;
    }
}
