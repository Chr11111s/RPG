using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum DragSortEnum
{
    在背包内,
    到装备栏,
    到快捷栏
}

public class InventoryWnd : BaseWnd
{
    List<InventorySlot> _allSlots;
    Transform _info;
    TMP_Text _infoTitle;
    TMP_Text _infoDesc;
    Image _workItem;
    ItemInfo _currentItemInfo;
    InventorySlot _currentSlot;
    InventorySlot _originSlot;
    DragSortEnum _currentDragSort;


    public override void Initial()
    {
        SelfTransform.Find("BG").gameObject.AddComponent<WndMove>();
        Button closeBtn = SelfTransform.Find("BG/Close").GetComponent<Button>();
        closeBtn.onClick.AddListener(OnCloseClick);

        Transform slotRoot = SelfTransform.Find("BG/Slots");
        _allSlots = new List<InventorySlot>();
        for (int i = 0; i < slotRoot.childCount; i++)
        {
            GameObject tmpSlot = slotRoot.GetChild(i).gameObject;
            InventorySlot inventorySlot = tmpSlot.AddComponent<InventorySlot>();
            inventorySlot.Initial(i);
            _allSlots.Add(inventorySlot);
        }
        _info = SelfTransform.Find("BG/Info");
        _infoTitle = _info.Find("Title").GetComponent<TMP_Text>();
        _infoDesc = _info.Find("Desc").GetComponent<TMP_Text>();

        _workItem = SelfTransform.Find("BG/WorkItem").GetComponent<Image>();
    }

    public void PickUp(ItemInfo itemInfo)
    {
        for (int i = 0; i < _allSlots.Count; i++)
        {
            if (_allSlots[i].HasItem && _allSlots[i].ItemCanAdd && _allSlots[i].SelfItem.itemid == itemInfo.itemid)
            {
                _allSlots[i].AddItem(itemInfo.itemcount);
                break;
            }
            else if (!_allSlots[i].HasItem)
            {
                _allSlots[i].SetItem(itemInfo);
                break;
            }
        }
    }

    public void SetWorkItem(InventorySlot originSlot, ItemInfo itemInfo, Sprite itemSp)
    {
        SelfTransform.SetAsLastSibling(); //又让它出现了
        WndManager.Instance.GetWnd<MouseWnd>().Show();
        _originSlot = originSlot;
        //_currentItemInfo = itemInfo;
        _currentItemInfo = new ItemInfo(itemInfo);
        _workItem.gameObject.SetActive(true);
        _workItem.sprite = itemSp;
    }

    public void SetWorkItemPos(Vector3 pos)
    {
        _workItem.transform.position = pos;
    }

    public void SetCurrentSlot(InventorySlot slot)
    {
        _currentSlot = slot;
    }

    public void SwitchDragSort(DragSortEnum dragSort)
    {
        _currentDragSort = dragSort;
    }

    /*public void ClearCurrentSlot()
    {
        _currentSlot = null;
    }*/

    public void SetEquipItem(ItemInfo itemInfo)
    {
        _currentSlot.SetItem(itemInfo);
    }

    public void EndDragItem()
    {
        switch (_currentDragSort)
        {
            case DragSortEnum.在背包内:
                {

                    if (_currentSlot != _originSlot)
                    {
                        if (_currentSlot.HasItem)
                        {
                            _originSlot.SetItem(_currentSlot.SelfItem);
                            _currentSlot.SetItem(_currentItemInfo);

                        }
                        else
                        {
                            _currentSlot.SetItem(_currentItemInfo);
                            _originSlot.Clear();
                        }
                    }

                }
                break;
            case DragSortEnum.到装备栏:
                bool flag = WndManager.Instance.GetWnd<EquipWnd>().CanSetSlotItem(_currentItemInfo);
                if (flag)
                {
                    WndManager.Instance.GetWnd<EquipWnd>().SetSlotItem(_currentItemInfo);
                    _originSlot.Clear();
                }
                break;
            case DragSortEnum.到快捷栏:
                {
                    WndManager.Instance.GetWnd<BattleWnd>().SetHotSlot(_currentItemInfo);
                }
                break;
        }
        /*if( _currentSlot != null )
        {
            
        }
        else
        {
            
        }*/
        _workItem.gameObject.SetActive(false);
    }

    public void ShowItemInfo(ItemInfo itemInfo, Vector3 pos)
    {
        _info.position = pos;
        _infoTitle.text = itemInfo.itemname;
        _infoDesc.text = itemInfo.itemdesc;
        _info.gameObject.SetActive(true);
    }

    internal void HideItemInfo()
    {
        _info.gameObject.SetActive(false);
    }

    private void OnCloseClick()
    {
        CloseWnd();
    }

    public bool CanSetEquip()
    {
        bool canSet = false;

        if (_currentSlot != null)
        {
            if (!_currentSlot.HasItem)
            {
                canSet = true;
            }
        }
        return canSet;
    }
}
