using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private new Rigidbody rigidbody;
    public bool IsStored {private set; get;}
    private Transform parentTransform;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        parentTransform = transform.parent;
    }

    public void Store(Transform storageParent)
    {
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        transform.SetParent(storageParent);
        transform.localPosition = Vector3.zero;
        IsStored = true;
    }

    public void Shoot(Transform shooterTransform, float force)
    {
        transform.SetParent(parentTransform);
        rigidbody.constraints = RigidbodyConstraints.None;
        transform.position = shooterTransform.position;
        rigidbody.AddForce(shooterTransform.forward * force, ForceMode.Impulse);
        IsStored = false;
        // Debug.Log(gameObject.name + " has been shot");
    }
}
