using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    Vector3 _offset;
    public static PlayerCamera Instance;
    bool _hasInit;

    public void Initial()
    {
        _offset = transform.position - PlayerCtrl.Instance.transform.position;
        _hasInit = true;
    }
    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if (_hasInit)
        {
            transform.position = _offset + PlayerCtrl.Instance.transform.position;

        }
    }
}
