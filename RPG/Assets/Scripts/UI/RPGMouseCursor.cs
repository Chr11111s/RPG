using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RPGMouseCursor : MonoBehaviour
{
    GameObject _normal;
    GameObject _select;
    Image _selectImage; //Select播放选中动画

    // Start is called before the first frame update
    void Start()
    {
        _normal = transform.Find("Normal").gameObject;
        _select = transform.Find("Select").gameObject;
        _selectImage = _select.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void SwitchToSelect()
    {
        _normal.SetActive(false);
        _select.SetActive(true);
    }

    public void SwitchToNormal()
    {
        _normal.SetActive(true);
        _select.SetActive(false);
    }
}
