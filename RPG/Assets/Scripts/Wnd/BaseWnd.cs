using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWnd
{
    protected Transform SelfTransform { get; private set; }


    public void CreateWnd(string wndName, Transform canvas)
    {
        GameObject originWnd = Resources.Load<GameObject>("Wnd/" + wndName);
        GameObject cloneWnd = GameObject.Instantiate(originWnd);
        SelfTransform = cloneWnd.transform;
        SelfTransform.SetParent(canvas, false); //它是UGUI的所以所有东西都必须在canvas底下
    }

    public abstract void Initial();

    public void OpenWnd()
    {
        SelfTransform.gameObject.SetActive(true);
        OnOpenWnd();
    }

    public void CloseWnd()
    {
        SelfTransform.gameObject.SetActive(false);
        OnCloseWnd();
    }

    protected virtual void OnOpenWnd()
    {

    }

    protected virtual void OnCloseWnd()
    {

    }
}
