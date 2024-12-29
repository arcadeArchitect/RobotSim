using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class ArcadeDriveRobot : MonoBehaviour
{
    public Gearbox rightGearbox;
    public Gearbox leftGearbox;

    [SerializeField] [Range(0, 1)] private float moveSensitivity = 1;
    [SerializeField] [Range(0, 1)] private float turnSensitivity = 0.5f;
    
    private float rightInput;
    private float leftInput;

    public void Update()
    {
        leftGearbox.SetInput(leftInput);
        rightGearbox.SetInput(rightInput);
    }

    public void ArcadeDrive(InputAction.CallbackContext context)
    {
        ArcadeDrive(context.ReadValue<Vector2>());
    }

    private void ArcadeDrive(Vector2 input)
    {
        leftInput = input.y * moveSensitivity + input.x * turnSensitivity;
        rightInput = input.y * moveSensitivity - input.x * turnSensitivity;
        
        // Normalize
        float factor = Mathf.Max(Mathf.Abs(leftInput), Mathf.Abs(rightInput));
        if (factor <= 1) return;
        leftInput /= factor;
        rightInput /= factor;
    }

    public float GetLeftInput()
    {
        return leftInput;
    }

    public float GetRightInput()
    {
        return rightInput;
    }
}
