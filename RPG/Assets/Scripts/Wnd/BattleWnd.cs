using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleWnd : BaseWnd
{
    GameObject _originHealthBar;
    Slider _hpBar;
    Slider _mpBar;
    Slider _expBar;
    GameObject _damageTip;
    PlayerInfo _playerInfo;

    List<HotSlot> _hotSlots;

    HotSlot _currentSlot;

    public override void Initial()
    {
        _hpBar = SelfTransform.Find("HPBar").GetComponent<Slider>();
        _mpBar = SelfTransform.Find("MPBar").GetComponent<Slider>();
        _expBar = SelfTransform.Find("EXPBar").GetComponent<Slider>();
        _originHealthBar = SelfTransform.Find("HealthBar").gameObject;
        _damageTip = SelfTransform.Find("DamageTip").gameObject;

        _playerInfo = DataManager.Instance.PlayerInfo;

        Transform hotBar = SelfTransform.Find("HotBar");
        _hotSlots = new List<HotSlot>();
        for (int i = 0; i < hotBar.childCount; i++)
        {
            Transform slot = hotBar.GetChild(i);
            HotSlot hotSlot = slot.gameObject.AddComponent<HotSlot>();
            _hotSlots.Add(hotSlot);
            hotSlot.Initial(i);
        }
    }

    public void SetHotSlot(int index, SkillInfo skillInfo)
    {
        _hotSlots[index].SetSkill(skillInfo);
    }

    public void SetHotSlot(ItemInfo itemInfo)
    {
        if (_currentSlot == null)
        {
            return;
        }
        _currentSlot.SetItem(itemInfo);
    }


    public void SetCurrentSlot(HotSlot slot)
    {
        _currentSlot = slot;
        WndManager.Instance.GetWnd<InventoryWnd>().SwitchDragSort(DragSortEnum.到快捷栏);
    }

    public void ClearCurrentSlot()
    {
        _currentSlot = null;
    }

    public void Use(int index)
    {
        _hotSlots[index].Use();
    }

    public void UpdatePlayerInfo()
    {
        _hpBar.value = _playerInfo.currenthp / _playerInfo.MAXHP;
        _mpBar.value = _playerInfo.currentmp / _playerInfo.MAXMP;
        _expBar.value = (float)_playerInfo.exp / _playerInfo.LvUpExp;
    }

    public void AddDamageTip(Transform target, string content)
    {
        GameObject cloneTip = GameObject.Instantiate(_damageTip);
        cloneTip.transform.SetParent(SelfTransform, false);
        cloneTip.AddComponent<DamageTip>().Initial(target, content);
        cloneTip.SetActive(true); //调整顺序最后再打开
    }

    public void AddHealthBar(EnemyCtrl target)
    {
        GameObject cloneHealthBar = GameObject.Instantiate(_originHealthBar);
        cloneHealthBar.transform.SetParent(SelfTransform, false);
        cloneHealthBar.AddComponent<HealthBar>().Initial(target);
        cloneHealthBar.SetActive(true); //调整顺序最后再打开
    }
}
