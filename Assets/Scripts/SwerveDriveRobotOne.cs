using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwerveDriveRobotOne : MonoBehaviour
{
    [SerializeField] private Module frontRightModule;
    [SerializeField] private Module frontLeftModule;
    [SerializeField] private Module backRightModule;
    [SerializeField] private Module backLeftModule;
    
    [SerializeField] private Gyro gyro;

    [Space] [SerializeField] private float maxTorque = 100f;
    [SerializeField] private float rotationSpeed = 180f * Mathf.Deg2Rad;

    // distance between front and rear wheels (wheelbase)
    [Space] [SerializeField] private float length;
    
    // distance between wheels on the same axel (trackwidth)
    [SerializeField] private float width;
    private float r;

    private Vector3 lastInput;

    public float frontRightSpeed, frontLeftSpeed, backRightSpeed, backLeftSpeed;
    public float frontRightAngle, frontLeftAngle, backRightAngle, backLeftAngle;

    private void Start()
    {
        r = GetHyp(length, width);
    }

    public void Update()
    {
        SwerveDrive(lastInput);
        frontRightModule.SetInputAndRotation(frontRightSpeed, frontRightAngle);
        frontLeftModule.SetInputAndRotation(frontLeftSpeed, frontLeftAngle);
        backRightModule.SetInputAndRotation(backRightSpeed, backRightAngle);
        backLeftModule.SetInputAndRotation(backLeftSpeed, backLeftAngle);
    }

    public void SwerveDrive(InputAction.CallbackContext context)
    {
        lastInput = context.ReadValue<Vector3>();
    }

    private void SwerveDrive(Vector3 input) 
    {
        FirstAlgorithm(input);
    }

    private void FirstAlgorithm(Vector3 input)
    {
        // Debug.Log("Swerve Drive");
        float angle = gyro.GetRobotAngle();

        float forward = input.y;
        float strafe = input.x;
        float rotate = input.z;
        
        float temp = forward * Mathf.Cos(angle) + strafe * Mathf.Sin(angle);
        strafe = -forward * Mathf.Sin(angle) + strafe * Mathf.Cos(angle);
        forward = temp;

        float a = strafe - rotate * (length / r);
        float b = strafe + rotate * (length / r);
        float c = forward - rotate * (width / r);
        float d = forward + rotate * (width / r);

        frontRightSpeed = GetHyp(b, c);
        frontLeftSpeed = GetHyp(b, d);
        backRightSpeed = GetHyp(a, d);
        backLeftSpeed = GetHyp(a, c);
        
        frontRightAngle = Mathf.Atan2(b, c) * Mathf.Rad2Deg;
        frontLeftAngle = Mathf.Atan2(b, d) * Mathf.Rad2Deg;
        backRightAngle = Mathf.Atan2(a, d) * Mathf.Rad2Deg;
        backLeftAngle = Mathf.Atan2(a, c) * Mathf.Rad2Deg;
        
        float max = Mathf.Max(frontRightSpeed, frontLeftSpeed, backRightSpeed, backLeftSpeed);
        if (max > 1)
        {
            frontRightSpeed /= max;
            frontLeftSpeed /= max;
            backRightSpeed /= max;
            backLeftSpeed /= max;
        }
    }

    private float GetHyp(float a, float b)
    {
        return Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2));
    }
}
