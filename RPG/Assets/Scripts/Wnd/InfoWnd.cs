using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoWnd : BaseWnd
{
    TMP_InputField _userName;
    TMP_Text _strength, _agility, _intelligence, _tip, _pointTip;
    Button _addS, _minusS, _addA, _minusA, _addI, _minusI;
    PlayerInfo _playerInfo;

    public override void Initial()
    {
        Button createBtn = SelfTransform.Find("CreateBtn").GetComponent<Button>();
        createBtn.onClick.AddListener(OnCreateClick);

        _userName = SelfTransform.Find("UserName").GetComponent<TMP_InputField>();

        _strength = SelfTransform.Find("Value/Strength").GetComponent<TMP_Text>();
        _agility = SelfTransform.Find("Value/Agility").GetComponent<TMP_Text>();
        _intelligence = SelfTransform.Find("Value/Intelligence").GetComponent<TMP_Text>();

        _addS = SelfTransform.Find("Btns/Strength/Add").GetComponent<Button>();
        _minusS = SelfTransform.Find("Btns/Strength/Minus").GetComponent<Button>();
        _addA = SelfTransform.Find("Btns/Agility/Add").GetComponent<Button>();
        _minusA = SelfTransform.Find("Btns/Agility/Minus").GetComponent<Button>();
        _addI = SelfTransform.Find("Btns/Intelligence/Add").GetComponent<Button>();
        _minusI = SelfTransform.Find("Btns/Intelligence/Minus").GetComponent<Button>();

        _addS.onClick.AddListener(OnAddSClick);
        _minusS.onClick.AddListener(OnMinusSClick);
        _addA.onClick.AddListener(OnAddAClick);
        _minusA.onClick.AddListener(OnMinusAClick);
        _addI.onClick.AddListener(OnAddIClick);
        _minusI.onClick.AddListener(OnMinusIClick);

        _tip = SelfTransform.Find("Tip").GetComponent<TMP_Text>();

        _pointTip = SelfTransform.Find("PointTip").GetComponent<TMP_Text>();

        _playerInfo = DataManager.Instance.PlayerInfo;

        _playerInfo.Reset();

        _strength.text = _playerInfo.currents.ToString();
        _agility.text = _playerInfo.currenta.ToString();
        _intelligence.text = _playerInfo.currenti.ToString();

        CheckPoint();
    }

    private void OnMinusIClick()
    {
        _playerInfo.point++;

        _playerInfo.currenti--;

        _intelligence.text = _playerInfo.currenti.ToString();

        _tip.text = "每点智力提升20点魔法上限，增加5点魔法恢复速度";

        CheckPoint();
    }

    private void OnAddIClick()
    {
        _playerInfo.point--;

        _playerInfo.currenti++;

        _intelligence.text = _playerInfo.currenti.ToString();

        _tip.text = "每点智力提升20点魔法上限，增加5点魔法恢复速度";

        CheckPoint();
    }

    private void OnMinusAClick()
    {
        _playerInfo.point++;

        _playerInfo.currenta--;

        _agility.text = _playerInfo.currenta.ToString();

        _tip.text = "每点敏捷提升5点移动速度";

        CheckPoint();
    }

    private void OnAddAClick()
    {
        _playerInfo.point--;

        _playerInfo.currenta++;

        _agility.text = _playerInfo.currenta.ToString();

        _tip.text = "每点敏捷提升5点移动速度";

        CheckPoint();
    }

    private void OnMinusSClick()
    {
        _playerInfo.point++;

        _playerInfo.currents--;

        _strength.text = _playerInfo.currents.ToString();

        _tip.text = "每点力量提升10点生命上限，增加5点生命恢复速度";

        CheckPoint();
    }

    private void OnAddSClick()
    {
        _playerInfo.point--;

        _playerInfo.currents++;

        _strength.text = _playerInfo.currents.ToString();

        _tip.text = "每点力量提升10点生命上限，增加5点生命恢复速度";

        CheckPoint();
    }

    private void OnCreateClick() //StartWnd的OnStartClick()方法放到这里写
    {
        CloseWnd();
        _playerInfo.Reset();
        _playerInfo.username = _userName.text;
        GameManager.Instance.StratGame();
    }

    void CheckPoint()
    {
        if (_playerInfo.currenti == _playerInfo.intelligence)
        {
            _minusI.gameObject.SetActive(false);
        }
        else
        {
            _minusI.gameObject.SetActive(true);
        }

        if (_playerInfo.currenta == _playerInfo.agility)
        {
            _minusA.gameObject.SetActive(false);
        }
        else
        {
            _minusA.gameObject?.SetActive(true);
        }

        if (_playerInfo.currents == _playerInfo.strength)
        {
            _minusS.gameObject.SetActive(false);
        }
        else
        {
            _minusS.gameObject.SetActive(true);
        }

        if (_playerInfo.point == 0)
        {
            _addS.gameObject.SetActive(false);
            _addA.gameObject.SetActive(false);
            _addI.gameObject.SetActive(false);
        }
        else if (_playerInfo.point > 0)
        {
            _addS.gameObject.SetActive(true);
            _addA.gameObject.SetActive(true);
            _addI.gameObject.SetActive(true);
        }

        _pointTip.text = "可用点数：" + _playerInfo.point;
    }
}
