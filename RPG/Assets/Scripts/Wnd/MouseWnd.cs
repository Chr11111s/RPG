using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWnd : BaseWnd
{
    RPGMouseCursor _cursor;
    public override void Initial()
    {
        Transform cursor = SelfTransform.Find("Cursor");
        _cursor = cursor.gameObject.AddComponent<RPGMouseCursor>();
    }

    public void SwitchToNormal()
    {
        _cursor.SwitchToNormal();
    }

    public void SwitchToSelect()
    {
        _cursor.SwitchToSelect();
    }

    public void Show() //解决面板打开后鼠标移动被遮挡问题
    {
        SelfTransform.SetAsLastSibling();
    }
}
