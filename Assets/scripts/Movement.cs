using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private const float MoveSpeed = 60f;
    
    [SerializeField] Flashlight flashlight;
    
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private CheckEnemy checkEnemy;
    private Coroutine checkingCor;
    private Joystick joystick;
    private GameObject camera;

    private void Awake()
    {
        flashlight.FlashlightChangeValue += FlashlightChangeValue; 
        flashlight.FlashlighDead += FlashlightDead;
        camera = Camera.main.gameObject;
        joystick = UIManager.Instance.GameUI.Joystick;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        flashlight.FlashlightChangeValue -= FlashlightChangeValue;
        flashlight.FlashlighDead -= FlashlightDead;
    }

    private void Update()
    {
        camera.transform.position = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);
    }

    private void FixedUpdate()
    {
        Move(joystick.JoystickValue);
    }

    private void Move(Vector2 axisDirection)
    {
        float inputX = axisDirection.x;
        float inputY = axisDirection.y;

        if ((Mathf.Abs(inputX) > 0.1 || Mathf.Abs(inputY) > 0.1))
        {
            movement.x = inputX;
            movement.y = inputY;

            movement = transform.TransformDirection(movement);
            rb.velocity = movement * MoveSpeed;
            
            animator.SetFloat("Horizontal", inputX);
            animator.SetFloat("Vertical", inputY);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    
    private void FlashlightChangeValue(float value)
    {
        //GameItems.Instance.SetBatteryValue(value);
    }
    
    private void FlashlightDead()
    {
        //GameplayManager.Instance.GameOver(EFIEnums.GameOverReason.FlashlightDead);
    }
}