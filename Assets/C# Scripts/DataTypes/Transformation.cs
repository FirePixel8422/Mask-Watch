using UnityEngine;



[System.Serializable]
public struct Transformation
{
    public Vector3 Position;
    public Vector3 RotationEuler;
    public Vector3 Scale;
    public Quaternion Rotation => Quaternion.Euler(RotationEuler);


    public static Transformation Identity => new Transformation
    {
        Position = Vector3.zero,
        RotationEuler = Vector3.zero,
        Scale = Vector3.one
    };

    public static Transformation FromTransform(Transform t)
    {
        return new Transformation
        {
            Position = t.localPosition,
            RotationEuler = t.localEulerAngles,
            Scale = t.localScale
        };
    }
    public void ApplyToTransform(Transform t)
    {
        t.SetLocalPositionAndRotation(Position, Rotation);
        t.localScale = Scale;
    }
}