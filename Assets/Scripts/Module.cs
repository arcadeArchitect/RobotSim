using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    [SerializeField] private MotorBehavior motor;
    private float input;
    private Transform wheel;

    private void Awake()
    {
        wheel = motor.GetWheel().transform;
    }

    private void FixedUpdate()
    {
        motor.SetInputAndTorque(input);
    }

    public void SetInput(float val)
    {
        input = Mathf.Clamp(val, 0, 1);
    }

    private void SetRotation(float rotation)
    {
        motor.GetWheel().transform.localRotation = Quaternion.Euler(0, rotation, 0);
    }

    public void SetInputAndRotation(float val, float rotation)
    {
        if (val < 0.01f)
        {
            SetInput(0);
            Brake();
            return;
        }
        
        motor.SetBrake(0);
        SetInput(val);
        SetRotation(rotation);
    }
    
    public void Brake()
    {
        motor.SetBrake(1000);
    }

    public Vector3 GetWheelOffset()
    {
        return wheel.localPosition;
    }
}
