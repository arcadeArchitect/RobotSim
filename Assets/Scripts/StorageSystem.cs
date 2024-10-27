using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StorageSystem : Subsystem
{
    [SerializeField] private int maxStoredProjectiles = 3;
    [SerializeField] private ProjectileStorage[] projectileStorages;

    public bool StoreProjectile(Projectile projectile)
    {
        if (GetStoredProjectileCount() >= maxStoredProjectiles) return false;
        
        ProjectileStorage storage = GetNextEmptyProjectileStorage();
        storage.Store(projectile);
        projectile.Store(storage.storageTransform);
        return true;
    }

    void OnDrawGizmos()
    {
        if (projectileStorages.Length == 0) return;
        
        int storedProjectileIndex = GetStoredProjectileCount() - 1;
        foreach (var storage in projectileStorages)
        {
            Gizmos.color = (storage.isFull) ? Color.green : Color.red;
            Gizmos.DrawWireSphere(storage.storageTransform.position, 0.1f);
        }
    }

    private int GetStoredProjectileCount()
    {
        int count = 0;
        foreach (var projectileStorage in projectileStorages)
        {
            if (projectileStorage.isFull)
                ++count;
        }

        return count;
    }
    
    private ProjectileStorage GetNextFullProjectileStorage()
    {
        foreach (var projectileStorage in projectileStorages)
        {
            if (projectileStorage.isFull)
                return projectileStorage;
        }

        return null;
    }

    private ProjectileStorage GetNextEmptyProjectileStorage()
    {
        foreach (var projectileStorage in projectileStorages)
        {
            if (!projectileStorage.isFull)
                return projectileStorage;
        }

        return null;
    }

    public bool CanShoot()
    {
        return GetStoredProjectileCount() > 0;
    }

    public Projectile RemoveNextProjectile()
    {
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
        this.projectile = null;
        isFull = false;
        return removedProjectile;
    }
}
