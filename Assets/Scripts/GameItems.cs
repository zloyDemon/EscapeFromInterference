using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItems
{
    private int batteryCount;
    private int keyCount;
    private float batteryValue;
    private float deviceValue;

    private static GameItems instance = new GameItems();
    
    public static GameItems Instance
    {
        get { return instance; }
    }
    
    public int BatteryCount { get { return batteryCount; }}
    public int KeyCount { get { return keyCount; }}
    public float BatteryValue { get { return batteryValue; }}
    public float DeviceValue { get { return deviceValue; }}

    public event Action<int> BatteryCountChange = c => { }; 
    public event Action<int> KeyCountChange = c => { }; 
    public event Action<float> BatteryValueChange = c => { }; 
    public event Action<float> DeviceValueChange = c => { };
    public event Action FlashlightReset = () => { };

    public void SetBatteryCount(int value)
    {
        int result = batteryCount + value;
        if (result < 0)
            result = 0;
        batteryCount = result;
        BatteryCountChange(batteryCount);
    }

    public void SetKeyCount(int value)
    {
        keyCount = keyCount + value;
        KeyCountChange(keyCount);
    }

    public void SetBatteryValue(float value)
    {
        batteryValue = value;
        BatteryValueChange(batteryValue);
    }

    public void SetDeviceValue(float value)
    {
        deviceValue = value;
        DeviceValueChange(deviceValue);
    }

    public void ResetFlashlight()
    {
        FlashlightReset();
    }

    public void ResetItems()
    {
        keyCount = 0;
    }
}
