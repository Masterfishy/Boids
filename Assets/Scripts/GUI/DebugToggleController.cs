using UnityEngine;
using UnityEngine.UI;

public class DebugToggleController : MonoBehaviour
{
    public BoolReference Value;
    public Toggle toggle;

    private void OnEnable()
    {
        if (toggle != null && Value != null)
        {
            toggle.isOn = Value;
            toggle.onValueChanged.AddListener(OnValueChanged);
        }
    }

    private void OnDisable()
    {
        if (toggle != null)
        {
            toggle.onValueChanged.RemoveAllListeners();
        }
    }

    private void OnValueChanged(bool newValue)
    {
        if (Value != null)
        {
            Value.Value = newValue;
        }
    }
}
