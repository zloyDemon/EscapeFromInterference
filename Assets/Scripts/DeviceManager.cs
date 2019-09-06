using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceManager
{
    public const float EndSignalValue = 0.9f;
    public static readonly float CheckGhostDeviceMaxValue = 5f;
    public static readonly float CheckDistance = 3f;
    public static DeviceManager Instance { private set; get; } = new DeviceManager();
    
    public float SignalValue { private set; get; }

    public event Action<float> SignalValueChanged = f => { };
    
    public void SetSignalValue(float signalValue)
    {
        SignalValue = signalValue;
        SignalValueChanged(SignalValue);
        CheckSignalForEnd();
    }

    private void CheckSignalForEnd()
    {
        GameplayManager.Instance.GameOver(SignalValue >= EndSignalValue ? EFIEnums.GameOverReason.EnemyCatch :
            EFIEnums.GameOverReason.DisableGO);
    }
}