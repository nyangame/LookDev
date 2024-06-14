using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class CommonText : UIPartsBase
{
    TextMeshPro _text;
    protected override void OnInitialize()
    {
        _text = GetComponent<TextMeshPro>();
    }

    /// <summary>
    /// Textに文字列を指定する
    /// </summary>
    public void SetText(string text)
    {
        _text.text = text;
    }
}
