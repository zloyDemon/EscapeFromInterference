﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] CanvasGroup fadeImage;
    [SerializeField] RectTransform loadingScreen; 

    public static UIManager Instance { get; private set; }

    public GameUI GameUI { get; set; }
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (Instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        fadeImage.gameObject.SetActive(true);
    }

    //TODO Временное решение
    public void SetGameUI(GameUI gameUI)
    {
        if (GameUI == null)
            GameUI = gameUI;
    }
    //TODO Временное решение
    public void ShowHideLoadingScreen(bool isShow)
    {
        loadingScreen.gameObject.SetActive(isShow);
    }

    public void BlackScrFadeOut(float duration, Action onComplete)
    {
        FadeOut(fadeImage, duration, onComplete);
    }

    public void BlackScrFadeIn(float duration, Action onComplete)
    {
        FadeIn(fadeImage, duration, onComplete);
    }

    public void FadeOut(CanvasGroup canvasGroup, float duration, Action onComplete)
    {
        StartCoroutine(Fade(canvasGroup, true, duration, onComplete));
    }

    public void FadeIn(CanvasGroup canvasGroup, float duration, Action onComplete)
    {
        StartCoroutine(Fade(canvasGroup, false, duration, onComplete));
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, bool isFadeOut, float duration, Action onComplete)
    {
        float finalValue = isFadeOut ? 0 : 1;
        float alpha = canvasGroup.alpha;
        float value = isFadeOut ? -alpha : alpha;
        alpha = value;
        
        while(value < finalValue)
        {
            value += duration * Time.deltaTime;
            alpha = value < 0 ? -value : value;
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = finalValue;
        onComplete();
    }
}
