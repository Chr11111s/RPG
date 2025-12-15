using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JobWnd : BaseWnd
{
    GameObject _originSlot;
    Transform _content;
    TMP_Text _infoTitle;
    TMP_Text _infoDesc;

    public override void Initial()
    {
        _content = SelfTransform.Find("BG/JobList/Viewport/Content");
        _originSlot = _content.Find("Slot").gameObject;
        _infoTitle = SelfTransform.Find("BG/Info/Title").GetComponent<TMP_Text>();
        _infoDesc = SelfTransform.Find("BG/Info/Desc").GetComponent<TMP_Text>();
        Button closeBtn = SelfTransform.Find("BG/Close").GetComponent<Button>();
        closeBtn.onClick.AddListener(OnCloseClick);
    }

    private void OnCloseClick()
    {
        CloseWnd();
    }

    public void AddJob(JobInfo jobInfo)
    {
        GameObject cloneSlot = GameObject.Instantiate(_originSlot);
        cloneSlot.transform.SetParent(_content, false);
        cloneSlot.gameObject.AddComponent<JobSlot>().Initial(jobInfo);
        cloneSlot.SetActive(true);
    }

    public void ShowJobInfo(JobInfo jobInfo)
    {
        _infoTitle.text = jobInfo.name;
        _infoDesc.text = jobInfo.desc;
    }
}
