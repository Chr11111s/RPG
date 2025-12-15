using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HotSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image _icon;
    Image _cd;
    TMP_Text _tip;
    SkillInfo _skillInfo;
    public ItemInfo SlotItemInfo { get; private set; }

    public void Initial(int index)
    {
        _icon = transform.Find("Icon").GetComponent<Image>();
        _cd = transform.Find("CD").GetComponent<Image>();
        _tip = transform.Find("Tip/Tip").GetComponent<TMP_Text>();

        if (index < 9) //因为是按1，2，3，...，9，0来算的
        {
            _tip.text = (index + 1).ToString();
        }
        else
        {
            _tip.text = "0";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        WndManager.Instance.GetWnd<BattleWnd>().SetCurrentSlot(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        WndManager.Instance.GetWnd<BattleWnd>().ClearCurrentSlot();
    }

    internal void SetItem(ItemInfo itemInfo)
    {
        _icon.sprite = itemInfo.ItemSp;
        _icon.color = Color.white;
        SlotItemInfo = itemInfo;
    }

    internal void SetSkill(SkillInfo skillInfo)
    {
        _icon.sprite = skillInfo.SkillSp;
        _cd.sprite = skillInfo.SkillSp;
        _icon.color = Color.white;
        _skillInfo = skillInfo;
    }

    public void Use()
    {
        if (SlotItemInfo == null)
        {
            return;
        }

        DataManager.Instance.PlayerInfo.UseItem(SlotItemInfo);

        if (SlotItemInfo.itemcount == 0)
        {
            SlotItemInfo = null;
            _icon.color = Color.clear;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_skillInfo == null)
        {
            return;
        }


        if (_skillInfo.IsCD)
        {
            _cd.fillAmount = 1 - _skillInfo.CDPercent;
        }
    }
}
