using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorBehavior : MonoBehaviour
{
    [SerializeField] private WheelBehavior wheel;
    private float input;
    
    [SerializeField] private float maxTorque = 100f;
    
    public void SetTorque(float torque)
    {
        wheel.SetTorque(torque);
    }

    public float GetTorque()
    {
        return input * maxTorque;
    }

    public void SetInput(float input)
    {
        this.input = Mathf.Clamp(input, -1, 1);
    }

    public void SetInputAndTorque(float input)
    {
        SetInput(input);
        SetTorque(GetTorque());
    }

    public float GetInput()
    {
        return input;
    }

    public WheelBehavior GetWheel()
    {
        return wheel;
    }
}
