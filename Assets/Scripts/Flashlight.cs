using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteMask))]
public class Flashlight : MonoBehaviour
{
    private static readonly float FlashlightStartScaleValue = .5f;
 
    private const float FlashlightMaxValue = 5f;
    private const float FlashlightMinValue = 0f;
    private const float FlashlighPassValueTime = 5f;
    private const float FlashlightReduceDuration = 0.5f;
    private const float FlashlightScaleEndValue = .15f;
    private const float FLReduceChargeValue = 0.5f;
    private const float FLBlinkDuration = 0.15f;
    
    [SerializeField] Transform target;
    [SerializeField] SpriteRenderer blinkImage;
    
    private Transform flashlight;
    private SpriteMask spriteMask;
    private float flashlightValue;
    private bool isFlashlightOff;
    private bool isFlashlightFewCharge;
    private Coroutine coChargeFullFlashlight;
    private float alphaBlink;

    public event Action FlashlighDead = () => { };
    public event Action<float> FlashlightChangeValue = v => { };

    public bool IsFlashlightOff => isFlashlightOff;

    public float FlashlightChargeValue
    {
        get => flashlightValue;
        set => flashlightValue = value;
    }
    
    public static float FlashlighMaxValue { get { return FlashlightMaxValue; }}

    private void Awake()
    {
        flashlightValue = FlashlightMaxValue;
        spriteMask = GetComponent<SpriteMask>();
        spriteMask.alphaCutoff = 0.3f;
        spriteMask.gameObject.SetActive(true);
        transform.localScale = Vector2.one * FlashlightStartScaleValue;
        coChargeFullFlashlight = StartCoroutine(CoChangeChargeFlashlight());
        GameItems.Instance.FlashlightReset += ResetFlashLightBattery;
    }

    private void OnDestroy()
    {
        GameItems.Instance.FlashlightReset -= ResetFlashLightBattery;
    }

    private void Update()
    {
        ReduceChargeFL();
    }

    private void ResetFlashLightBattery()
    {
        if (!IsFlashlightOff && GameItems.Instance.BatteryCount > 0)
            ChargeFullFlashlight();
    }

    public void ChargeFullFlashlight()
    {
        FlashlightChargeValue = FlashlightMaxValue;
        FlashlightChangeValue(FlashlightChargeValue);
        isFlashlightFewCharge = false;
        isFlashlightOff = false;
        var blinkColor = blinkImage.color;
        blinkColor.a = 0;
        blinkImage.color = blinkColor;
        transform.localScale = Vector2.one * FlashlightStartScaleValue;
        GameItems.Instance.SetBatteryValue(FlashlightChargeValue);
        GameItems.Instance.SetBatteryCount(-1);
        if (coChargeFullFlashlight != null)
        {
            StopCoroutine(coChargeFullFlashlight);
            coChargeFullFlashlight = null;
        }
            
        coChargeFullFlashlight = StartCoroutine(CoChangeChargeFlashlight());
        Debug.Log("ChargeFullFlashlight");
    }
    
    private void ReduceChargeFL()
    {
        if (!isFlashlightOff && isFlashlightFewCharge)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, FlashlightReduceDuration * Time.deltaTime);
            var blinkColor = blinkImage.color;
            blinkColor.a = Mathf.Lerp(blinkColor.a, 1f, FLBlinkDuration * Time.deltaTime);
            blinkImage.color = blinkColor;
            isFlashlightOff = transform.localScale.x < FlashlightScaleEndValue;
            if (isFlashlightOff)
            {
                spriteMask.enabled = false;
                FlashlighDead();
            }
        }
    }

    private IEnumerator CoChangeChargeFlashlight()
    {
        while (!isFlashlightFewCharge)
        {
            yield return new WaitForSeconds(FlashlighPassValueTime);
            FlashlightChargeValue -= FLReduceChargeValue;
            GameItems.Instance.SetBatteryValue(FlashlightChargeValue);
            FlashlightChangeValue(FlashlightChargeValue);
            isFlashlightFewCharge = FlashlightChargeValue <= FlashlightMinValue;
        }
    }
}
