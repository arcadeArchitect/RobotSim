using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class StorageSystem : Subsystem
{
    private enum ShootingMode
    {
        MiddleFirst,
        StorageOrder
    }
    
    [SerializeField] private int maxStoredProjectiles = 3;
    [SerializeField] private ProjectileStorage[] projectileStorages;
    [SerializeField] private ShootingMode shootingMode;
    private readonly Queue<int> storageOrderIndices = new();
    [SerializeField] private List<int> storageOrder = new();

    private void Update()
    {
        storageOrder = new List<int>(storageOrderIndices.ToArray());
    }

    public bool StoreProjectile(Projectile projectile)
    {
        if (GetStoredProjectileCount() >= maxStoredProjectiles) return false;
        
        ProjectileStorage storage = GetNextEmptyProjectileStorage();
        storage.Store(projectile);
        projectile.Store(storage.storageTransform);
        
        if (shootingMode == ShootingMode.StorageOrder)
            storageOrderIndices.Enqueue(Array.IndexOf(projectileStorages, storage));
        
        return true;
    }

    void OnDrawGizmos()
    {
        if (projectileStorages.Length == 0) return;
        
        foreach (var storage in projectileStorages)
        {
            Gizmos.color = (storage.isFull) ? Color.green : Color.red;
            Gizmos.DrawWireSphere(storage.storageTransform.position, 0.1f);
        }
    }

    private int GetStoredProjectileCount()
    {
        return projectileStorages.Count(projectileStorage => projectileStorage.isFull);
    }
    
    private ProjectileStorage GetNextFullProjectileStorage()
    {
        if (shootingMode == ShootingMode.MiddleFirst)
            return projectileStorages.FirstOrDefault(projectileStorage => projectileStorage.isFull);

        return null;
    }

    private ProjectileStorage GetNextEmptyProjectileStorage()
    {
        return projectileStorages.FirstOrDefault(projectileStorage => !projectileStorage.isFull);
    }

    public bool CanShoot()
    {
        return GetStoredProjectileCount() > 0;
    }

    public Projectile RemoveNextProjectile()
    {
        if (shootingMode == ShootingMode.StorageOrder)
            return projectileStorages[storageOrderIndices.Dequeue()].Remove();
        return GetNextFullProjectileStorage().Remove();
    }
}

[System.Serializable]
public class ProjectileStorage
{
    public Transform storageTransform;
    public bool isFull;
    public Projectile projectile;

    public void Store(Projectile projectile)
    {
        this.projectile = projectile;
        isFull = true;
    }

    public Projectile Remove()
    {
        Projectile removedProjectile = projectile;
        projectile = null;
        isFull = false;
        return removedProjectile;
    }
}
