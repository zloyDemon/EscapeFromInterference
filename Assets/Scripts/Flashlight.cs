using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteMask))]
public class Flashlight : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] SpriteRenderer blinkImage;
    
    private const float FlashlightMaxValue = 5f;
    private const float FlashlightMinValue = 0f;
    private const float FlashlighPassValueTime = 5f;
    private const float FlashlightReduceDuration = 0.1f;
    private const float FlashlightScaleEndValue = 0.3f;
    private const float FlashlightStartScaleValue = 1f;
    private const float FLReduceChargeValue = 0.5f;
    private const float FLBlinkDuration = 0.15f;
    
    private Transform flashlight;
    private SpriteMask spriteMask;
    private float flashlightValue;
    private bool isFlashlightOff;
    private bool isFlashlightFewCharge;
    private Coroutine coChargeFullFlashlight;
    private float alphaBlink;

    public Action FlashlighDead = () => { };
    public Action<float> FlashlightChangeValue = v => { };

    public bool IsFlashlightOff { get { return isFlashlightOff; }}

    public float FlashlightChargeValue
    {
        get { return flashlightValue; }
        set { flashlightValue = value; }
    }
    
    public static float FlashlighMaxValue { get { return FlashlightMaxValue; }}
    //public static Flashlight Instance { get; private set; }

    private void Awake()
    {
        flashlightValue = FlashlightMaxValue;
        spriteMask = GetComponent<SpriteMask>();
        spriteMask.alphaCutoff = 0.3f;
        spriteMask.gameObject.SetActive(true);
        coChargeFullFlashlight = StartCoroutine(CoChangeChargeFlashlight());
        GameItems.Instance.FlashlightReset += ResetFlashLightBattery;
    }

    private void OnDestroy()
    {
        GameItems.Instance.FlashlightReset -= ResetFlashLightBattery;
    }

    private void Update()
    {
        //transform.position = target.position;
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
        if(coChargeFullFlashlight != null)
            StopCoroutine(coChargeFullFlashlight);
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
