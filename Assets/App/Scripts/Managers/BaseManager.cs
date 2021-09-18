using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseManager<Type> : MonoBehaviour where Type : BaseManager<Type>
{
    private static Type instance = null;
    public static Type Instance
    {
        get
        {
            GenerateInstance();
            return instance;
        }
        private set { instance = value; }
    }

    public static void GenerateInstance()
    {
        if (instance != null) return;

        GameObject managerObject = new GameObject(typeof(Type).Name);
        instance = managerObject.AddComponent<Type>();

        DontDestroyOnLoad(instance);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = (Type)this;
            DontDestroyOnLoad(instance);
        }

        Init();
    }

    protected virtual void Init() { }
}
