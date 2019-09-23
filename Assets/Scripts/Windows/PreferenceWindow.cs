using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreferenceWindow : BaseWindow
{
    [SerializeField] Button closeButton;
    [SerializeField] Button savePreferencesButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(Close);
    }
}
