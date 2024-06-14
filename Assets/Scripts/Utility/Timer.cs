using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Timer
{
    private float _interval = 0f;
    private float _timer = 0f;

    public bool IsPause;
    
    public Timer(float interval)
    {
        _interval = interval;
    }

    public bool RunTimer()
    {
        if (IsPause)
        {
            return false;
        }
        _timer += Time.deltaTime;
        if (_timer >= _interval)
        {
            _timer = 0f;
            return true;
        }
        else
        {
            return false;
        }
    }
}
