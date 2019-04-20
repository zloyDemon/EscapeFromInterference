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
    private const float FlashlighPassValueTime = 1f;
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
    
    public float FlashlightChargeValue
    {
        get { return flashlightValue; }
        set { flashlightValue = value; }
    }
    
    public static float FlashlighMaxValue { get { return FlashlightMaxValue; }}

    private void Awake()
    {
        flashlightValue = FlashlightMaxValue;
        spriteMask = GetComponent<SpriteMask>();
        spriteMask.alphaCutoff = 0.3f;
        spriteMask.gameObject.SetActive(true);
        coChargeFullFlashlight = StartCoroutine(CoChangeChargeFlashlight());
    }

    private void Update()
    {
        transform.position = target.position;
        ReduceChargeFL();
        
        //For test only
        if(Input.GetKeyDown(KeyCode.R))
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
        if(coChargeFullFlashlight != null)
            StopCoroutine(coChargeFullFlashlight);
        coChargeFullFlashlight = StartCoroutine(CoChangeChargeFlashlight());
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
                spriteMask.gameObject.SetActive(false);
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
            FlashlightChangeValue(FlashlightChargeValue);
            isFlashlightFewCharge = FlashlightChargeValue <= FlashlightMinValue;
        }
    }
}
