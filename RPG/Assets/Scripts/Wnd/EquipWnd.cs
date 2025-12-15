using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EquipWnd : BaseWnd
{
    Transform _info;
    TMP_Text _infoTitle;
    TMP_Text _infoDesc;
    EquipSlot _currentSlot;
    Dictionary<EquipSortEnum, EquipSlot> _allEquipSlots;
    Image _workItem;
    EquipSlot _originSlot;
    ItemInfo _currentItemInfo;

    public override void Initial()
    {
        SelfTransform.Find("BG").gameObject.AddComponent<WndMove>();

        _workItem = SelfTransform.Find("BG/WorkItem").GetComponent<Image>();

        Button closeBtn = SelfTransform.Find("BG/Close").GetComponent<Button>();
        closeBtn.onClick.AddListener(OnCloseClick);
        _info = SelfTransform.Find("BG/Info");
        _infoTitle = _info.Find("Title").GetComponent<TMP_Text>();
        _infoDesc = _info.Find("Desc").GetComponent<TMP_Text>();

        _allEquipSlots = new Dictionary<EquipSortEnum, EquipSlot>();
        Transform head = SelfTransform.Find("BG/Head");
        EquipSlot equip = head.gameObject.AddComponent<EquipSlot>();
        equip.Initial(EquipSortEnum.头);
        _allEquipSlots.Add(EquipSortEnum.头, equip);
        Transform LG = SelfTransform.Find("BG/LG");
        equip = LG.gameObject.AddComponent<EquipSlot>();
        equip.Initial(EquipSortEnum.左手手套);
        _allEquipSlots.Add(EquipSortEnum.左手手套, equip);
        Transform RG = SelfTransform.Find("BG/RG");
        equip = RG.gameObject.AddComponent<EquipSlot>();
        equip.Initial(EquipSortEnum.右手手套);
        _allEquipSlots.Add(EquipSortEnum.右手手套, equip);
        Transform cloth = SelfTransform.Find("BG/Cloth");
        equip = cloth.gameObject.AddComponent<EquipSlot>();
        equip.Initial(EquipSortEnum.衣服);
        _allEquipSlots.Add(EquipSortEnum.衣服, equip);
        Transform lShoe = SelfTransform.Find("BG/LShoe");
        equip = lShoe.gameObject.AddComponent<EquipSlot>();
        equip.Initial(EquipSortEnum.左鞋子);
        _allEquipSlots.Add(EquipSortEnum.左鞋子, equip);
        Transform rShoe = SelfTransform.Find("BG/RShoe");
        equip = rShoe.gameObject.AddComponent<EquipSlot>();
        equip.Initial(EquipSortEnum.右鞋子);
        _allEquipSlots.Add(EquipSortEnum.右鞋子, equip);
        Transform lEaring = SelfTransform.Find("BG/LEaring");
        equip = lEaring.gameObject.AddComponent<EquipSlot>();
        equip.Initial(EquipSortEnum.左耳环);
        _allEquipSlots.Add(EquipSortEnum.左耳环, equip);
        Transform rEaring = SelfTransform.Find("BG/REaring");
        equip = rEaring.gameObject.AddComponent<EquipSlot>();
        equip.Initial(EquipSortEnum.右耳环);
        _allEquipSlots.Add(EquipSortEnum.右耳环, equip);
        Transform lRing = SelfTransform.Find("BG/LRing");
        equip = lRing.gameObject.AddComponent<EquipSlot>();
        equip.Initial(EquipSortEnum.左手戒指);
        _allEquipSlots.Add(EquipSortEnum.左手戒指, equip);
        Transform rRing = SelfTransform.Find("BG/RRing");
        equip = rRing.gameObject.AddComponent<EquipSlot>();
        equip.Initial(EquipSortEnum.右手戒指);
        _allEquipSlots.Add(EquipSortEnum.右手戒指, equip);
    }

    private void OnCloseClick()
    {
        CloseWnd();
    }

    public void SetCurrentSlot(EquipSlot slot)
    {
        _currentSlot = slot;
        WndManager.Instance.GetWnd<InventoryWnd>().SwitchDragSort(DragSortEnum.到装备栏); //替换掉原来的ClearCurrentSlot()
    }

    public bool CanSetSlotItem(ItemInfo itemInfo)
    {
        return _currentSlot.CanSetItem(itemInfo);
    }

    public void SetSlotItem(ItemInfo itemInfo)
    {
        switch (_currentSlot.EquipSort)
        {
            case EquipSortEnum.左手手套:
                _allEquipSlots[EquipSortEnum.右手手套].SetItem(itemInfo, false);
                break;
            case EquipSortEnum.右手手套:
                _allEquipSlots[EquipSortEnum.左手手套].SetItem(itemInfo, false);
                break;
            case EquipSortEnum.左鞋子:
                _allEquipSlots[EquipSortEnum.右鞋子].SetItem(itemInfo, false);
                break;
            case EquipSortEnum.右鞋子:
                _allEquipSlots[EquipSortEnum.左鞋子].SetItem(itemInfo, false);
                break;
            case EquipSortEnum.左耳环:
                _allEquipSlots[EquipSortEnum.右耳环].SetItem(itemInfo, false);
                break;
            case EquipSortEnum.右耳环:
                _allEquipSlots[EquipSortEnum.左耳环].SetItem(itemInfo, false);
                break;
        }
        _currentSlot.SetItem(itemInfo);
    }

    public void ShowItemInfo(ItemInfo itemInfo, Vector3 pos)
    {
        _infoTitle.text = itemInfo.itemname;
        _infoDesc.text = itemInfo.itemdesc;
        _info.gameObject.SetActive(true);
        _info.position = pos;
    }

    public void HideItemInfo()
    {
        _info.gameObject.SetActive(false);
    }

    public void SetWorkItem(EquipSlot equip, ItemInfo itemInfo)
    {
        SelfTransform.SetAsLastSibling();
        WndManager.Instance.GetWnd<MouseWnd>().Show();

        switch (equip.EquipSort) //防止从装备栏将左鞋子拖回背包右鞋子还留在装备栏
        {
            case EquipSortEnum.左手手套:
                _allEquipSlots[EquipSortEnum.右手手套].HideItem();
                break;
            case EquipSortEnum.右手手套:
                _allEquipSlots[EquipSortEnum.左手手套].HideItem();
                break;
            case EquipSortEnum.左鞋子:
                _allEquipSlots[EquipSortEnum.右鞋子].HideItem();
                break;
            case EquipSortEnum.右鞋子:
                _allEquipSlots[EquipSortEnum.左鞋子].HideItem();
                break;
            case EquipSortEnum.左耳环:
                _allEquipSlots[EquipSortEnum.右耳环].HideItem();
                break;
            case EquipSortEnum.右耳环:
                _allEquipSlots[EquipSortEnum.左耳环].HideItem();
                break;
        }

        _originSlot = equip;
        _workItem.gameObject.SetActive(true);
        _workItem.sprite = itemInfo.ItemSp;
        _currentItemInfo = itemInfo;
    }

    public void SetWorkItemPos(Vector3 pos)
    {
        _workItem.transform.position = pos;
    }

    public void EndDrag()
    {
        _workItem.gameObject.SetActive(false);
        switch (_originSlot.EquipSort)
        {
            case EquipSortEnum.左手手套:
                _allEquipSlots[EquipSortEnum.右手手套].ShowItem();
                break;
            case EquipSortEnum.右手手套:
                _allEquipSlots[EquipSortEnum.左手手套].ShowItem();
                break;
            case EquipSortEnum.左鞋子:
                _allEquipSlots[EquipSortEnum.右鞋子].ShowItem();
                break;
            case EquipSortEnum.右鞋子:
                _allEquipSlots[EquipSortEnum.左鞋子].ShowItem();
                break;
            case EquipSortEnum.左耳环:
                _allEquipSlots[EquipSortEnum.右耳环].ShowItem();
                break;
            case EquipSortEnum.右耳环:
                _allEquipSlots[EquipSortEnum.左耳环].ShowItem();
                break;
        }
        if (WndManager.Instance.GetWnd<InventoryWnd>().CanSetEquip())
        {
            switch (_originSlot.EquipSort)
            {
                case EquipSortEnum.左手手套:
                    _allEquipSlots[EquipSortEnum.右手手套].ClearItem(false);
                    break;
                case EquipSortEnum.右手手套:
                    _allEquipSlots[EquipSortEnum.左手手套].ClearItem(false);
                    break;
                case EquipSortEnum.左鞋子:
                    _allEquipSlots[EquipSortEnum.右鞋子].ClearItem(false);
                    break;
                case EquipSortEnum.右鞋子:
                    _allEquipSlots[EquipSortEnum.左鞋子].ClearItem(false);
                    break;
                case EquipSortEnum.左耳环:
                    _allEquipSlots[EquipSortEnum.右耳环].ClearItem(false);
                    break;
                case EquipSortEnum.右耳环:
                    _allEquipSlots[EquipSortEnum.左耳环].ClearItem(false);
                    break;
            }

            WndManager.Instance.GetWnd<InventoryWnd>().SetEquipItem(_currentItemInfo);
            _originSlot.ClearItem();
        }
    }
}
