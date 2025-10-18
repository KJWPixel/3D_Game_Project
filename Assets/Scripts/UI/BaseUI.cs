using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    public event Action<BaseUI> OnClosed;
    public virtual void Opne()
    {
        gameObject.SetActive(true);
        OnOpen();
    }

    public virtual void Close()
    {
        OnClose();
        gameObject.SetActive(false);
    }

    protected virtual void OnOpen() { }
    protected virtual void OnClose() { }
}
