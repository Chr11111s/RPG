using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool Learn { get; private set; }
    SkillSlot[] _preSkillSlots;
    Image _skillImage;
    SkillInfo _skillInfo;

    public void Initial(SkillSlot[] preSkillSlots, SkillInfo skillInfo)
    {
        _preSkillSlots = preSkillSlots;
        _skillImage = transform.Find("Image").GetComponent<Image>();
        _skillImage.sprite = skillInfo.SkillSp;
        _skillInfo = skillInfo;
    }

    public void LearnSkill()
    {
        Learn = true;
        _skillImage.color = Color.white;
        DataManager.Instance.PlayerInfo.skillpoint--;
        WndManager.Instance.GetWnd<SkillWnd>().UpdatePlayerInfo();
        DataManager.Instance.PlayerInfo.LearnSkill(_skillInfo.skillid);
        WndManager.Instance.GetWnd<BattleWnd>().SetHotSlot(_skillInfo.skillid, _skillInfo);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (DataManager.Instance.PlayerInfo.skillpoint == 0)
        {
            return;
        }


        if(eventData.button == PointerEventData.InputButton.Right)
        {
            bool canLearn = true;
            if(_preSkillSlots != null)
            {
                for(int i = 0; i < _preSkillSlots.Length; i++)
                {
                    if (!_preSkillSlots[i].Learn)
                    {
                        canLearn = false;
                        break;
                    }
                }
            }
            if(canLearn)
            {
                LearnSkill();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        WndManager.Instance.GetWnd<SkillWnd>().ShowSkillInfo(_skillInfo, transform.position + Vector3.right * 200);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        WndManager.Instance.GetWnd<SkillWnd>().HideSkillInfo();
    }
}
