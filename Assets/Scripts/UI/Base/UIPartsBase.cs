using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIPartsBase : MonoBehaviour
{
    CanvasGroup _canvasGroup;

    [SerializeField]
    bool PlayOnAwake = false;

    private bool _isInitialized = false;

    public void Initialize()
    {
        if(_isInitialized)
        {
            return;
        }
        _isInitialized = true;
        _canvasGroup = null ?? GetComponent<CanvasGroup>();
        OnInitialize();
    }

    protected virtual void OnInitialize()
    {
    }

    private void Start()
    {
        if(PlayOnAwake)
        {
            Initialize();
        }
    }

    /// <summary>
    /// 見た目の表示、非表示を設定する
    /// </summary>
    public void SetViewActive(bool isActive)
    {
        _canvasGroup.alpha = isActive ? 1f : 0f;
    }
}
