using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public EquipSortEnum EquipSort { get; private set; }
    ItemInfo _selfItemInfo;
    Image _selfImage;
    bool HasItem
    {
        get
        {
            return _selfItemInfo != null;
        }
    }

    public void Initial(EquipSortEnum equipSort)
    {
        EquipSort = equipSort;
        _selfImage = transform.Find("Image").GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        WndManager.Instance.GetWnd<EquipWnd>().SetCurrentSlot(this);
        if (HasItem)
        {
            WndManager.Instance.GetWnd<EquipWnd>().ShowItemInfo(_selfItemInfo, transform.position + Vector3.right * 200);
        }
    }

    public void ClearItem(bool needUnUse = true)
    {
        _selfItemInfo = null;
        _selfImage.gameObject.SetActive(false);
    }

    public bool CanSetItem(ItemInfo itemInfo)
    {
        bool flag = false;
        switch (itemInfo.ItemSort)
        {
            case ItemSortEnum.消耗品:
                break;
            case ItemSortEnum.头:
                if (EquipSort == EquipSortEnum.头)
                {
                    flag = true;
                }
                break;
            case ItemSortEnum.手套:
                if (EquipSort == EquipSortEnum.左手手套 || EquipSort == EquipSortEnum.右手手套)
                {
                    flag = true;
                }
                break;
            case ItemSortEnum.衣服:
                if (EquipSort == EquipSortEnum.衣服)
                {
                    flag = true;
                }
                break;
            case ItemSortEnum.鞋子:
                if (EquipSort == EquipSortEnum.左鞋子 || EquipSort == EquipSortEnum.右鞋子)
                {
                    flag = true;
                }
                break;
            case ItemSortEnum.耳环:
                if (EquipSort == EquipSortEnum.左耳环 || EquipSort == EquipSortEnum.右耳环)
                {
                    flag = true;
                }
                break;
            case ItemSortEnum.戒指:
                if (EquipSort == EquipSortEnum.左手戒指 || EquipSort == EquipSortEnum.右手戒指)
                {
                    flag = true;
                }
                break;
        }
        return flag;
    }

    public void HideItem()
    {
        _selfImage.color = Color.clear;
    }

    public void ShowItem()
    {
        _selfImage.color = Color.white;
    }

    internal void SetItem(ItemInfo itemInfo, bool needUse = true)
    {
        _selfItemInfo = itemInfo;
        //Sprite sprite = Resources.Load<Sprite>("Icon/" + itemInfo.itemspname);挪到DataManager的ItemSp那里
        _selfImage.sprite = _selfItemInfo.ItemSp;
        _selfImage.gameObject.SetActive(true);

        if (needUse)
        {
            DataManager.Instance.PlayerInfo.UseItem(itemInfo);
            WndManager.Instance.GetWnd<CharacterWnd>().SyncPlayerInfo();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        WndManager.Instance.GetWnd<EquipWnd>().HideItemInfo();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!HasItem)
        {
            return;
        }
        HideItem();
        WndManager.Instance.GetWnd<EquipWnd>().SetWorkItem(this, _selfItemInfo);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!HasItem)
        {
            return;
        }
        WndManager.Instance.GetWnd<EquipWnd>().SetWorkItemPos(Input.mousePosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!HasItem)
        {
            return;
        }
        ShowItem();
        WndManager.Instance.GetWnd<EquipWnd>().EndDrag();
    }
}
