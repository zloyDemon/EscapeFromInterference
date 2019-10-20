using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScale : MonoBehaviour
{
    private void Awake()
    {
        var scaler = GetComponent<CanvasScaler>();
        scaler.scaleFactor = GetCanvasScaler();
    }
    
    private int GetCanvasScaler()
    {
        int height = Screen.height;
        int minHudHeight = 200;

        if (600 <= height && height < 640)
            minHudHeight = 300;

        int zoom;
        if (height >= minHudHeight)
            zoom = height / minHudHeight;
        else
            zoom = 1;
        return zoom;
    }
}
