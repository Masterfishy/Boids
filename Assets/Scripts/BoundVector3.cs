using UnityEngine;

[CreateAssetMenu(menuName ="BoundVector3")]
public class BoundVector3 : ScriptableObject
{
    public Vector3 Size;
    
    public float XMax
    {
        get
        {
            return Size.x / 2;
        }
    }

    public float XMin
    {
        get
        {
            return -Size.x / 2;
        }
    }

    public float YMax
    {
        get
        {
            return Size.y / 2;
        }
    }

    public float YMin
    {
        get
        {
            return -Size.y / 2;
        }
    }

    public float ZMax
    {
        get
        {
            return Size.z / 2;
        }
    }

    public float ZMin
    {
        get
        {
            return -Size.z / 2;
        }
    }
}
