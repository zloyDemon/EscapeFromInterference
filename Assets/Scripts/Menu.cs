using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button newGameBtn;

    private void Awake()
    {
        newGameBtn.onClick.AddListener(NewGameClick);
        UIManager.Instance.BlackScrFadeOut(0.5f, () => { });
    }

    private void NewGameClick()
    {
        UIManager.Instance.BlackScrFadeIn(0.8f, () =>
        {
            UIManager.Instance.ShowHideLoadingScreen(true);
            SceneManager.LoadScene(1);
        });
    }
}
