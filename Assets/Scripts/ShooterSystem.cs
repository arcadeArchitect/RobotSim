using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterSystem : Subsystem
{
    [SerializeField] private Transform shooterTransform;
    [SerializeField] private float shootForce = 10f;

    public void Shoot(Projectile projectile)
    {
        projectile.Shoot(shooterTransform, shootForce);
    }
}
