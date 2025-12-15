using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public bool HasItem
    {
        get
        {
            return SelfItem != null;
        }
    }

    public bool ItemCanAdd
    {
        get
        {
            return SelfItem.ItemSort == ItemSortEnum.消耗品;
        }
    }

    public ItemInfo SelfItem { get; private set; }

    int _selfIndex;
    Image _itemImage;
    TMP_Text _tip;
    

    public void Initial(int index)
    {
        _selfIndex = index;
        _itemImage = transform.Find("Image").GetComponent<Image>();
        _tip = transform.Find("Tip").GetComponent<TMP_Text>();
        Clear();
    }

    public void SetItem(ItemInfo itemInfo)
    {
        SelfItem = itemInfo;
        _tip.gameObject.SetActive(true);
        //Sprite itemSp = Resources.Load<Sprite>("Icon/" + itemInfo.itemspname);同理
        _itemImage.sprite = SelfItem.ItemSp;
        _itemImage.color = Color.white;
        if (itemInfo.itemsortindex == (int)ItemSortEnum.消耗品)
        {
            _tip.text = itemInfo.itemcount.ToString();
        }
        else
        {
            _tip.text = ""; //解决选中A与B交换时将A数量下标赋给B
        }
    }

    public void AddItem(int count)
    {
        SelfItem.itemcount += count;
        _tip.text = SelfItem.itemcount.ToString();
    }

    public void Clear()
    {
        _itemImage.color = Color.clear;
        _tip.text = "";
        SelfItem = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        WndManager.Instance.GetWnd<InventoryWnd>().SetCurrentSlot(this);
        WndManager.Instance.GetWnd<InventoryWnd>().SwitchDragSort(DragSortEnum.在背包内); // 重置拖拽类型为背包内部

        if (HasItem)
        {
            WndManager.Instance.GetWnd<InventoryWnd>().ShowItemInfo(SelfItem, transform.position + Vector3.right * 200);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (HasItem)
        {
            WndManager.Instance.GetWnd<InventoryWnd>().HideItemInfo();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (HasItem)
        {
            HideItem();
            WndManager.Instance.GetWnd<InventoryWnd>().SwitchDragSort(DragSortEnum.在背包内); // 确保从背包开始拖拽时类型正确
            WndManager.Instance.GetWnd<InventoryWnd>().SetWorkItem(this, SelfItem, _itemImage.sprite);
        }
        else return;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (HasItem)
        {
            WndManager.Instance.GetWnd<InventoryWnd>().SetWorkItemPos(eventData.position);
        }
        else return;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (HasItem)
        {
            ShowItem();
            WndManager.Instance.GetWnd<InventoryWnd>().EndDragItem();
        }
        else return;
    }

    void ShowItem()
    {
        _itemImage.gameObject.SetActive(true);
        _tip.gameObject.SetActive(true);
    }

    void HideItem()
    {
        _itemImage?.gameObject.SetActive(false);
        _tip.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!HasItem)
        {
            return;
        }


        if (SelfItem.ItemSort == ItemSortEnum.消耗品)
        {
            DataManager.Instance.PlayerInfo.UseItem(SelfItem);
            SelfItem.Use();
            if (SelfItem.itemcount == 0)
            {
                Clear();
            }
        }
    }
}
