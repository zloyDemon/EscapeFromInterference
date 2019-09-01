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
    [SerializeField] GameOverScr gameOverScr;
    [SerializeField] Button pauseButton;
    
    private static GameUI _instance;
    private bool isIndicate = false;
    private Coroutine indicationCo;
    private float currentValue = 0.0f;
    private float toValue = 1.0f;
    private bool isReset = false;
    private Coroutine corFade;
    
    public float CurrentIndicateValue { get; set; }
    public Joystick Joystick => joystick;
  

    private void Awake()
    {
        UIManager.Instance.SetGameUI(this);
        subtitleText.text = string.Empty;
        GameItems.Instance.BatteryCountChange += InstanceOnBatteryCountChange;
        GameItems.Instance.BatteryValueChange += InstanceOnBatteryValueChange;
        GameItems.Instance.KeyCountChange += InstanceOnKeyCountChange;
        changeBatteryBtn.onClick.AddListener(ChangeBatteryClick);
        pauseButton.onClick.AddListener(PauseGame);
        InstanceOnBatteryCountChange(GameItems.Instance.BatteryCount);
        InstanceOnKeyCountChange(0);
    }

    private void Start()
    {
        SubtitleManager.Instance.SubtitleSet += SetSubtitleText;
        GameplayManager.Instance.EndGame += EndGame;
        Debug.Log("GameUI Start");
    }

    private void EndGame()
    {
        gameOverScr.Init();
    }

    private void OnDestroy()
    {
        GameItems.Instance.BatteryCountChange -= InstanceOnBatteryCountChange;
        GameItems.Instance.BatteryValueChange -= InstanceOnBatteryValueChange;
        GameItems.Instance.KeyCountChange -= InstanceOnKeyCountChange;
        GameplayManager.Instance.EndGame -= EndGame;
        SubtitleManager.Instance.SubtitleSet -= SetSubtitleText;
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

        if (batteryValue.Value < 0.3f)
            batteryValue.FillImage.color = Color.red;
    }
    
    private void InstanceOnKeyCountChange(int value)
    {
        keyCount.text = string.Format("{0}", value);
    }

    private void PauseGame()
    {
        bool isPause = GameplayManager.Instance.IsGamePause;
        GameplayManager.Instance.IsGamePause = !isPause;
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
}
