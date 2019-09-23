using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseWindow : BaseWindow
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        continueButton.onClick.AddListener(ContinueButtonClick);
        exitButton.onClick.AddListener(OnExitClick);
    }

    private void ContinueButtonClick()
    {
        Close();
    }

    private void OnExitClick()
    {
        GameplayManager.Instance.ReturnToMenu(Close);
    }
}
