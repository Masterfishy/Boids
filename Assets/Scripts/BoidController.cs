using UnityEngine;

public class BoidController : MonoBehaviour
{
    public BoundVector3 WorldDimensions;
    public FloatReference Speed;

    [Header("Collision Detection")]
    public BoolReference Debug_ShowCollisionVectors;
    public BoolReference Debug_ShowAvoidVectors;
    public BoolReference Debug_ShowVelocityVector;
    public IntReference NumberOfCollisionVectors;
    public FloatReference CollisionDistance;
    public EvaluationCurveReference CollisionCurve;
    public FloatReference CollisionFov;

    private Vector3 m_Velocity;
    private Vector3 m_NextPosition;

    // Start is called before the first frame update
    void Start()
    {
        float zRotation = Mathf.Deg2Rad * transform.rotation.eulerAngles.z;
        m_Velocity.x = Speed * Mathf.Cos(zRotation);
        m_Velocity.y = Speed * Mathf.Sin(zRotation);

        m_NextPosition = transform.position;
    }

    /// <summary>
    /// Get the velocity of the Boid
    /// </summary>
    /// <returns>The velocity of the boid</returns>
    public Vector3 GetVelocityDirection()
    {
        return m_Velocity.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyCollisionAvoidance(ref m_Velocity);
        //ApplyVelocityMatching(ref m_Velocity);

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

        float rotationOffset = Mathf.Atan2(velocity.y, velocity.x) - (Mathf.Deg2Rad * CollisionFov) / 2;

        for (int i = 0; i < NumberOfCollisionVectors; i++)
        {
            float angle = i * Mathf.Deg2Rad * CollisionFov / NumberOfCollisionVectors;
            angle += rotationOffset;
            Vector3 collisionRay = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f).normalized;

            if (Debug_ShowCollisionVectors)
            {
                Debug.DrawRay(transform.position, collisionRay * CollisionDistance, Color.yellow);
            }

            RaycastHit2D result = Physics2D.Raycast(transform.position, collisionRay, CollisionDistance);
            if (result)
            {
                float weight = 1 - (result.distance / CollisionDistance);
                velocityDirection += -CollisionCurve.Evaluate(weight) * collisionRay;

                if (Debug_ShowAvoidVectors)
                {
                    Debug.DrawRay(transform.position, CollisionDistance * CollisionCurve.Evaluate(weight) * collisionRay, Color.red);
                }
            }
        }

        velocity = Speed * velocityDirection;
    }

    private void ApplyVelocityMatching(ref Vector3 velocity)
    {
        Vector3 velocityDirection = velocity.normalized;

        float rotationOffset = Mathf.Atan2(velocity.y, velocity.x) - (Mathf.Deg2Rad * CollisionFov) / 2;

        for (int i = 0; i < NumberOfCollisionVectors; i++)
        {
            float angle = i * Mathf.Deg2Rad * CollisionFov / NumberOfCollisionVectors;
            angle += rotationOffset;
            Vector3 collisionRay = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f).normalized;

            RaycastHit2D result = Physics2D.Raycast(transform.position, collisionRay, CollisionDistance);
            if (result && result.transform.gameObject.TryGetComponent(out BoidController boid))
            {
                float weight = 1 - (result.distance / CollisionDistance);

                velocityDirection += CollisionCurve.Evaluate(weight) * boid.GetVelocityDirection();
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
        if (Debug_ShowVelocityVector)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, m_Velocity.normalized);
        }
    }
}
