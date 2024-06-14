using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class ReactionManager : MonoBehaviour,IInitialize
{
    [SerializeField]
    ReactionPanelBase _reactionPanelPrefab;
    [SerializeField]
    int _maxPanelCount = 4;
    [SerializeField]
    Vector2 _panelSize;

    [SerializeField] private float _reactionFadeTime = 3f;
    [SerializeField] private float _interval = 5f;
    public float Interval => _interval;

    ObjectPool<ReactionPanelBase> _reactionPanelPool;

    bool[] _availableArray;

    private bool _isStop;
    int _currentAvailableIndex
    {
        get
        {
            int count = 0;
            foreach (var i in _availableArray)
            {
                if (i)
                {
                    break;
                }
                count++;
            }
            return count;
        }
    }


    public void Initialize()
    {
        _availableArray = new bool[_maxPanelCount];
        Array.Fill(_availableArray, true);
        _reactionPanelPool = new ObjectPool<ReactionPanelBase>(_maxPanelCount, _reactionPanelPrefab, this.transform);
    }

    public async UniTask ReactionAsync(CancellationToken ct)
    {
        var panel = _reactionPanelPool.RentObject();
        if (panel == null || _currentAvailableIndex >= _availableArray.Length)
        {
            await UniTask.CompletedTask;
        }
        else
        {
            int index = _currentAvailableIndex;
            _availableArray[index] = false;
            panel.SetSize(_panelSize);
            panel.SetAnchoredPosition(new Vector2(_panelSize.x, index * _panelSize.y));
            await panel.MoveAsync(0, 1f,ct);
            await UniTask.Delay(TimeSpan.FromSeconds(_reactionFadeTime),cancellationToken:ct);
            await panel.MoveAsync(_panelSize.x, 1f,ct);
            _reactionPanelPool.ReturnObject(panel);
            _availableArray[index] = true;
        }
    }

    public async UniTask EventAsync(CancellationToken ct)
    {
        var panel = _reactionPanelPool.RentObject();
        if (panel == null || _currentAvailableIndex >= _availableArray.Length)
        {
            await UniTask.CompletedTask;
        }
        else
        {
            int index = _currentAvailableIndex;
            _availableArray[index] = false;
            panel.SetSize(_panelSize);
            Debug.Log(index);
            panel.SetAnchoredPosition(new Vector2(_panelSize.x, index * _panelSize.y));
            await panel.MoveAsync(0, 1f,ct);
            await panel.ClickWait(ct);
            await panel.MoveAsync(_panelSize.x, 1f,ct);
            _reactionPanelPool.ReturnObject(panel);
            _availableArray[index] = true;
        }
    }
}
