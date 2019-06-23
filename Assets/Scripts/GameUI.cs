using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private static readonly float shakeTime = 1f;
    private static readonly float lerpSpeed = 2f;

    [SerializeField] private Slider indicationsSlider;
    [SerializeField] private Slider batteryChargeSlider;
    [SerializeField] private Text batteryCount;
    [SerializeField] private Text keyCount;
    [SerializeField] private Button changeBatteryBtn;
    [SerializeField] private Text subtitleText;
    
    private static GameUI _instance;
    private bool isIndicate = false;
    private Coroutine indicationCo;
    private float currentValue = 0.0f;
    private float toValue = 1.0f;
    private bool isReset = false;
    
    public float CurrentIndicateValue { get; set; }
    public static GameUI Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        if(_instance != this)
            Destroy(gameObject);

        batteryChargeSlider.value = batteryChargeSlider.maxValue;
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
        Debug.Log("GameUI Start");
    }

    private void InstanceOnLevelLoaded()
    {

    }

    private void OnDestroy()
    {
        GameItems.Instance.BatteryCountChange -= InstanceOnBatteryCountChange;
        GameItems.Instance.BatteryValueChange -= InstanceOnBatteryValueChange;
        GameItems.Instance.KeyCountChange -= InstanceOnKeyCountChange;
        GameItems.Instance.DeviceValueChange -= SetIndication;
        SubtitleManager.Instance.SubtitleSet -= SetSubtitleText;
        DungeonManager.Instance.LevelLoaded -= InstanceOnLevelLoaded;
    }

    private void Update()
    {
        ResetIndication();          
    }
    
    private void InstanceOnBatteryCountChange(int value)
    {
        batteryCount.text = string.Format("{0}", value);
        changeBatteryBtn.interactable = value > 0;
    }
    
    private void InstanceOnBatteryValueChange(float value)
    {
        float res = value / Flashlight.FlashlighMaxValue;
        batteryChargeSlider.value = res;
    }
    
    private void InstanceOnKeyCountChange(int value)
    {
        keyCount.text = string.Format("{0}", value);
    }


    public void SetIndication(float value)
    {
        currentValue = indicationsSlider.value;
        toValue = value;
        indicationsSlider.value = toValue;
        if (value <= 0 && !isReset)
            isReset = true;
    }

    private void ResetIndication()
    {
        if (isReset)
        {
            currentValue = Mathf.Lerp(currentValue, -0.001f, Time.deltaTime * lerpSpeed);
            indicationsSlider.value = currentValue;
            isReset = currentValue > toValue;
            if(!isReset)
                Debug.Log("ISReset false");
        }
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
