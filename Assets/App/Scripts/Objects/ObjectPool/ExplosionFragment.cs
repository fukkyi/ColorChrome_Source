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
    /// éwíËÇµÇΩç¿ïWÇìhÇÈ
    /// </summary>
    /// <param name="position"></param>
    /// <param name="drawRadius"></param>
    public void DrawOfPosition(Vector3 position, float drawRadius)
    {
        transform.position = position;
        drawObject.drawRadius = drawRadius;

        DrawOnce();
    }

    private void DrawOnce()
    {
        drawObject.DrawToGrayableObject();
        DisableObject();
    }
}
