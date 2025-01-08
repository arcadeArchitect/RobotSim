using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Grabbable : MonoBehaviour
{
    public Rigidbody attachedRigidbody;
    private ArticulationBody articulationBody;
    public bool isBeingGrabbed;
    private Collider[] colliders;
    private Vector3 calculatedCOM;
    private Vector3 startingCOM;
    private float startingMass;

    private void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
        articulationBody = GetComponent<ArticulationBody>();
        startingCOM = articulationBody.centerOfMass;
        startingMass = articulationBody.mass;
    }

    private void Update()
    {
        if (isBeingGrabbed)
        {
            CalculateCOM();
            articulationBody.mass = startingMass + attachedRigidbody.mass;
            articulationBody.centerOfMass = calculatedCOM;
        }
        else
        {
            articulationBody.mass = startingMass;
            articulationBody.centerOfMass = startingCOM;
        }
    }

    public void ChangeCollision(Collider c, bool ignore)
    {
        foreach (Collider col in colliders)
            Physics.IgnoreCollision(col, c, ignore);
        
        // Debug.Log("Ignoring collision? " + ignore + " btw " + c.name);
    }

    private void CalculateCOM()
    {
        calculatedCOM = transform.InverseTransformPoint((transform.position + transform.TransformPoint(startingCOM)) * startingMass +
                         attachedRigidbody.worldCenterOfMass * attachedRigidbody.mass) /
                        (startingMass + attachedRigidbody.mass);
    }

    private void OnDrawGizmos()
    {
        if (!articulationBody || !attachedRigidbody) return;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + transform.TransformPoint(calculatedCOM), 0.5f);
    }
}
