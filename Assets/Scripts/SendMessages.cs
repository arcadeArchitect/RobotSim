using UnityEngine;
using UnityEngine.InputSystem;

public class SendMessages : MonoBehaviour
{
    public void OnMove(InputValue value)
    {
        var v = value.Get<Vector2>();
        Debug.Log(v);
    }
}
