﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]private float MoveSpeed = 25f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private CheckEnemy checkEnemy;
    private Coroutine checkingCor;
    
    public GameObject camera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        checkEnemy = GetComponent<CheckEnemy>();
        checkEnemy.EnemyChecked += EnemyChecked;
    }

    private float v = 0.2f;
    private void Update()
    {
            
    }

    void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

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
    
    private void EnemyChecked(GameObject[] enemies)
    {
        if (enemies.Length <= 0) return;
        var nearEnemy = enemies[0];
        var distance = Vector2.Distance(transform.position, nearEnemy.transform.position);
        var deviceValue = EFIUtils.DeviceValue(distance);
        if (deviceValue > EFIUtils.CheckGhostDeviceMaxValue)
            deviceValue = EFIUtils.CheckGhostDeviceMaxValue;
        GameUI.Instance.SetIndication(deviceValue / EFIUtils.CheckGhostDeviceMaxValue);
        GameUI.Instance.SetText(nearEnemy.name + " " + deviceValue);
        if (distance > EFIUtils.CheckDistance)
        {
            GameUI.Instance.SetText("Empty");
            GameUI.Instance.SetIndication(0);
        }
            
    }
}