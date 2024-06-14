using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 長さ単位
/// </summary>
public record SenbeiLength
{
    public readonly float Value;

    public SenbeiLength(float radius)
    {
        if (radius<0)
        {
            throw new ArgumentException($"不正な値です{radius}");
        }
        Value = radius;
    }
}
