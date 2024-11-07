using UnityEngine;

[CreateAssetMenu(menuName = "Value References/EvaluationCurveReference")]
public class EvaluationCurveReference : ScriptableObject
{
    [SerializeField]
    private AnimationCurve m_InitialCurve;

    private AnimationCurve m_RunTimeCurve;

    private void OnEnable()
    {
        m_RunTimeCurve = m_InitialCurve;
    }

    private void OnValidate()
    {
        m_RunTimeCurve = m_InitialCurve;
    }

    public float Evaluate(float time)
    {
        return m_RunTimeCurve.Evaluate(time);
    }
}
