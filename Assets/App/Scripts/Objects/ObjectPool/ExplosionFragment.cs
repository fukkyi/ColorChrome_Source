using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFragment : PoolableObject
{
    private DrawObject drawObject = null;

    private void Awake()
    {
        drawObject = GetComponent<DrawObject>();
        drawObject.allowUpdateDraw = false;
    }

    /// <summary>
    /// 指定した座標を塗る
    /// </summary>
    /// <param name="position"></param>
    /// <param name="drawRadius"></param>
    public void DrawOfPosition(Vector3 position, float drawRadius, bool unGray = false)
    {
        transform.position = position;
        drawObject.drawRadius = drawRadius;
        drawObject.unGray = unGray;

        DrawOnce();
    }

    private void DrawOnce()
    {
        drawObject.DrawToGrayableObject();
        DisableObject();
    }
}
