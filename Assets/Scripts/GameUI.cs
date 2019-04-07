using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private static readonly float shakeTime = 1f;
    
    [SerializeField] private Text indicationText;
    [SerializeField] private Slider indicationsSlider;
    
    private static GameUI _instance;
    private bool isIndicate = false;
    private Coroutine indicationCo;
    
    public float CurrentIndicateValue { get; set; }
    public static GameUI Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        if(_instance != this)
            Destroy(gameObject);
    }

    public void SetText(string text)
    {
        indicationText.text = text;
    }

    public void SetIndication(float value)
    {
        isIndicate = value <= CheckEnemy.CheckRadius;
        var maxValue = indicationsSlider.maxValue;
        
    }

}
