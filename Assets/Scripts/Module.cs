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

    public void SetInput(float input)
    {
        this.input = Mathf.Clamp(input, 0, 1);
    }

    private void SetRotation(float rotation)
    {
        motor.GetWheel().transform.localRotation = Quaternion.Euler(0, rotation, 0);
    }

    public void SetInputAndRotation(float input, float rotation)
    {
        if (input < 0.01f)
        {
            SetInput(0);
            motor.SetBrake(1000);
            return;
        }
        
        motor.SetBrake(0);
        SetInput(input);
        SetRotation(rotation);
    }

    public Vector3 GetWheelOffset()
    {
        return wheel.localPosition;
    }
}
