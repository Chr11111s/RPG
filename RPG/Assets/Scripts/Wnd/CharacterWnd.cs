using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterWnd : BaseWnd
{
    Button _addS, _minusS, _addA, _minusA, _addI, _minusI, _confirm;
    TMP_Text _userName, _lv, _s, _a, _i, _pointTip, _hp, _mp, _exp;
    PlayerInfo _playerInfo;
    public override void Initial()
    {
        SelfTransform.Find("BG").gameObject.AddComponent<WndMove>();

        Button closeBtn = SelfTransform.Find("BG/Close").GetComponent<Button>();
        closeBtn.onClick.AddListener(OnCloseClick);

        _addS = SelfTransform.Find("BG/AddS").GetComponent<Button>();
        _minusS = SelfTransform.Find("BG/MinusS").GetComponent<Button>();
        _addA = SelfTransform.Find("BG/AddA").GetComponent<Button>();
        _minusA = SelfTransform.Find("BG/MinusA").GetComponent<Button>();
        _addI = SelfTransform.Find("BG/AddI").GetComponent<Button>();
        _minusI = SelfTransform.Find("BG/MinusI").GetComponent<Button>();

        _addS.onClick.AddListener(OnAddSClick);
        _minusS.onClick.AddListener(OnMinusSClick);
        _addA.onClick.AddListener(OnAddAClick);
        _minusA.onClick.AddListener(OnMinusAClick);
        _addI.onClick.AddListener(OnAddIClick);
        _minusI.onClick.AddListener(OnMinusIClick);

        _userName = SelfTransform.Find("BG/UserName").GetComponent<TMP_Text>();
        _lv = SelfTransform.Find("BG/Lv").GetComponent<TMP_Text>();
        _s = SelfTransform.Find("BG/S").GetComponent<TMP_Text>();
        _a = SelfTransform.Find("BG/A").GetComponent<TMP_Text>();
        _i = SelfTransform.Find("BG/I").GetComponent<TMP_Text>();
        _pointTip = SelfTransform.Find("BG/PointTip").GetComponent<TMP_Text>();
        _hp = SelfTransform.Find("BG/HP").GetComponent<TMP_Text>();
        _mp = SelfTransform.Find("BG/MP").GetComponent<TMP_Text>();
        _exp = SelfTransform.Find("BG/EXP").GetComponent<TMP_Text>();

        //删掉UpdatePlayerInfo方法的形参PlayerInfo info以及_playerInfo = info;
        _playerInfo = DataManager.Instance.PlayerInfo;

        _userName.text = _playerInfo.username;
        _lv.text = _playerInfo.lv.ToString();
        _s.text = _playerInfo.strength.ToString();
        _a.text = _playerInfo.agility.ToString();
        _i.text = _playerInfo.intelligence.ToString();
        _pointTip.text = _playerInfo.point.ToString();
        _hp.text = _playerInfo.currenthp + "/" + _playerInfo.MAXHP;
        _mp.text = _playerInfo.currentmp + "/" + _playerInfo.MAXMP;
        _exp.text = _playerInfo.exp.ToString();

        _confirm = SelfTransform.Find("BG/Confirm").GetComponent<Button>();
        _confirm.onClick.AddListener(OnConfirmClick);
    }

    private void OnConfirmClick()
    {
        _playerInfo.Reset();
        _confirm.gameObject.SetActive(false);
        CheckPoint();
    }

    public void UpdatePlayerInfo() //以此取代UpdateHP()和UpdateMP()，删掉这两个方法
    {
        _hp.text = _playerInfo.currenthp + "/" + _playerInfo.MAXHP;
        _mp.text = _playerInfo.currentmp + "/" + _playerInfo.MAXMP;

        //将if分支移至ShowLvUpInfo()方法中
    }

    public void SyncPlayerInfo()
    {
        _s.text = _playerInfo.strength.ToString();
        _i.text = _playerInfo.intelligence.ToString();
        _a.text = _playerInfo.agility.ToString();
    }

    public void ShowLvUpInfo()
    {
        _pointTip.gameObject.SetActive(true);
        _confirm.gameObject.SetActive(true);

        /*替换为CheckPoint()
        _addS.gameObject.SetActive(true);
        _minusS.gameObject.SetActive(true);
        _addA.gameObject.SetActive(true);
        _minusA.gameObject.SetActive(true);
        _addI.gameObject.SetActive(true);
        _minusI.gameObject.SetActive(true);
        */
        CheckPoint();
        
    }

    public void HideLvUpInfo()
    {
        _pointTip.gameObject.SetActive(false);
    }

    public void UpdateExp()
    {

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
        else if( _playerInfo.point > 0)
        {
            _addS.gameObject.SetActive(true);
            _addA.gameObject.SetActive(true);
            _addI.gameObject.SetActive(true);
        }

        _pointTip.text = "可用点数：" + _playerInfo.point;
    }

    private void OnMinusIClick()
    {
        _playerInfo.point++;

        _playerInfo.currenti--;

        _i.text = _playerInfo.currenti.ToString();

        CheckPoint();
    }

    private void OnAddIClick()
    {
        _playerInfo.point--;

        _playerInfo.currenti++;

        _i.text = _playerInfo.currenti.ToString();

        CheckPoint();
    }

    private void OnMinusAClick()
    {
        _playerInfo.point++;

        _playerInfo.currenta--;

        _a.text = _playerInfo.currenta.ToString();

        CheckPoint();
    }

    private void OnAddAClick()
    {
        _playerInfo.point--;

        _playerInfo.currenta++;

        _a.text = _playerInfo.currenta.ToString();

        CheckPoint();
    }

    private void OnMinusSClick()
    {
        _playerInfo.point++;

        _playerInfo.currents--;

        _s.text = _playerInfo.currents.ToString();

        CheckPoint();
    }

    private void OnAddSClick()
    {
        _playerInfo.point--;

        _playerInfo.currents++;

        _s.text = _playerInfo.currents.ToString();

        CheckPoint();
    }

    private void OnCloseClick()
    {
        CloseWnd();
    }
}
