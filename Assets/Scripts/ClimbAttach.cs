using System;
using UnityEngine;

public class ClimbAttach : MonoBehaviour
{
    public Transform target;
    public Vector3 climbOffset;
    private FixedJoint fixedJoint;
    [HideInInspector] public Rigidbody robot;
    [SerializeField] private BoxCollider groundCollider;
    [SerializeField] private BoxCollider robotSwerveCollider;
    private new Rigidbody rigidbody;

    private const float MaxY = -0.5f;
    private const float MinY = -1.5f;


    private void Start()
    {
        fixedJoint = GetComponent<FixedJoint>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!target) return;
        
        // rigidbody.position = target.position + target.TransformDirection(climbOffset);
        transform.position = target.position + target.TransformPoint(climbOffset);
        // rigidbody.MovePosition(target.position + target.TransformDirection(climbOffset));
        
        transform.up = target.up;
        
        // groundCollider.transform.position = robotSwerveCollider.transform.position;
        groundCollider.transform.rotation = robotSwerveCollider.transform.rotation;
        groundCollider.center = groundCollider.transform.InverseTransformPoint(robotSwerveCollider.transform.TransformPoint(robotSwerveCollider.center));
    }

    public void Move(Vector3 direction)
    {
        Debug.Log("Climbing in direction: " + direction);
        climbOffset += direction;
        climbOffset.y = Mathf.Clamp(climbOffset.y, MinY, MaxY);
    }

    public void SetOffsetByWorldPos(Vector3 pos)
    {
        if (fixedJoint.connectedBody) return;
        climbOffset = target.InverseTransformDirection(pos - target.position);
    }

    public void Attach()
    {
        Debug.Log("Attach");
        fixedJoint.connectedBody = robot;
        groundCollider.gameObject.SetActive(true);
    }

    public void Detach()
    {
        Debug.Log("Detach");
        fixedJoint.connectedBody = null;
        groundCollider.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if (!target) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(target.position + target.TransformDirection(climbOffset), 0.2f);
    }
}
