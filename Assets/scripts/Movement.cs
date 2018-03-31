using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float move_speed = 5f;
    private Vector2 movement;
    public GameObject camera;

    void Start()
    {
        
    }


    void Update()
    {
        Move();
    }

    private void Move()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        if (inputX != 0 || inputY != 0)
        {
            movement.x = inputX;
            movement.y = inputY;

            Debug.Log(movement.x + " " + movement.y);
            //Rotation player
            Quaternion rotation = Quaternion.LookRotation(-movement, Vector3.forward);
            rotation.x = rotation.y = 0;
            movement = transform.TransformDirection(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 10f * Time.deltaTime);
            //Move player
            Vector3 move = new Vector3(inputX, inputY, 0);
            transform.position += move * move_speed * Time.deltaTime;
        }
    }
}