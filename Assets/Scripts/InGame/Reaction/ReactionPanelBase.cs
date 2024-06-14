using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine.UI;

/// <summary>
/// スライドインする客の反応やボーナス
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class ReactionPanelBase : UIPartsBase
{
    [SerializeField]
    Button _touchArea;
    [SerializeField]
    private RectTransform _rectTransform;
    private RectTransform RectTransform
    {
        get
        {
            if(_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }

    public void SetSize(Vector2 size)
    {
        RectTransform.sizeDelta = size;
    }
    public void SetAnchoredPosition(Vector2 positon)
    {
        RectTransform.anchoredPosition = positon;
    }
    public async UniTask MoveAsync(float endValue, float duration,CancellationToken ct)
    {
        await RectTransform.DOAnchorPosX(endValue, duration).SetLink(this.gameObject).ToUniTask(tweenCancelBehaviour: TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
    }

    public async UniTask ClickWait(CancellationToken ct)
    {
        await _touchArea.OnClickAsync(ct);
    }
}
