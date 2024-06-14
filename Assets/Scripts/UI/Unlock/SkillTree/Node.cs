using System;
using UnityEngine.Serialization;

// ノードの情報を保存する構造体
[Serializable]
public class Node
{
    public NodeKind ID;         // ノードのID
    public int PrevNodeID;    // 前のノードのID
    public bool isActive;   // 有効かどうか
    public bool isObtain;   // 解放されているかどうか
    public int type;        // ノードの傾向タイプ
    public int param;       // 影響値
}

public enum NodeKind
{
    Tong,
    Kote
}
