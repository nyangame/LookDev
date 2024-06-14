using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable, ExecuteAlways]
public class CharcoalIntensityController : MonoBehaviour
{
    [SerializeField] float _firePower = 1f;
    [SerializeField] float _rotateTime = 1f;
    [SerializeField] Material _charcoalmaterial;
    [SerializeField] Color _charcoalEmmisionColor = Color.white;

    #region prop
    /// <summary>
    /// 火力調整
    /// </summary>
    public float FirePower
    {
        get
        {
            return _firePower;
        }
        set
        {
            _firePower = value;
        }
    }
    /// <summary>
    /// 繰り返す時間
    /// </summary>
    public float RotateTime
    {
        get
        {
            return _rotateTime;
        }
        set
        {
            _rotateTime = value;
        }
    }
    /// <summary>
    /// 炭の発光色
    /// </summary>
    public Color CharcoalEmmisionColor
    {
        get
        {
            return _charcoalEmmisionColor;
        }
        set
        {
            _charcoalEmmisionColor = value;
        }
    }
    /// <summary>
    /// 炭のマテリアル
    /// </summary>
    public Material CharcoalMaterial
    {
        get
        {
            return _charcoalmaterial;
        }
        set
        {
            _charcoalmaterial = value;
        }
    }

    #endregion
    float time = 0f;

    void FixedUpdate()
    {
        time += Time.deltaTime;
        float s = Mathf.Sin(time * 3.14f / _rotateTime);

        Color setColor = ShaderSupport.ShaderSupport.ToHDRColor(_charcoalEmmisionColor, s * _firePower);

        _charcoalmaterial.SetColor("_EmissionColor", setColor);
    }
}
