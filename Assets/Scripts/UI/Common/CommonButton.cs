using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

[RequireComponent(typeof(Button))]
public class CommonButton : UIPartsBase
{
    // Todo 色々便利にする
    private Button _button;
    public Button Button => _button == null ? _button = GetComponent<Button>() : _button;

    [SerializeField]
    private TextMeshProUGUI _text;
    public TextMeshProUGUI Text => _text;

    protected ReactiveProperty<ButtonState> ButtonStateProperty = new ReactiveProperty<ButtonState>();

    protected override void OnInitialize()
    {
        ButtonStateProperty.Subscribe(state => 
        {
            // Todo アニメーション処理
            bool isDefault = state == ButtonState.Default;
            Button.image.color = isDefault ? Color.white : Color.gray;
            Button.interactable = isDefault;
        }).AddTo(this);
    }

    public void SetButtonState(ButtonState state)
    {
        ButtonStateProperty.Value = state;
    }
}

public enum ButtonState
{
    Default,
    Disabled
}
