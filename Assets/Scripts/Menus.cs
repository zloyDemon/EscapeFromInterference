using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Menus : MonoBehaviour
{
    [SerializeField] private RectTransform menuParent;
    [SerializeField] private List<BaseWindow> menus;

    private List<BaseWindow> listWindows = new List<BaseWindow>();
    private Stack<BaseWindow> windowsStack = new Stack<BaseWindow>();

    public BaseWindow CurrentOpenedWindow { get; private set; }
    public Stack<BaseWindow> WindowsStack => windowsStack;
    
    private void Awake()
    {
        StartCoroutine(LoadMenus());
    }

    private T GetMenuType<T>() where T : BaseWindow
    {
       var windowType = (T)listWindows.First(m => m.GetType() == typeof(T));
       return windowType;
    }

    public T Show<T>() where T : BaseWindow
    {
        var menu = GetMenuType<T>();
        CurrentOpenedWindow = menu;
        CurrentOpenedWindow.OnWindowClosed += OnWindowClosed;
        if (windowsStack.Count > 0)
        {
            var window = windowsStack.Peek();
            window.Hide();
        }
        windowsStack.Push(menu);
        LogStack();
        menu.transform.SetSiblingIndex(menus.Count - 1);
        menu.Show();
        return menu;
    }

    public void CloseCurrentWindow()
    {
        if(windowsStack.Count == 0)
            throw new Exception("Windows stack is empty");
                
        CurrentOpenedWindow.OnWindowClosed -= OnWindowClosed;
        var menu = windowsStack.Pop();
        menu.transform.SetSiblingIndex(0);
        menu.Close();
        
        if (windowsStack.Count > 0)
        {
            var window = windowsStack.Peek();
            CurrentOpenedWindow = window;
            CurrentOpenedWindow.Show();
        }
        
        LogStack();
    }

    private void OnWindowClosed(BaseWindow closedWindow)
    {
        CloseCurrentWindow();
    }

    private void LogStack()
    {
        Debug.Log($"Windows Stack Count: {windowsStack.Count}");
    }

    private IEnumerator LoadMenus()
    {
        foreach (var menu in menus)
        {
            var newMenu = Instantiate(menu, menuParent.position, Quaternion.identity, menuParent);
            listWindows.Add(newMenu);
        }

        yield return new WaitForEndOfFrame();
    }
}
