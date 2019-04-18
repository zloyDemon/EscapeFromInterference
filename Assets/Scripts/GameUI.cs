﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private static readonly float shakeTime = 1f;
    private static readonly float lerpSpeed = 2f;
    
    [SerializeField] private Text indicationText;
    [SerializeField] private Slider indicationsSlider;
    [SerializeField] private Slider batteryChargeSlider;
    
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
    }

    private void OnDestroy()
    {
        
    }

    private void Update()
    {
        ResetIndication();
    }

    public void SetText(string text)
    {
        indicationText.text = text;
    }

    public void SetIndication(float value)
    {
        currentValue = indicationsSlider.value;
        toValue = value;
        indicationsSlider.value = toValue;
        if (value <= 0 && !isReset)
            isReset = true;
    }
    
    public void FlashlightChargeChange(float value)
    {
        var setValue = value / Flashlight.FlashlighMaxValue;
        setValue = Mathf.Clamp01(setValue);
        batteryChargeSlider.value = setValue;
    }

    private void ResetIndication()
    {
        if (isReset)
        {
            currentValue = Mathf.Lerp(currentValue, -0.001f, Time.deltaTime * lerpSpeed);
            indicationsSlider.value = currentValue;
            isReset = currentValue > toValue;
            if(!isReset)
                Debug.Log("ISReset");
        }
    }
}
