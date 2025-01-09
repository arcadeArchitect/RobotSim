using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ClearGround : MonoBehaviour
{
    [SerializeField] private Vector3 boxSize = new(10f, 1f, 10f);
    [SerializeField] private Vector3 boxOffset = Vector3.down * 0.5f;
    private bool didClearGround;
    [SerializeField] private LayerMask robotLayer;
    [SerializeField] private ClimbSystem climbSystem;

    private void Update()
    {
        didClearGround = CalculateClearGround();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = didClearGround ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position + boxOffset, boxSize);
    }

    private bool CalculateClearGround()
    {
        return climbSystem.isClimbing && Physics.OverlapBoxNonAlloc(transform.position + boxOffset, boxSize / 2, new Collider[1], Quaternion.identity, robotLayer, QueryTriggerInteraction.Ignore) == 0;
    }
}
