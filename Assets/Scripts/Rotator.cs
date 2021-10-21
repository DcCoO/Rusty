﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : SingletonMonoBehaviour<Rotator>
{

    [SerializeField] bool auto;
    
    [Header("Automatic Config")]
    [SerializeField] float autoSpeed;

    [Header("Manual Config")]
    [SerializeField] float manualMovePercent;
    [SerializeField] float manualAcceleration;
    [SerializeField] float manualMaxSpeed;
    [SerializeField] float manualSpeed;

    Transform tf;
    float sw, sh;
    void Start()
    {
        tf = transform;
        sw = Screen.width;
        sh = Screen.height;
    }

    public void SetAutomaticRotation(bool state) => auto = state;

    // Update is called once per frame
    void Update()
    {
        if(auto) tf.Rotate(0, autoSpeed * Time.deltaTime, 0); 
        else if(Input.GetMouseButton(0))
        {
            Vector2 mouse = Input.mousePosition;
            if(mouse.x < manualMovePercent * sw && mouse.y < 0.85f * sh)
            {
                tf.Rotate(0, manualSpeed * Time.deltaTime, 0);
                Chisel.instance.Disappear();
            }
            else if (mouse.x > (1 - manualMovePercent) * sw && mouse.y < 0.85f * sh)
            {
                tf.Rotate(0, -manualSpeed * Time.deltaTime, 0);
                Chisel.instance.Disappear();
            }
            else
            {
                manualSpeed = 0;
            }
            manualSpeed = Mathf.Clamp(manualSpeed + manualAcceleration * Time.deltaTime, 0, manualMaxSpeed);
        }
        else
        {
            manualSpeed = 0;
        }
    }
}
