using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class DriveSystem : Subsystem
{
    [SerializeField] private Module frontRightModule;
    [SerializeField] private Module frontLeftModule;
    [SerializeField] private Module backRightModule;
    [SerializeField] private Module backLeftModule;
    
    [SerializeField] private Gyro gyro;

    [Space] [SerializeField] private float maxTorque = 100f;
    [SerializeField] private float rotationSpeed = Mathf.PI / 2;

    // distance between front and rear wheels (wheelbase)
    [Space] [SerializeField] private float length;
    
    // distance between wheels on the same axel (trackwidth)
    [SerializeField] private float width;

    [Space] [SerializeField] private bool fieldCentric = true;

    private Vector3 lastInput;
    private Vector2 translation;

    private float frontRightSpeed, frontLeftSpeed, backRightSpeed, backLeftSpeed;
    private float frontRightAngle, frontLeftAngle, backRightAngle, backLeftAngle;

    private void Start()
    {
        length = frontRightModule.GetWheelOffset().z - backRightModule.GetWheelOffset().z;
        width = frontRightModule.GetWheelOffset().x - frontLeftModule.GetWheelOffset().x;
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
        ThirdAlgorithm(input);
    }

    public void ToggleFieldCentric()
    {
        fieldCentric = !fieldCentric;
    }

    private void ThirdAlgorithm(Vector3 input)
    {
        float angle = gyro.GetRobotAngle();
        translation = fieldCentric ? new Vector2(-input.y * Mathf.Sin(angle) + input.x * Mathf.Cos(angle), input.y * Mathf.Cos(angle) + input.x * Mathf.Sin(angle)) : input;
        float rotation = input.z * rotationSpeed;
        
        // strafe = -input.y * Mathf.Sin(angle) + input.x * Mathf.Cos(angle);
        // forward = input.y * Mathf.Cos(angle) + input.x * Mathf.Sin(angle);

        float a = translation.x - rotation * length / 2;
        float b = translation.x + rotation * length / 2;
        float c = translation.y - rotation * width / 2;
        float d = translation.y + rotation * width / 2;
        
        frontRightSpeed = GetHyp(b, c);
        frontRightAngle = Mathf.Atan2(b, c) * Mathf.Rad2Deg;
        
        frontLeftSpeed = GetHyp(b, d);
        frontLeftAngle = Mathf.Atan2(b, d) * Mathf.Rad2Deg;
        
        backRightSpeed = GetHyp(a, c);
        backRightAngle = Mathf.Atan2(a, c) * Mathf.Rad2Deg;
        
        backLeftSpeed = GetHyp(a, d);
        backLeftAngle = Mathf.Atan2(a, d) * Mathf.Rad2Deg;
        
        float max = Mathf.Max(frontRightSpeed, frontLeftSpeed, backRightSpeed, backLeftSpeed);

        if (max <= 1) return;
        frontRightSpeed /= max;
        frontLeftSpeed /= max;
        backRightSpeed /= max;
        backLeftSpeed /= max;
    }

    private static float GetHyp(float a, float b)
    {
        return Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2));
    }

    private Vector3 V2ToV3(Vector2 v)
    {
        return new Vector3(v.x, 0f, v.y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(V2ToV3(translation)));
    }
}
