using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillWnd : BaseWnd
{
    TMP_Text _pointTip;
    Transform _info;
    TMP_Text _infoTitle;
    TMP_Text _infoDesc;
    PlayerInfo _playerInfo;
    public override void Initial()
    {
        SelfTransform.gameObject.AddComponent<WndMove>();

        Button closeBtn = SelfTransform.Find("BG/Close").GetComponent<Button>();
        closeBtn.onClick.AddListener(OnCloseClick);

        GameObject l1Slot = SelfTransform.Find("BG/L1Slot").gameObject;
        SkillSlot l1SkillSlot = l1Slot.AddComponent<SkillSlot>();
        GameObject l2Slot = SelfTransform.Find("BG/L2Slot").gameObject;
        SkillSlot l2SkillSlot = l2Slot.AddComponent<SkillSlot>();
        GameObject r1Slot = SelfTransform.Find("BG/R1Slot").gameObject;
        SkillSlot r1SkillSlot = r1Slot.AddComponent<SkillSlot>();
        GameObject r2Slot = SelfTransform.Find("BG/R2Slot").gameObject;
        SkillSlot r2SkillSlot = r2Slot.AddComponent<SkillSlot>();
        GameObject tSlot = SelfTransform.Find("BG/TSlot").gameObject;
        SkillSlot tSkillSlot = tSlot.AddComponent<SkillSlot>();

        l1SkillSlot.Initial(null, DataManager.Instance.GetSkill(0));
        l2SkillSlot.Initial(new SkillSlot[] { l1SkillSlot }, DataManager.Instance.GetSkill(1));
        r1SkillSlot.Initial(null, DataManager.Instance.GetSkill(2));
        r2SkillSlot.Initial(new SkillSlot[] { r1SkillSlot }, DataManager.Instance.GetSkill(3));
        tSkillSlot.Initial(new SkillSlot[] { r2SkillSlot, l2SkillSlot }, DataManager.Instance.GetSkill(4));

        _pointTip = SelfTransform.Find("BG/Point/Tip").GetComponent<TMP_Text>();
        _info = SelfTransform.Find("BG/Info");
        _infoTitle = _info.Find("Title").GetComponent<TMP_Text>();
        _infoDesc = _info.Find("Desc").GetComponent<TMP_Text>();

        _playerInfo = DataManager.Instance.PlayerInfo;

        UpdatePlayerInfo();
    }

    protected override void OnOpenWnd()
    {
        UpdatePlayerInfo();
    }

    public void ShowSkillInfo(SkillInfo skillInfo, Vector3 pos)
    {
        _infoTitle.text = skillInfo.skillname;
        _infoDesc.text = skillInfo.skilldesc;
        _info.position = pos;
        _info.gameObject.SetActive(true);
    }


    public void HideSkillInfo()
    {
        _info.gameObject.SetActive(false);
    }

    public void UpdatePlayerInfo() //代替SetPoint()方法，删掉SetPoint()
    {
        _pointTip.text = "可用点数：" + DataManager.Instance.PlayerInfo.skillpoint;
    }

    private void OnCloseClick()
    {
        CloseWnd();
    }
}
