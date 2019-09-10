using System;
using UnityEngine;

public class BaseWindow : MonoBehaviour
{
    public bool IsShowing { get; private set; }

    public event Action<BaseWindow> OnWindowOpen = bw => { }; 
    public event Action<BaseWindow> OnWindowClosed = bw => { }; 

    public virtual void Show()
    {
        gameObject.SetActive(true);
        IsShowing = true;
        OnWindowOpen(this);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
        IsShowing = false;
        OnWindowClosed(this);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        IsShowing = false;
    }
}
