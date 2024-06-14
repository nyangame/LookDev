using System.Threading;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class OshigawaraCotroller : MonoBehaviour
{
    [SerializeField, Header("動きにかかる時間")] float _moveTime;
    [SerializeField, Header("押し瓦のモデル")] GameObject _model;
    [SerializeField, Header("押し瓦上の位置")] float _resetPosY = 0f;
    [SerializeField, Header("押し瓦下の位置")] float _pushPosY = 0f;
    //ToDo 後でクリックで取得できるようにする
    [SerializeField, Header("テスト用のせんべい位置")] Transform _senbeiTransform;

    public async UniTask PushSenbei(ToolsSenbeiModel senbeiModel,CancellationToken ct)
    {
        //↓仮
        _model.SetActive(true);
        transform.position = new Vector3(senbeiModel.Transform.position.x, _resetPosY, senbeiModel.Transform.position.z);

        await transform.DOLocalMoveY(_pushPosY, _moveTime).SetLink(this.gameObject).ToUniTask(tweenCancelBehaviour: TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
    }

    public async UniTask StopPushSenbei(CancellationToken ct)
    {
        await transform.DOLocalMoveY(_resetPosY, _moveTime).SetLink(this.gameObject).ToUniTask(tweenCancelBehaviour: TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
        //↓仮
        _model.SetActive(false);
    }
}
