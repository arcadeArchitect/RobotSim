using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gearbox : MonoBehaviour
{
    [SerializeField] private MotorBehavior[] motors;
    private float input;

    private void FixedUpdate()
    {
        float totalTorque = 0;
        foreach (var motor in motors)
        {
            motor.SetInput(input);
            totalTorque += motor.GetTorque();
        }
        
        float averageTorque = totalTorque / motors.Length;

        foreach (var motor in motors)
        {
            motor.SetTorque(averageTorque);
        }
    }

    public void SetInput(float input)
    {
        this.input = Mathf.Clamp(input, -1, 1);
    }
}
