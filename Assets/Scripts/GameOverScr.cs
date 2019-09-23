using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScr : BaseWindow
{
    [SerializeField] private Text gameOverText;
    [SerializeField] private CanvasGroup textCanvasGroup;
    [SerializeField] private Button retryPlay;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        retryPlay.onClick.AddListener(RetryGameClick);
        exitButton.onClick.AddListener(ExitGameClick);
    }

    public void Init()
    {
        gameObject.SetActive(true);
        HideBeforeShow();
        UIManager.Instance.FadeIn(textCanvasGroup, 1.5f, () =>
        {
            StartCoroutine(CorInitGameIverScreen());
        });
        
    }

    private void HideBeforeShow()
    {
        textCanvasGroup.alpha = 0;
        retryPlay.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }

    private IEnumerator CorInitGameIverScreen()
    { 
        retryPlay.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        yield break;
    }

    private void RetryGameClick()
    {
        UIManager.Instance.BlackScrFadeIn(2f, () =>
        {
            Close();
            SceneManager.LoadScene(1);
        });
    }

    private void ExitGameClick()
    {
        GameplayManager.Instance.ReturnToMenu(Close);
    }
}
