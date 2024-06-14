using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    List<T> _poolList = new List<T>();
    int _maxCount = 0;
    public int CurrentAvailableIndex
    {
        get
        {
            int count = 0;
            foreach (var i in _poolList)
            {
                if (i.isActiveAndEnabled == false)
                {
                    break;
                }
                count++;
            }
            return count;
        }
    }
    public ObjectPool(int maxCount, T prefab, Transform parent)
    {
        _maxCount = maxCount;
        for (int i = 0; i < maxCount; i++)
        {
            var obj = GameObject.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            _poolList.Add(obj);
        }
    }

    public T RentObject()
    {
        if(_maxCount<= CurrentAvailableIndex)
        {
#if UNITY_EDITOR
            Debug.Log("最大数を超えているので生成できませんでした");
#endif
            return null;
        }
        var obj = _poolList[CurrentAvailableIndex];
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnObject(T obj)
    {
        if (_poolList.Contains(obj) == false)
        {
            return;
        }
        int index = _poolList.IndexOf(obj);
        _poolList[index].gameObject.SetActive(false);
    }
}
