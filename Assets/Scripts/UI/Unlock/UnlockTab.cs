using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnlockTab : UIPartsBase
{
    [SerializeField, Header("アンロックの種類グループ")]
    List<UnlockGroup> _unlockGroups = new List<UnlockGroup>();

    [SerializeField, Header("タブを切り替えるボタン")]
    CommonButton _tabButton;

    protected override void OnInitialize()
    {
        _unlockGroups.ForEach(group => group.Initialize());
        _tabButton.Initialize();
    }
}

public enum UnlockTabCategoryType
{
    Facility
}
