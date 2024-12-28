using UnityEngine;
using UnityEngine.UI;

public class DebugSliderController : MonoBehaviour
{
    public bool m_WholeNumbers;
    public IntReference m_IntValue;
    public FloatReference m_FloatValue;
    public Slider m_Slider;

    private void OnEnable()
    {
        if (m_Slider == null)
        {
            return;
        }

        m_Slider.wholeNumbers = m_WholeNumbers;
        m_Slider.onValueChanged.AddListener(OnValueChanged);

        if (m_WholeNumbers && m_IntValue != null)
        {
            m_Slider.value = m_IntValue;
        }

        if (!m_WholeNumbers && m_FloatValue != null)
        {
            m_Slider.value = m_FloatValue;
        }
    }

    private void OnDisable()
    {
        if (m_Slider != null)
        {
            m_Slider.onValueChanged.RemoveAllListeners();
        }
    }

    private void OnValueChanged(float newValue)
    {
        if (m_WholeNumbers && m_IntValue != null)
        {
            m_IntValue.Value = Mathf.FloorToInt(newValue);
        }

        if (!m_WholeNumbers && m_FloatValue != null)
        {
            m_FloatValue.Value = newValue;
        }
    }
}
