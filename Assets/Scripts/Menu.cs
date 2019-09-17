using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button newGameBtn;
    [SerializeField] private Button preferenceBtn;

    private void Awake()
    {
        newGameBtn.onClick.AddListener(NewGameClick);
        preferenceBtn.onClick.AddListener(PrefOpen);
        UIManager.Instance.BlackScrFadeOut(2f, () => { });
    }

    private void NewGameClick()
    {
        UIManager.Instance.BlackScrFadeInOut(2f, type =>
        {
            if(type == EFIEnums.FadeType.FadeIn)
                UIManager.Instance.ShowHideLoadingScreen(true);
            if(type == EFIEnums.FadeType.FadeOut)
                SceneManager.LoadScene(1);
        });
    }

    private void PrefOpen()
    {
        UIManager.Instance.ShowMenu<PreferenceWindow>(m => {});
    }
}
