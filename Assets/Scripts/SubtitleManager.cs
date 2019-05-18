using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    private const float subShowSpeed = 8f;
    
    private Coroutine subCoroutine;
    
    public static SubtitleManager Instance { get; private set; }

    public event Action<string> SubtitleSet = s => { };
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        if(Instance != this)
            Destroy(gameObject);
    }

    public void SetSubtitle(string text, string name = "")
    {
        if(subCoroutine != null)
            StopCoroutine(subCoroutine);
        subCoroutine = StartCoroutine(ShowSubtitle(text));
    }

    private IEnumerator ShowSubtitle(string text)
    {
        float time = text.Length / subShowSpeed;
        SubtitleSet(text);
        yield return new WaitForSeconds(time);
        SubtitleSet(string.Empty);
    }
}
