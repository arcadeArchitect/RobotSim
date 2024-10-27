using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterSystem : Subsystem
{
    [SerializeField] private Transform shooterTransform;
    [SerializeField] private float shootForce = 10f;
    [SerializeField] private float lastTimeShot = Mathf.NegativeInfinity;
    [SerializeField] private float shootDelay = 0.5f;

    public bool CanShoot()
    {
        return Time.time >= lastTimeShot + shootDelay;
    }

    public void Shoot(Projectile projectile)
    {
        projectile.Shoot(shooterTransform, shootForce);
        lastTimeShot = Time.time;
    }
}
