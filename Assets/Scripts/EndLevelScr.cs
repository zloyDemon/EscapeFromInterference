using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLevelScr : BaseWindow
{
    [SerializeField] private Text labelText;
    [SerializeField] private Button continueButton;

    public override void Show()
    {
        base.Show();
        labelText.text = FormatStringLabel();
        continueButton.onClick.AddListener(ContinueButtonClick);
    }

    public override void Close()
    {
        base.Close();
        continueButton.onClick.RemoveListener(ContinueButtonClick);
    }

    public void LogOpen()
    {
        Debug.Log($"EndLevelOpen");
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
                
                Close();
                UIManager.Instance.ShowHideLoadingScreen(true);
                GameItems.Instance.ResetItems();
            }

            if (type == EFIEnums.FadeType.FadeOut)
            {
                SceneManager.LoadScene(1);
            }
        });
    }
}
