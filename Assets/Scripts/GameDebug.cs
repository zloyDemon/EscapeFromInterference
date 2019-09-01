using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDebug : MonoBehaviour
{
    [SerializeField] private Button keyCountDebug;
    [SerializeField] private Button batteryCountDebug;

    private void Awake()
    {
        keyCountDebug.onClick.AddListener(KeyCountChange);
        batteryCountDebug.onClick.AddListener(BatteryCountChange);
    }

    private void KeyCountChange()
    {
        GameItems.Instance.SetKeyCount(1);
    }
    
    private void BatteryCountChange()
    {
        GameItems.Instance.SetBatteryCount(1);
    }
}
