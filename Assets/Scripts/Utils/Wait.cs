using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Wait
{
    private static WaitForEndOfFrame m_WaitForEndOfFrame = new WaitForEndOfFrame();
    private static WaitForFixedUpdate m_WaitForFixedUpdate = new WaitForFixedUpdate();
    public static WaitForEndOfFrame ForEndOfFrame
    {
        get { return m_WaitForEndOfFrame; }
    }
    public static WaitForFixedUpdate ForFixedUpdate
    {
        get { return m_WaitForFixedUpdate; }
    }
}