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

    private bool isShooting;

    public void Input(InputAction.CallbackContext context)
    {
        onInput.Invoke(context);
    }

    public void ToggleFieldCentricInput(InputAction.CallbackContext context)
    {
        bool button = context.ReadValueAsButton();
        if (!button) return;
        driveSystem.ToggleFieldCentric();
    }

    public void ShootInput(InputAction.CallbackContext context)
    {
        bool button = context.ReadValueAsButton();
        isShooting = button;
        // if (!button || !storageSystem.CanShoot()) return;
        // Projectile projectile = storageSystem.RemoveNextProjectile();
        // shooterSystem.Shoot(projectile);
    }

    private void Start()
    {
        driveSystem.SetRobotContainer(this);
        intakeSystem.SetRobotContainer(this);
        storageSystem.SetRobotContainer(this);
        shooterSystem.SetRobotContainer(this);
    }

    private void Update()
    {
        if (!(isShooting && shooterSystem.CanShoot() && storageSystem.CanShoot())) return;
        Projectile projectile = storageSystem.RemoveNextProjectile();
        shooterSystem.Shoot(projectile);
    }
}