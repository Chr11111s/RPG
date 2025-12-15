using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class DialogWnd : BaseWnd
{
    TMP_Text _name;
    TMP_Text _desc;
    NPCInfo _npcInfo;
    JobInfo _jobInfo;

    GameObject _accept, _deny, _yes, _no, _bye;

    public override void Initial()
    {
        Button closeBtn = SelfTransform.Find("BG/Close").GetComponent<Button>();
        closeBtn.onClick.AddListener(OnCloseClick);
        Button acceptBtn = SelfTransform.Find("BG/Accept").GetComponent<Button>();
        acceptBtn.onClick.AddListener(OnAcceptClick);
        _accept = acceptBtn.gameObject;
        Button denyBtn = SelfTransform.Find("BG/Deny").GetComponent<Button>();
        denyBtn.onClick.AddListener(OnDenyClick);
        _deny = denyBtn.gameObject;
        Button yesBtn = SelfTransform.Find("BG/Yes").GetComponent<Button>();
        yesBtn.onClick.AddListener(OnYesClick);
        _yes = yesBtn.gameObject;
        Button noBtn = SelfTransform.Find("BG/No").GetComponent<Button>();
        noBtn.onClick.AddListener(OnNoClick);
        _no = noBtn.gameObject;
        Button byeBtn = SelfTransform.Find("BG/Bye").GetComponent<Button>();
        byeBtn.onClick.AddListener(OnByeClick);
        _bye = byeBtn.gameObject;

        _name = SelfTransform.Find("BG/Name/Tip").GetComponent<TMP_Text>();
        _desc = SelfTransform.Find("BG/Desc").GetComponent<TMP_Text>();
    }

    private void OnByeClick()
    {
        CloseWnd();
    }

    private void OnNoClick()
    {
        CloseWnd();
    }

    private void OnYesClick()
    {
        /*Debug.Log($"IsFinish={_jobInfo.IsFinish}, state={_jobInfo.JobState}");
        Debug.Log($"targetcount={string.Join(",", _jobInfo.targetcount)}");
        Debug.Log($"finishcount={string.Join(",", _jobInfo.finishcount)}");*/
        if (_jobInfo.IsFinish)
        {
            _jobInfo.SwitchState(JobStateEnum.已完成);
            DataManager.Instance.PlayerInfo.EarnExp(_jobInfo.exp);
            ShowJobInfo(_npcInfo, _jobInfo);
        }
        /*else
        {
            Debug.Log("任务未完成，未刷新对话");
        }*/
    }

    public void ShowJobInfo(NPCInfo npcInfo, JobInfo jobInfo)
    {
        _npcInfo = npcInfo;
        _jobInfo = jobInfo;

        _name.text = _npcInfo.name;
        //_desc.text = _jobInfo.desc;
        switch (jobInfo.JobState)
        {
            case JobStateEnum.未领取:
                {
                    _desc.text = _jobInfo.desc;
                    _accept.gameObject.SetActive(true);
                    _deny.gameObject.SetActive(true);
                    _yes.gameObject.SetActive(false);
                    _no.gameObject.SetActive(false);
                    _bye.gameObject.SetActive(false);
                }
                break;
            case JobStateEnum.已领取:
                {
                    _desc.text = _npcInfo.common[0];
                    _accept.gameObject.SetActive(false);
                    _deny.gameObject.SetActive(false);
                    _yes.gameObject.SetActive(true);
                    _no.gameObject.SetActive(true);
                    _bye.gameObject.SetActive(false);

                }
                break;
            case JobStateEnum.已完成:
                {
                    _desc.text = _npcInfo.common[1];
                    _accept.gameObject.SetActive(false);
                    _deny.gameObject.SetActive(false);
                    _yes.gameObject.SetActive(false);
                    _no.gameObject.SetActive(false);
                    _bye.gameObject.SetActive(true);
                }
                break;
        }
    }

    private void OnDenyClick()
    {
        CloseWnd();
    }

    private void OnAcceptClick()
    {
        WndManager.Instance.GetWnd<JobWnd>().AddJob(_jobInfo);
        _jobInfo.SwitchState(JobStateEnum.已领取);
        CloseWnd();
    }

    private void OnCloseClick()
    {
        CloseWnd();
    }
}
