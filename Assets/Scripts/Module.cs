using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    [SerializeField] private MotorBehavior motor;
    private float input;
    public float rotation;
    private Transform wheel;

    void Awake()
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
        wheel.localRotation = Quaternion.Euler(0, rotation, 0);
    }

    private void SetRotation(Vector3 rotation)
    {
        wheel.LookAt(transform.position + transform.TransformDirection(rotation));
    }

    public void SetInputAndRotation(float input, float rotation)
    {
        SetInput(input);
        SetRotation(rotation);
    }

    public void SetInputAndRotation(float input, Vector3 rotation)
    {
        SetInput(input);
        SetRotation(rotation);
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public Vector3 GetOffset3()
    {
        return transform.localPosition;
    }
        
    public Vector2 GetOffset()
    {
        Vector3 offset3 = GetOffset3();
        return new Vector2(offset3.x, offset3.z);
    }

    public Vector3 GetForwardVector()
    {
        return wheel.forward;
    }

    public float GetRotation()
    {
        rotation = wheel.rotation.eulerAngles.y;
        return rotation;
    }

    public Vector2 GetWheelOffset()
    {
        return new Vector2(wheel.localPosition.x, wheel.localPosition.z);
    }
}
