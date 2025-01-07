using UnityEngine;

public class ClimbAttach : MonoBehaviour
{
    public Transform target;
    public Vector3 climbOffset;

    private void Update()
    {
        if (!target) return;
        
        transform.position = target.position + target.TransformDirection(climbOffset);
    }

    public void Move(Vector3 direction)
    {
        transform.Translate(target.TransformDirection(direction));
    }

    public void SetOffsetByWorldPos(Vector3 pos)
    {
        climbOffset = pos - target.position;
    }
}
