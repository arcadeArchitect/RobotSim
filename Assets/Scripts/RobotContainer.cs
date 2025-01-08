using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RobotContainer : MonoBehaviour
{
    public UnityEvent<InputAction.CallbackContext> onInput;
    
    public DriveSystem driveSystem;
    public IntakeSystem intakeSystem;
    public StorageSystem storageSystem;
    public ShooterSystem shooterSystem;
    public ClimbSystem climbSystem;
    public Transform chassis;

    private bool isShooting;
    private bool isGrabbing;
    private bool didGrab;
    private float climbInput;

    public void Input(InputAction.CallbackContext context)
    {
        onInput.Invoke(context);
    }

    public void ToggleFieldCentricInput(InputAction.CallbackContext context)
    {
        if (!context.ReadValueAsButton()) return;
        driveSystem.ToggleFieldCentric();
    }

    public void ShootInput(InputAction.CallbackContext context)
    {
        isShooting = context.ReadValueAsButton();
    }

    public void GrabInput(InputAction.CallbackContext context)
    {
        isGrabbing = context.ReadValueAsButton();
    }

    public void ClimbInput(InputAction.CallbackContext context)
    {
        climbInput = context.ReadValue<Vector2>().y;
    }

    private void Start()
    {
        driveSystem.SetRobotContainer(this);
        intakeSystem.SetRobotContainer(this);
        storageSystem.SetRobotContainer(this);
        shooterSystem.SetRobotContainer(this);
        climbSystem.SetRobotContainer(this);
    }

    private void Update()
    {
        if (isShooting && shooterSystem.canShoot && storageSystem.CanShoot())
            shooterSystem.ShootNextProjectile();

        if (isGrabbing && !didGrab)
        {
            climbSystem.ToggleGrab();
        }
        didGrab = isGrabbing;
        
        if (climbInput != 0 && climbSystem.isClimbing)
            climbSystem.Climb(climbInput);
    }
}