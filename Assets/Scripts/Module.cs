using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    [SerializeField] private MotorBehavior motor;
    private float input;

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
        SetInput(input);
        SetRotation(rotation);
    }
}
