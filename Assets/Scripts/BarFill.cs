using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarFill : MonoBehaviour
{
    [SerializeField] private Image bgImage;
    [SerializeField] private Image fillImage;
    
    private Vector2 maxValueSize;
    private float value;

    public Image BgImage => bgImage;
    public Image FillImage => fillImage;
    public float Value => value;

    private void Awake()
    {
        maxValueSize = fillImage.rectTransform.sizeDelta;
    }

    public void SetValue(float value)
    {
        var checkedValue = Mathf.Clamp01(value);
        fillImage.rectTransform.sizeDelta = new Vector2(maxValueSize.x * checkedValue, maxValueSize.y);
    }
}
