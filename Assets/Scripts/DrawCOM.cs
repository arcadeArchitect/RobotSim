using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DrawCom : MonoBehaviour
{
    private new Rigidbody rigidbody;

    private void OnValidate()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 com = transform.position + rigidbody.centerOfMass;
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(com, 0.3f);
    }
}
