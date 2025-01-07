using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private new Rigidbody rigidbody;
    public bool isStored {private set; get;}
    private Transform parentTransform;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        parentTransform = transform.parent;
    }

    public void Store(Transform storageParent)
    {
        rigidbody.isKinematic = true;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        transform.SetParent(storageParent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        isStored = true;
    }

    public void Shoot(Transform shooterTransform, float force)
    {
        transform.SetParent(parentTransform);
        rigidbody.isKinematic = false;
        rigidbody.constraints = RigidbodyConstraints.None;
        transform.position = shooterTransform.position;
        transform.forward = shooterTransform.forward;
        rigidbody.AddForce(shooterTransform.forward * force, ForceMode.Impulse);
        rigidbody.angularVelocity = Vector3.zero;
        isStored = false;
        // Debug.Log(gameObject.name + " has been shot");
    }
}
