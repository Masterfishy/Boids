using UnityEngine;

/// <summary>
/// Base class for ScriptableObject references for built in types
/// </summary>
/// <typeparam name="T">The type of the reference</typeparam>
public abstract class ValueReference<T> : ScriptableObject
{
    [SerializeField]
    protected T m_InitialValue;

    protected T m_RuntimeValue;

    private void OnEnable()
    {
        m_RuntimeValue = m_InitialValue;
    }

    private void OnValidate()
    {
        m_RuntimeValue = m_InitialValue;
    }

    /// <summary>
    /// Get the runtime value of the ValueReference.
    /// </summary>
    public static implicit operator T(ValueReference<T> reference)
    {
        return reference.m_RuntimeValue;
    }

    /// <summary>
    /// Create a new ValueReference from a given value.
    /// </summary>
    public static implicit operator ValueReference<T>(T value)
    {
        ValueReference<T> instance = CreateInstance<ValueReference<T>>();
        instance.m_RuntimeValue = value;
        return instance;
    }

    public T Value
    {
        get { return m_RuntimeValue; }
        set { m_RuntimeValue = value; }
    }
}
