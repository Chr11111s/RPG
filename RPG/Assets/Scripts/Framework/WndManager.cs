using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WndManager : Singleton<WndManager>
{
    Dictionary<string, BaseWnd> _allWnds;
    Transform _canvas; //WndManager是不挂在节点上的所以canvas必须从外部传进来

    public void Initial(Transform canvas)
    {
        _canvas = canvas;
        _allWnds = new Dictionary<string, BaseWnd>();
    }

    public void OpenWnd<T>() where T : BaseWnd, new()
    {
        string wndName = typeof(T).Name;
        if (_allWnds.ContainsKey(wndName))
        {
            _allWnds[wndName].OpenWnd();
        }
        else
        {
            T wnd = new T();
            wnd.CreateWnd(wndName, _canvas);
            wnd.Initial();
            _allWnds.Add(wndName, wnd);
        }
    }

    public void CloseWnd<T>() where T : BaseWnd, new()
    {
        string wndName = typeof(T).Name;
        if (_allWnds.ContainsKey(wndName))
        {
            _allWnds[wndName].CloseWnd();
        }
    }

    public T GetWnd<T>() where T : BaseWnd, new()
    {
        string wndName = typeof(T).Name;
        if (_allWnds.ContainsKey(wndName))
        {
            return _allWnds[wndName] as T;
        }
        else
        {
            OpenWnd<T>();
            CloseWnd<T>();
            return GetWnd<T>();
        }
    }
}
