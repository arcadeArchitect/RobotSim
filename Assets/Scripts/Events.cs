using UnityEngine;
using UnityEngine.InputSystem;

public class Events : MonoBehaviour
{
    public void PrintVector(Vector2 vector2)
    {
        Debug.Log(vector2.ToString());
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
    }
}
