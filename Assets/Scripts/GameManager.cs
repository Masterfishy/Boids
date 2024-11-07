using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject BoidPrefab;
    public uint TotalBoids;
    public BoundVector3 WorldDimensions;

    // Start is called before the first frame update
    void Start()
    {
        if (WorldDimensions == null)
        {
            return;
        }

        float xMax = WorldDimensions.XMax;
        float xMin = WorldDimensions.XMin;

        float yMax = WorldDimensions.YMax;
        float yMin = WorldDimensions.XMin;

        float zMax = WorldDimensions.ZMax;
        float zMin = WorldDimensions.ZMin;

        Vector3 boidPosition = Vector3.zero;
        Quaternion boidRotation;

        for (uint i = 0; i < TotalBoids; i++)
        {
            // Create a random position
            boidPosition.x = Random.Range(xMin, xMax);
            boidPosition.y = Random.Range(yMin, yMax);
            boidPosition.z = Random.Range(zMin, zMax);

            // Create a random rotation
            boidRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

            // Spawn a boid
            Instantiate(BoidPrefab, boidPosition, boidRotation, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (WorldDimensions != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero, WorldDimensions.Size);
        }
    }
}
