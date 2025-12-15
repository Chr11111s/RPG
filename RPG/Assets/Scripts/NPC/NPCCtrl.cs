using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCtrl : MonoBehaviour
{
    [SerializeField]
    int _npcID;

    NPCInfo _npcInfo;
    JobInfo _currentJob;

    public void Initial()
    {
        _npcInfo = DataManager.Instance.GetNewNPC(_npcID);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (_npcInfo.CurrentJobID == -1)
        {
            return;
        }

        if (_currentJob == null)
        {
            _currentJob = DataManager.Instance.GetNewJob(_npcInfo.CurrentJobID);
        }

        WndManager.Instance.OpenWnd<DialogWnd>();
        WndManager.Instance.GetWnd<DialogWnd>().ShowJobInfo(_npcInfo, _currentJob);
        WndManager.Instance.GetWnd<MouseWnd>().Show();
    }
}
