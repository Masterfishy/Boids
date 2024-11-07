using UnityEngine;

public class BoidController : MonoBehaviour
{


    public BoundVector3 WorldDimensions;
    public float Speed;

    [Header("Collision Detection")]
    public BoolReference Debug_ShowCollisionVectors;
    public BoolReference Debug_ShowAvoidVectors;
    public IntReference NumberOfCollisionVectors;
    public FloatReference CollisionDistance;
    public EvaluationCurveReference CollisionCurve;
    public FloatReference CollisionFov;

    private Vector3 m_Velocity;
    private Vector3 m_NextPosition;
    private Vector3[] m_CollisionVectors;

    // Start is called before the first frame update
    void Start()
    {
        float zRotation = Mathf.Deg2Rad * transform.rotation.eulerAngles.z;
        m_Velocity.x = Speed * Mathf.Cos(zRotation);
        m_Velocity.y = Speed * Mathf.Sin(zRotation);

        m_NextPosition = transform.position;

        if (NumberOfCollisionVectors != null)
        {
            m_CollisionVectors = new Vector3[NumberOfCollisionVectors];

            for (uint i = 0; i < NumberOfCollisionVectors; i++)
            {
                float angle = i * 2 * Mathf.PI / NumberOfCollisionVectors;
                m_CollisionVectors[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f).normalized;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ApplyCollisionAvoidance(ref m_Velocity);

        m_NextPosition.x = transform.position.x + m_Velocity.x * Time.deltaTime;
        m_NextPosition.y = transform.position.y + m_Velocity.y * Time.deltaTime;

        ApplyWorldWrap(ref m_NextPosition);

        transform.position = m_NextPosition;

        float zRotation = Mathf.Atan2(m_Velocity.y, m_Velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }

    private void ApplyCollisionAvoidance(ref Vector3 velocity)
    {
        Vector3 velocityDirection = velocity.normalized;

        for (uint i = 0; i < NumberOfCollisionVectors; i++)
        {
            RaycastHit2D result = Physics2D.Raycast(transform.position, m_CollisionVectors[i], CollisionDistance);
            if (result)
            {
                float weight = 1 - (result.distance / CollisionDistance);
                velocityDirection += -CollisionCurve.Evaluate(weight) * m_CollisionVectors[i];

                if (Debug_ShowAvoidVectors)
                {
                    Debug.DrawRay(transform.position, CollisionDistance * CollisionCurve.Evaluate(weight) * m_CollisionVectors[i], Color.red);
                }
            }
        }

        velocity = Speed * velocityDirection;
    }

    /// <summary>
    /// Transform the given position to a position within the WorldDimensions.
    /// </summary>
    /// <param name="position">The position to transform</param>
    private void ApplyWorldWrap(ref Vector3 position)
    { 
        if (WorldDimensions == null)
        {
            return;
        }

        // Check the x component
        if (position.x > WorldDimensions.XMax)
        {
            position.x = WorldDimensions.XMin;
        }
        else if (position.x < WorldDimensions.XMin)
        {
            position.x = WorldDimensions.XMax;
        }

        // Check the y component
        if (position.y > WorldDimensions.YMax)
        {
            position.y = WorldDimensions.YMin;
        }
        else if (position.y < WorldDimensions.YMin)
        {
            position.y = WorldDimensions.YMax;
        }

        // Check the z component
        if (position.z > WorldDimensions.ZMax)
        {
            position.z = WorldDimensions.ZMin;
        }
        else if (position.z < WorldDimensions.ZMin)
        {
            position.z = WorldDimensions.ZMax;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, m_Velocity.normalized);

        if (Debug_ShowCollisionVectors)
        {
            Gizmos.color = Color.yellow;
            for (uint i = 0; i < NumberOfCollisionVectors; i++)
            {
                Gizmos.DrawRay(transform.position, CollisionDistance * m_CollisionVectors[i]);
            }
        }
    }
}
