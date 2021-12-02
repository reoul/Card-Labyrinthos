using System;
using UnityEngine;

[Serializable]
public class PRS
{
    public Vector3 Pos { get; set; }
    public Quaternion Rot { get; set; }
    public Vector3 Scale { get; set; }

    public PRS(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        Pos = pos;
        Rot = rot;
        Scale = scale;
    }
}

public class Utils
{
    public static Quaternion CardRotate => Quaternion.AngleAxis(90, Vector3.up);

    public static Vector3 MousePos
    {
        get
        {
            var result = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            result.z = -10;
            return result;
        }
    }
}
