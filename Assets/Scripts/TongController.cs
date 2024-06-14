using System.Threading;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class TongController : MonoBehaviour
{
    [SerializeField, Header("要する時間")] float _reverceTime;
    [SerializeField, Header("ズレ")] float _offset;
    [SerializeField, Header("回転前の値")] Vector3 _resetRotation = new Vector3(0, 180, 0);
    [SerializeField, Header("回転後の値")] Vector3 _rotation = new Vector3(0, 180, 180);
    [SerializeField, Header("トングのモデル")] GameObject _tongModel;
    public async UniTask ReverceSenbei(ToolsSenbeiModel senbeiObj,CancellationToken ct)
    {
        _tongModel.SetActive(true);
        SetTongPosition(senbeiObj.Transform, senbeiObj.Radius.Value);
        senbeiObj.Transform.SetParent(transform);
        await transform.DOLocalRotate(_rotation, _reverceTime).SetLink(this.gameObject).ToUniTask(tweenCancelBehaviour: TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
        senbeiObj.Transform.parent = null;
        transform.eulerAngles = _resetRotation;
        _tongModel.SetActive(false);
    }

    void SetTongPosition(Transform senbeiObj, float radius)
    {
        float z = -radius - senbeiObj.transform.position.z - _offset;
        transform.position = new Vector3(0, 0, z);
    }
}
