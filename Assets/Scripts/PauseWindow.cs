using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseWindow : BaseWindow
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        continueButton.onClick.AddListener(ContinueButtonClick);
    }

    private void ContinueButtonClick()
    {
        Close();
    }
}
