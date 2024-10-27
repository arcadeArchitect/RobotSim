using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwerveDriveRobotTwo : MonoBehaviour
{

    [SerializeField] private Module[] modules;
    [SerializeField] private ModuleSpeed[] moduleSpeeds;
    [SerializeField] private Gyro gyro;

    [Space] [SerializeField] private float maxTorque = 100f;
    [SerializeField] private float rotationSpeed = 180f * Mathf.Deg2Rad;

    private Vector3 lastInput;
    private void Start()
    {
        moduleSpeeds = new ModuleSpeed[modules.Length];
        for (int i = 0; i < moduleSpeeds.Length; i++)
        {
            moduleSpeeds[i] = new ModuleSpeed(modules[i]);
        }
    }

    public void Update()
    {
        SwerveDrive(lastInput);
        // for (int i = 0; i < modules.Length; i++)
        // {
        //     modules[i].SetInputAndRotation(moduleSpeeds[i].speed, moduleSpeeds[i].angle);
        // }
        foreach (ModuleSpeed speed in moduleSpeeds)
        {
            speed.RunModule();
        }
    }

    public void SwerveDrive(InputAction.CallbackContext context)
    {
        lastInput = context.ReadValue<Vector3>();
    }

    private void SwerveDrive(Vector3 input) 
    {
        SecondAlgorithm(input);
    }

    private void SecondAlgorithm(Vector3 input)
    {
        input.z *= -1;
        float angle = gyro.GetRobotAngle();

        Vector2 desiredTranslation = input;
        desiredTranslation = V3ToV2(transform.TransformDirection(V2ToV3(desiredTranslation)));
        // Vector2 desiredTranslation = new Vector2(input.y * Mathf.Cos(angle) + input.x * Mathf.Sin(angle), -input.y * Mathf.Sin(angle) + input.x * Mathf.Cos(angle));
        Debug.DrawRay(transform.position, V2ToV3(desiredTranslation), Color.red);
        float desiredRotation = input.z * rotationSpeed;
        
        float maxSpeed = -Mathf.Infinity;
        foreach (ModuleSpeed speed in moduleSpeeds)
        {
            Vector2 moduleOut = ModuleOutput(speed.module, desiredTranslation, desiredRotation);
            speed.SetDirection(moduleOut);
            if (speed.speed > maxSpeed) maxSpeed = speed.speed;
            speed.Draw();
            // Debug.DrawRay(speed.module.GetWorldPosition(),  V2ToV3(desiredTranslation), Color.cyan);

            // Debug.DrawRay(speed.module.GetWorldOffset(),  speed.GetWorldDirection(), Color.cyan);
            // Debug.DrawRay(modules[i].GetWorldOffset(), new Vector3(moduleOut.x, 0, moduleOut.y), Color.cyan);
        }

        if (maxSpeed < 1) return;
        foreach (ModuleSpeed speed in moduleSpeeds)
        {
            speed.Normalize(maxSpeed);
        }
    }

    private Vector2 ModuleOutput(Module module, Vector2 desiredTranslation, float desiredRotation)
    {
        // return desiredTranslation + desiredRotation * Vector2.Perpendicular(module.GetOffset());
        return Vector2.zero;
    }

    private float GetHyp(float a, float b)
    {
        return Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2));
    }

    private Vector3 V2ToV3(Vector2 v)
    {
        return new Vector3(v.x, 0f, v.y);
    }

    private Vector2 V3ToV2(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }
}
