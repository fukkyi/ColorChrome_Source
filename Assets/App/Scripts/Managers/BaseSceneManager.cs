using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSceneManager<Type> : MonoBehaviour where Type : BaseSceneManager<Type>
{
    public static Type Instance { get; private set; } = null;

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = (Type)this;
        }

        Init();
    }

    protected virtual void Init() { }
}
