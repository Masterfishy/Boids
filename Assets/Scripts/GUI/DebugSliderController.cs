using UnityEngine;
using UnityEngine.UI;

public class DebugSliderController : MonoBehaviour
{
    public FloatReference Value;
    public Slider slider;

    private void OnEnable()
    {
        if (slider != null && Value != null)
        {
            slider.value = Value;
            slider.onValueChanged.AddListener(OnValueChanged);
        }
    }

    private void OnDisable()
    {
        if (slider != null)
        {
            slider.onValueChanged.RemoveAllListeners();
        }
    }

    private void OnValueChanged(float newValue)
    {
        if (Value != null)
        {
            Value.Value = newValue;
        }
    }
}
