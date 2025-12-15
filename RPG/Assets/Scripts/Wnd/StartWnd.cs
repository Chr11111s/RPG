using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StartWnd : BaseWnd
{
    public override void Initial()
    {
        Button startBtn = SelfTransform.Find("StartBtn").GetComponent<Button>();
        Button exitBtn = SelfTransform.Find("ExitBtn").GetComponent<Button>();



        startBtn.onClick.AddListener(OnStartClick);
        exitBtn.onClick.AddListener(OnExitClick);
    }

    private void OnExitClick()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;

#elif UNITY_STANDALONE
    Application.Quit();

#endif
    }

    private void OnStartClick() //代码优化：删掉private void OnStartClick()下的①WndManager.Instance.OpenWnd<BattleWnd>();②PlayerCtrl.Instance.Initial();这两句挪到GameManager.cs中去写。
    {
        CloseWnd();
        //GameManager.Instance.StratGame(); 不能这么写了
        WndManager.Instance.OpenWnd<InfoWnd>();
        WndManager.Instance.GetWnd<MouseWnd>().Show();
    }
}
