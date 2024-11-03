using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterSystem : Subsystem
{
    [SerializeField] private Transform shooterTransform;
    [SerializeField] private float shootForce = 10f;
    [SerializeField] private float shootDelay = 0.5f;
    public bool CanShoot { get; private set; }
    private WaitForSeconds shootWait;

    private void Start()
    {
        shootWait = new WaitForSeconds(shootDelay);
        CanShoot = true;
    }

    public void Shoot(Projectile projectile)
    {
        projectile.Shoot(shooterTransform, shootForce);
        CanShoot = false;
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        yield return shootWait;
        CanShoot = true;
    }
}
