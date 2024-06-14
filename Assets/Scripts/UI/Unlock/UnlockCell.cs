using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockCell : UIPartsBase
{
    [SerializeField, Header("レベル上げのボタン")]
    CommonButton _levelupButton;
    [SerializeField]
    TextMeshPro _cellTitle;

    protected override void OnInitialize()
    {
        _levelupButton.Initialize();
        _levelupButton.SetButtonState(ButtonState.Disabled);
    }
}
