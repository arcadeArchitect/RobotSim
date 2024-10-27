using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class IntakeSystem : Subsystem
{
    private void OnTriggerStay(Collider other)
    {
        Projectile projectile = other.GetComponent<Projectile>();
        if (!projectile || projectile.IsStored) return;
            
        bool didStore = robotContainer.storageSystem.StoreProjectile(projectile);
        
        if (!didStore) Debug.Log("Could not store projectile");
    }
}
