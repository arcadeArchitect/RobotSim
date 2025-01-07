using TMPro;
using UnityEngine;

public class SpeedCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private new Rigidbody rigidbody;

    private const float UnitsToFeet = 3.28084f;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        text.text = $"{GetVelocity():0.000}";
    }

    private float GetVelocity()
    {
        Vector3 flatVelocity = new(rigidbody.linearVelocity.x, 0f, rigidbody.linearVelocity.z);
        return flatVelocity.magnitude * UnitsToFeet;
    }
}
