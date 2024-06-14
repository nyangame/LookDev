using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class SkillTree : MonoBehaviour,IInitialize
{
    // ノードを格納するリスト
    private List<Node> _nodeList;

    // ボタンを格納するリスト
    private List<CommonButton> _buttonList;

    [SerializeField] private CommonButton _buttonPrefab;

    [SerializeField] private NodeListSO _nodeListSO;

    private CanvasGroup _canvasGroup;

    //　各ノードの種類ごとのイベント配列
    private Dictionary<NodeKind, Action> _nodeEvents;
    
    public void Initialize()
    {
        if (_nodeListSO)
        {
            _nodeList = _nodeListSO.NodeList;
        }
        // ボタンのリストを初期化する
        _buttonList = new List<CommonButton>();

        // ノードを生成する
        CreateNodes();

        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetEvent(Dictionary<NodeKind,Action> nodeEvents)
    {
        _nodeEvents = nodeEvents;
    }

    // ボタンを生成する関数
    private void CreateButton(int nodeId)
    {
        Node node = _nodeList[nodeId];

        // ボタンを生成し、親オブジェクトを設定する
        CommonButton button = Instantiate(_buttonPrefab);
        button.transform.SetParent(transform);

        // ボタンの位置を設定する
        button.transform.position = new Vector2((int)node.ID * 300, node.type * 300);

        // ボタンのテキストを設定する
        button.Text.text = "Skill " + node.ID.ToString();

        // ボタンの有効/無効を設定する
        button.Button.interactable = node.isActive;

        // ボタンのクリックイベントを設定する
        button.Button.onClick.AddListener(() =>
        {
            // ボタンがクリックされたときの処理
            if (node.isObtain)
            {
                // ノードが解放されている場合の処理
                Debug.Log("Skill " + node.ID.ToString() + " is obtained!");
            }
            else
            {
                // ノードが解放されていない場合の処理
                Debug.Log("Skill " + node.ID.ToString() + " is not obtained!");
                OnButtonClick(node);
            }
        });

        // ボタンをリストに追加する
        _buttonList.Add(button);
    }

    // ノードを生成する関数
    private void CreateNodes()
    {
        // ノードを生成する
        for (int i = 0; i < _nodeList.Count; i++)
        {
            CreateButton(i);
        }
    }

    // ノードの状態を更新する関数
    private void UpdateNodes()
    {
        // ボタンの状態を更新する
        for (int i = 0; i < _nodeList.Count; i++)
        {
            Node node = _nodeList[i];
            if (node.PrevNodeID >= 0 && node.PrevNodeID < _nodeList.Count)
            {
                // ボタンの有効/無効を設定する
                node.isActive = _nodeList[node.PrevNodeID].isObtain;
                _buttonList[i].Button.interactable = node.isActive;
            }
        }
    }

    public void SetActiveView(bool isActive)
    {
        _canvasGroup.alpha = isActive?1f:0f;
    }

    // ボタンがクリックされたときに呼び出される関数
    public void OnButtonClick(Node node)
    {
        // ノードの状態を更新する
        node.isObtain = true;
        // 設定されたイベントを呼ぶ
        Action onClickEvent = _nodeEvents[node.ID];
        onClickEvent.Invoke();
        UpdateNodes();
    }

}