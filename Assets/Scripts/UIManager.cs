using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public const float FadeDurationGeneral = 0.8f; 
    
    [SerializeField] CanvasGroup fadeImage;
    [SerializeField] RectTransform loadingScreen;
    [SerializeField] RectTransform endLevelScreen;

    private Coroutine corFadeBS;
    private Coroutine corFadeElement;

    public static UIManager Instance { get; private set; }

    public GameUI GameUI { get; set; }
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (Instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        
        fadeImage.alpha = 1;
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

    //TODO Временное решение
    public void ShowEndLevelScreen(bool isShow)
    {
        endLevelScreen.gameObject.SetActive(isShow);
    }

    public void BlackScrFadeOut(float duration, Action onComplete)
    {
        FadeOut(fadeImage, duration, onComplete);
    }

    public void BlackScrFadeIn(float duration, Action onComplete)
    {
        FadeIn(fadeImage, duration, onComplete);
    }
    
    public void BlackScrFadeInOut(float duration, Action<EFIEnums.FadeType> onComplete)
    {
        FadeInOut(fadeImage, duration, onComplete);
    }

    public void FadeOut(CanvasGroup canvasGroup, float duration, Action onComplete)
    {
        StartFadeCoroutine(canvasGroup, true, duration, onComplete);
    }

    public void FadeIn(CanvasGroup canvasGroup, float duration, Action onComplete)
    {
        StartFadeCoroutine(canvasGroup, false, duration, onComplete);
    }

    public void FadeInOut(CanvasGroup canvasGroup, float duration, Action<EFIEnums.FadeType> onComplete)
    {
        FadeIn(canvasGroup, duration, () =>
        {
            onComplete(EFIEnums.FadeType.FadeIn);
            FadeOut(canvasGroup, duration, () => onComplete(EFIEnums.FadeType.FadeOut));
        });
    }

    private void StartFadeCoroutine(CanvasGroup canvasGroup, bool isFadeOut, float duration, Action onComplete)
    {
        if(corFadeBS != null)
        {
            StopCoroutine(corFadeBS);
            corFadeBS = null;
        }

        corFadeBS = StartCoroutine(Fade(canvasGroup, isFadeOut, duration, onComplete));
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, bool isFadeOut, float duration, Action onComplete)
    {
        const float FadeMultiplier = 0.01f;
        float finalValue = isFadeOut ? 0 : 1;
        float alpha = canvasGroup.alpha;
        float value = isFadeOut ? -alpha : alpha;
        
        while(value < finalValue)
        {
            value += duration * FadeMultiplier;
            alpha = value < 0 ? -value : value;
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = finalValue;
        corFadeBS = null;
        onComplete?.Invoke();
    }
}
