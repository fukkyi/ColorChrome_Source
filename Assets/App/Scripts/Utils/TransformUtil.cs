using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformUtil
{
    /// <summary>
    /// 一周の度数
    /// </summary>
    public const float AroundDegAngle = 360;

    /// <summary>
    /// 箱状のエリア内でランダムな座標を返す
    /// </summary>
    /// <param name="center"></param>
    /// <param name="boxSize"></param>
    /// <returns></returns>
    public static Vector3 GetRandPosByBox(Vector3 center, Vector3 boxSize)
    {
        Vector3 randPos = center;
        randPos.x += Random.Range(-boxSize.x / 2, boxSize.x / 2);
        randPos.y += Random.Range(-boxSize.y / 2, boxSize.y / 2);
        randPos.z += Random.Range(-boxSize.z / 2, boxSize.z / 2);

        return randPos;
    }

    /// <summary>
    /// Y軸を無視した距離を返す
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Calc2DDistance(Vector3 a, Vector3 b)
    {
        a.y = 0;
        b.y = 0;
        return Vector3.Distance(a, b);
    }

    /// <summary>
    /// Y軸を無視した目標への方向を返す
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static Vector3 Calc2DDirection(Vector3 origin, Vector3 target)
    {
        origin.y = 0;
        target.y = 0;
        return (target - origin).normalized;
    }

    /// <summary>
    /// Y軸を無視した目標への方向を前方を基準とした3次元ベクトルで返す
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static Vector3 Calc2DDirectionForVector3(Vector3 origin, Vector3 target)
    {
        Vector2 direction2D = Calc2DDirection(origin, target);
        return Vector3.right * direction2D.x + Vector3.forward * direction2D.y;
    }
}
