using UnityEngine;

public class DrawCom : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private ArticulationBody articulationBody;

    private void OnValidate()
    {
        rigidbody = GetComponent<Rigidbody>();
        articulationBody = GetComponent<ArticulationBody>();
    }

    private void OnDrawGizmosSelected()
    {
        if (!articulationBody && !rigidbody) return;
        Vector3 com = transform.position + transform.TransformDirection(rigidbody ? rigidbody.centerOfMass : articulationBody.centerOfMass);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(com, 0.3f);
    }
}
