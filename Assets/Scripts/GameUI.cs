using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private static readonly float shakeTime = 1f;
    private static readonly float lerpSpeed = 2f;
    private static readonly float FadeDuration = 0.5f;

    [SerializeField] BarFill batteryValue;
    [SerializeField] Text batteryCount;
    [SerializeField] Text keyCount;
    [SerializeField] Button changeBatteryBtn;
    [SerializeField] Text subtitleText;
    [SerializeField] CanvasGroup fadeImage;
    [SerializeField] Joystick joystick;
    [SerializeField] Button pauseButton;
    
    private static GameUI _instance;
    private bool isIndicate = false;
    private Coroutine indicationCo;
    private float currentValue = 0.0f;
    private float toValue = 1.0f;
    private bool isReset = false;
    private Coroutine corFade;
    private PauseWindow pauseWindow;
    
    public float CurrentIndicateValue { get; set; }
    public Joystick Joystick => joystick;
  

    private void Awake()
    {
        subtitleText.text = string.Empty;
        GameItems.Instance.BatteryCountChange += InstanceOnBatteryCountChange;
        GameItems.Instance.BatteryValueChange += InstanceOnBatteryValueChange;
        GameItems.Instance.KeyCountChange += InstanceOnKeyCountChange;
        changeBatteryBtn.onClick.AddListener(ChangeBatteryClick);
        pauseButton.onClick.AddListener(PauseGame);
        InstanceOnBatteryCountChange(GameItems.Instance.BatteryCount);
        InstanceOnKeyCountChange(0);
        UIManager.Instance.SetGameUI(this);
    }

    private void Start()
    {
        SubtitleManager.Instance.SubtitleSet += SetSubtitleText;
        GameplayManager.Instance.EndGame += EndGame;
        GameplayManager.Instance.PauseGame += PauseGame;
    }

    private void EndGame()
    {
        UIManager.Instance.ShowMenu<GameOverScr>(w => w.Init());
    }

    private void OnDestroy()
    {
        GameItems.Instance.BatteryCountChange -= InstanceOnBatteryCountChange;
        GameItems.Instance.BatteryValueChange -= InstanceOnBatteryValueChange;
        GameItems.Instance.KeyCountChange -= InstanceOnKeyCountChange;
        GameplayManager.Instance.EndGame -= EndGame;
        GameplayManager.Instance.PauseGame -= PauseGame;
        SubtitleManager.Instance.SubtitleSet -= SetSubtitleText;
        UIManager.Instance.SetGameUI(null);
    }
    
    private void InstanceOnBatteryCountChange(int value)
    {
        batteryCount.text = string.Format("{0}", value);
        changeBatteryBtn.interactable = value > 0;
    }
    
    private void InstanceOnBatteryValueChange(float value)
    {
        float res = value / Flashlight.FlashlighMaxValue;
        batteryValue.SetValue(res);
    }
    
    private void InstanceOnKeyCountChange(int value)
    {
        keyCount.text = string.Format("{0}", value);
    }

    private void PauseGame()
    {
        GameplayManager.Instance.IsGamePause = true;
        pauseWindow = UIManager.Instance.ShowMenu<PauseWindow>(m => {});
        pauseWindow.OnWindowClosed += OnPauseWindowClosed;
    }

    private void OnPauseWindowClosed(BaseWindow baseWindow)
    {
        Debug.Log("OnPausedClosed");
        if (pauseWindow != null)
        {
            pauseWindow.OnWindowClosed -= OnPauseWindowClosed;
            pauseWindow = null;
        }

        GameplayManager.Instance.IsGamePause = false;
    }

    private void ChangeBatteryClick()
    {
        Debug.Log("ChangeBatteryClick");
        GameItems.Instance.ResetFlashlight();   
    }

    private void SetSubtitleText(string text)
    {
        subtitleText.text = text;
    }

    private void PauseGame(bool isPause)
    {
        changeBatteryBtn.image.raycastTarget = !isPause;
    }
}
