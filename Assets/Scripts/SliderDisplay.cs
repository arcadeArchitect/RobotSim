
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SliderDisplay : MonoBehaviour
{
    [FormerlySerializedAs("robot")] public ArcadeDriveRobot arcadeDriveRobot;
    public Slider leftSlider;
    public Slider rightSlider;

    private void Update()
    {
        rightSlider.value = arcadeDriveRobot.GetRightInput();
        leftSlider.value = arcadeDriveRobot.GetLeftInput();
    }
}
