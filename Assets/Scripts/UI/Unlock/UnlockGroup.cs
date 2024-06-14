using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnlockGroup : UIPartsBase
{
    [SerializeField,Header("アンロックのセルリスト")]
    List<UnlockCell> _cells = new List<UnlockCell>();    

    protected override void OnInitialize()
    {
        _cells.ForEach(cell=> cell.Initialize());
    }
}
