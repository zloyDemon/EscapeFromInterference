using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteMask))]
public class Flashlight : MonoBehaviour
{
    [SerializeField] Transform target;
    
    private const float FlashlightMaxValue = 5f;
    private const float FlashlighPassValueTime = 4f;
    
    private Transform flashlight;
    private SpriteMask spriteMask;
    private float flashlightValue;
    
    private void Awake()
    {
        flashlightValue = FlashlightMaxValue;
        spriteMask = GetComponent<SpriteMask>();
        spriteMask.alphaCutoff = 0.3f;
    }

    private void Update()
    {
        transform.position = target.position;
    }
}
