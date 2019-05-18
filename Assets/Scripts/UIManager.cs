using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] CanvasGroup fadeImage;
    public static UIManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.LogError("Instance UIManger");
        }
            
        if (Instance != this)
        {
            Destroy(gameObject);
            Debug.LogError("Destroy UIManger");
        }
            
        
        DontDestroyOnLoad(gameObject);
        fadeImage.gameObject.SetActive(true);
    }

    public void FadeOut(float duration, Action onComplete)
    {
        StartCoroutine(CoFadeOut(duration, onComplete));
    }

    public void FadeIn(float duration, Action onComplete)
    {
        StartCoroutine(CoFadeIn(duration, onComplete));
    }

    private IEnumerator CoFadeOut(float duration, Action onComplete)
    {

        for (float i = 1; i > 0; i -= Time.deltaTime * duration)
        {
            fadeImage.alpha = i;
            Debug.Log("Value: " + i);
            yield return null;
        }

        fadeImage.alpha = 0;
        onComplete();
    }

    private IEnumerator CoFadeIn(float duration, Action onComplete)
    {
        fadeImage.GetComponent<Image>().raycastTarget = true;
        for (float i = 0; i < 1; i += duration * Time.deltaTime)
        {
            fadeImage.alpha = i;
            yield return null;
        }

        fadeImage.alpha = 1;
        fadeImage.GetComponent<Image>().raycastTarget = false;
        onComplete();
    }
}
