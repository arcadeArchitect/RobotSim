using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class MiddleSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    // This can be set to a SerializeField if you want to be able to
    // change this in editor
    private const float StartValue = 0f;
    private const float CenterAnchorX = 0.5f;

    void Awake()
    {
        slider = GetComponent<Slider>();

        // If the slider isn't null, add the UpdateSlider method as a
        // listener for the slider's onValueChanged event
        if (slider)
        {
            slider.onValueChanged.AddListener(UpdateSlider);
        }
    }

    private void Start()
    {
        // Ensures the slider value is set to the center
        slider.value = StartValue;
        // If the value of the slider is already 0, the onValueChanged
        // event won't be fired, so make sure it does to ensure the fill
        // is centered from the start
        UpdateSlider(StartValue);
    }

    private void OnDisable()
    {
        if (slider)
        {
            slider.onValueChanged.RemoveListener(UpdateSlider);
        }
    }

    /// <summary>
    /// Updates the fillRect's anchors based on the position of the handle's anchors, which get adjusted as the
    /// handle is moved. If the handle is in the "negative", the fill is adjusted to stretch from the center to
    /// the position of the handle, and vice versa in the "positive".
    /// </summary>
    public void UpdateSlider(float value)
    {
        slider.fillRect.anchorMin = new Vector2(Mathf.Clamp(slider.handleRect.anchorMin.x, 0, CenterAnchorX), 0);
        slider.fillRect.anchorMax = new Vector2(Mathf.Clamp(slider.handleRect.anchorMin.x, CenterAnchorX, 1), 1);
    }
}