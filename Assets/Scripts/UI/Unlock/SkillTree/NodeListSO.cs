using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NodeListSO : ScriptableObject
{
    [SerializeField]
    List<Node> _nodeList;
    public List<Node> NodeList => _nodeList;
}
