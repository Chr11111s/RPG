using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JobSlot : MonoBehaviour
{
    Toggle _selfToggle;
    JobInfo _jobInfo;
    TMP_Text _tip;

    public void Initial(JobInfo jobInfo)
    {
        _jobInfo = jobInfo;
        _selfToggle = GetComponent<Toggle>();
        _selfToggle.onValueChanged.AddListener(OnCheck);
        _tip = transform.Find("Tip").GetComponent<TMP_Text>();
        _tip.text = jobInfo.name;
    }

    private void OnCheck(bool arg0)
    {
        if (arg0) //true
        {
            WndManager.Instance.GetWnd<JobWnd>().ShowJobInfo(_jobInfo);
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
}
