using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        DataManager.Instance.Initial();

        Cursor.visible = false;

        Transform canvas = transform.Find("Canvas");
        WndManager.Instance.Initial(canvas);
        WndManager.Instance.OpenWnd<StartWnd>();
        WndManager.Instance.OpenWnd<MouseWnd>();
    }

    public void StratGame()
    {
        EnemyManager.Instance.Initial();
        NPCManager.Instance.Initial();
        WndManager.Instance.OpenWnd<BattleWnd>();
        PlayerCtrl.Instance.Initial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
