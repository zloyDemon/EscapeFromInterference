using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLevelScr : MonoBehaviour
{
    [SerializeField] private Text labelText;
    [SerializeField] private Button continueButton;

    private void Awake()
    {
        continueButton.onClick.AddListener(ContinueButtonClick);
    }

    private void OnEnable()
    {
        labelText.text = FormatStringLabel();
    }

    private string FormatStringLabel()
    {
        string result = string.Empty;
        result = string.Format("Level {0} comleted.", MissionManager.Instance.CurrentMission.Level);
        return result;
    }

    private void ContinueButtonClick()
    {
        MissionManager.Instance.ChangeOnNextMission();
        
        UIManager.Instance.BlackScrFadeInOut(1f, type =>
        {
            if (type == EFIEnums.FadeType.FadeIn)
            {
                UIManager.Instance.ShowHideLoadingScreen(true);
                UIManager.Instance.ShowEndLevelScreen(false);
                GameItems.Instance.ResetItems();
            }

            if (type == EFIEnums.FadeType.FadeOut)
            {
                SceneManager.LoadScene(1);
            }
        });
    }
}
