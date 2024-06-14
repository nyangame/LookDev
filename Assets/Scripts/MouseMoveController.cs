using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMoveController : MonoBehaviour
{
    Vector3 _mousePos;
    Vector3 _worldPos;

    void Update()
    {
        ObjectMove();
    }

    void ObjectMove()
    {
        _mousePos = Input.mousePosition;
        _worldPos = Camera.main.ScreenToWorldPoint(_mousePos);
        transform.position = _worldPos;
    }
}
