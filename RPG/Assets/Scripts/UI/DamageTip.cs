using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTip : MonoBehaviour
{
    Transform _target;
    TMP_Text _tip;
    bool _toBig;
    float _workTime;
    float _originSize;
    float _targetSize;
    float _height = 50f;
    float _floatWorkTime;

    public void Initial(Transform target, string content)
    {
        _tip = GetComponent<TMP_Text>();
        _originSize = _tip.fontSize;
        _targetSize = _tip.fontSize * 2;
        _target = target;
        _tip.text = content;

        //将Update()中定义位置的代码在初始化时就挪过去
        Vector3 screenPos = Camera.main.WorldToScreenPoint(_target.position);
        transform.position = screenPos + Vector3.up * _height * _floatWorkTime;
    }

    private void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        _workTime += Time.deltaTime;
        _floatWorkTime += Time.deltaTime;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(_target.position);
        transform.position = screenPos + Vector3.up * _height * _floatWorkTime;

        if (_toBig)
        {
            _tip.fontSize = Mathf.Lerp(_originSize, _targetSize, _workTime);
        }
        else
        {
            _tip.fontSize = Mathf.Lerp(_targetSize, _originSize, _workTime);

        }

        if (_workTime >= 1)
        {
            _workTime = 0;
            if (!_toBig)
            {
                Destroy(gameObject);
            }
        }
    }
}
