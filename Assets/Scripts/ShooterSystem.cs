using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterSystem : Subsystem
{
    [SerializeField] private Transform shooterTransform;
    [SerializeField] private float shootForce = 10f;
    [SerializeField] private float shootDelay = 0.5f;
    public bool canShoot { get; private set; }
    private WaitForSeconds shootWait;

    private void Start()
    {
        shootWait = new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    public void Shoot(Projectile projectile)
    {
        if (projectile == null) return;
        projectile.Shoot(shooterTransform, shootForce);
        canShoot = false;
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        yield return shootWait;
        canShoot = true;
    }

    public void ShootNextProjectile()
    {
        Shoot(robotContainer.storageSystem.RemoveNextProjectile());
    }
}
