using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDebug : MonoBehaviour
{
    [SerializeField] private Button keyCountDebug;

    private void Awake()
    {
        keyCountDebug.onClick.AddListener(KeyCountChange);
    }

    private void KeyCountChange()
    {
        GameItems.Instance.SetKeyCount(1);
    }
}
