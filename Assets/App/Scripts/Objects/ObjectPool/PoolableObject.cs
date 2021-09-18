using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    protected float lifeTime = 0;

    public virtual void EnableObject()
    {
        gameObject.SetActive(true);
    }

    public virtual void DisableObject()
    {
        gameObject.SetActive(false);
    }

    protected void UpdateLifeTime()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            OnTimeOut();
        }
    }

    protected virtual void OnTimeOut()
    {
        DisableObject();
    }
}
