using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class ClimbSystem : Subsystem
{
    [SerializeField] private GameObject robot;
    // public List<Grabbable> grabbables = new();
    private Grabbable grabby;
    private SpringJoint climbJoint;
    [SerializeField] private Transform castPos;
    [SerializeField] private float castRadius;

    public bool isClimbing { get; private set; }
    private Collider[] results;
    private Grabbable lastGrabby;
    [SerializeField] private ClimbAttach climbAttach;
    
    private void Start()
    {
        climbJoint = GetComponentInParent<SpringJoint>();
    }
    
    private void Grab()
    {
        // Debug.Log("GRAB");
        FindGrabbedObjects();
        if (grabby == null)
        {
            // Debug.Log("NVM GRAB");
            isClimbing = false;
            return;
        }
        
        // climbJoint = robot.AddComponent<SpringJoint>();
        climbJoint.connectedArticulationBody = grabby.attachBody;
        grabby.robotChassis = robotContainer.chassis;
        grabby.isBeingGrabbed = true;
        climbJoint.spring = 10000f;
    }

    private void FindGrabbedObjects()
    {
        results = Physics.OverlapSphere(castPos.position, castRadius);
        foreach (Collider c in results)
        {
            Grabbable g = c.GetComponentInParent<Grabbable>();
            if (!g) continue;
            grabby = g;
            climbAttach.target = c.transform;
            climbAttach.SetOffsetByWorldPos(c.ClosestPoint(castPos.position));
            return;
        }

        grabby = null;
    }

    private void Ungrab()
    {
        // Debug.Log("UNGRAB");
        lastGrabby.isBeingGrabbed = false;
        grabby = null;
        climbJoint.connectedArticulationBody = null;
        climbJoint.spring = 0f;
        // Destroy(climbJoint);
    }

    public void ToggleGrab()
    {
        isClimbing = !isClimbing;
        if (isClimbing) Grab();
        else Ungrab();
    }

    private void Update()
    {
        if (!grabby) return;
        lastGrabby = grabby;
    }

    private void OnDrawGizmos()
    {
        if (!castPos) return;
        FindGrabbedObjects();
        Gizmos.color = isClimbing ? Color.Lerp(Color.blue, Color.cyan, 0.5f) : grabby ? Color.green : Color.red;
        Gizmos.DrawWireSphere(castPos.position, castRadius);
    }
}
