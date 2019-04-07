using System;
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
        checkEnemy.enemyChecked += EnemyChecked;
    }


    void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
//        Debug.Log(inputX + " -- " + inputY + " " + Mathf.RoundToInt(inputX));
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
    
    private void EnemyChecked(GameObject go)
    {
        GameplayManager.Instance.EnemyChecked(go.transform, transform);
    }

    IEnumerator Catching()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.LogError("Death");
    }
}