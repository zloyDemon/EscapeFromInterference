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

    [SerializeField] Image batteryValue;
    [SerializeField] Text batteryCount;
    [SerializeField] Text keyCount;
    [SerializeField] Button changeBatteryBtn;
    [SerializeField] Text subtitleText;
    [SerializeField] CanvasGroup fadeImage;
    
    private static GameUI _instance;
    private bool isIndicate = false;
    private Coroutine indicationCo;
    private float currentValue = 0.0f;
    private float toValue = 1.0f;
    private bool isReset = false;
    private Coroutine corFade;
    
    public float CurrentIndicateValue { get; set; }
    public static GameUI Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        if(_instance != this)
            Destroy(gameObject);

        batteryValue.fillAmount = 1;
        subtitleText.text = string.Empty;

        GameItems.Instance.BatteryCountChange += InstanceOnBatteryCountChange;
        GameItems.Instance.BatteryValueChange += InstanceOnBatteryValueChange;
        GameItems.Instance.KeyCountChange += InstanceOnKeyCountChange;
        GameItems.Instance.DeviceValueChange += SetIndication;
        changeBatteryBtn.onClick.AddListener(ChangeBatteryClick);

        
        InstanceOnBatteryCountChange(GameItems.Instance.BatteryCount);
        InstanceOnKeyCountChange(0);
    }

    private void Start()
    {
        DungeonManager.Instance.LevelLoaded += InstanceOnLevelLoaded;
        SubtitleManager.Instance.SubtitleSet += SetSubtitleText;
        GameplayManager.Instance.StartFallGame += FadeTakingPlayer;
        GameplayManager.Instance.EndGame += EndGame;
        Debug.Log("GameUI Start");
    }

    private void InstanceOnLevelLoaded()
    {
        
    }

    private void EndGame()
    {
        SubtitleManager.Instance.SetSubtitle("Game over");
    }

    private void OnDestroy()
    {
        GameItems.Instance.BatteryCountChange -= InstanceOnBatteryCountChange;
        GameItems.Instance.BatteryValueChange -= InstanceOnBatteryValueChange;
        GameItems.Instance.KeyCountChange -= InstanceOnKeyCountChange;
        GameItems.Instance.DeviceValueChange -= SetIndication;
        GameplayManager.Instance.EndGame += EndGame;
        SubtitleManager.Instance.SubtitleSet -= SetSubtitleText;
        DungeonManager.Instance.LevelLoaded -= InstanceOnLevelLoaded;
        GameplayManager.Instance.StartFallGame -= FadeTakingPlayer;
    }
    
    private void InstanceOnBatteryCountChange(int value)
    {
        batteryCount.text = string.Format("{0}", value);
        changeBatteryBtn.interactable = value > 0;
    }

    private void FadeTakingPlayer(bool isTaking)
    {
        Debug.LogError("IsTaking: " + isTaking);
        if (corFade != null)
        {
            StopCoroutine(corFade);
            corFade = null;
        }

        corFade = StartCoroutine(CorTakingFade(isTaking));
    }
    
    private void InstanceOnBatteryValueChange(float value)
    {
        float res = value / Flashlight.FlashlighMaxValue;
        batteryValue.fillAmount = res;
    }
    
    private void InstanceOnKeyCountChange(int value)
    {
        keyCount.text = string.Format("{0}", value);
    }


    public void SetIndication(float value)
    {

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

    private IEnumerator CorTakingFade(bool FadeOut)
    {
        float result;
        if (FadeOut)
        {
            for (float i = fadeImage.alpha; i < 1; i += (FadeDuration * Time.deltaTime))
            {
                fadeImage.alpha = i;
                yield return null;
            }
               
            result = 1;
            yield return new WaitForSeconds(1);
        }
        else
        {
            for (float i = fadeImage.alpha; i > 0; i -= (FadeDuration * Time.deltaTime))
            {
                fadeImage.alpha = i;
                yield return null;
            }
            result = 0;
            yield return new WaitForSeconds(1);
        }

        fadeImage.alpha = result;
        Debug.LogError("isTaking " + FadeOut);
        if(FadeOut)
            GameplayManager.Instance.StartEndGame();
        corFade = null;
    }
}
