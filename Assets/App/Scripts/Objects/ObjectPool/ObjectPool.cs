using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private PoolableObject poolTarget = null;
    [SerializeField]
    private Transform poolParent = null;
    [SerializeField]
    private int poolSize = 100;

    private PoolableObject[] poolObjects = new PoolableObject[0];
    private int currentElement = 0;

    private void Awake()
    {
        if (poolParent == null)
        {
            poolParent = transform;
        }

        GeneratePoolObject();
    }

    /// <summary>
    /// �v�[���p�̃I�u�W�F�N�g�𐶐�����
    /// </summary>
    private void GeneratePoolObject()
    {
        System.Array.Resize(ref poolObjects, poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            poolObjects[i] = Instantiate(poolTarget, poolParent);
            poolObjects[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// �v�[�����O���̃I�u�W�F�N�g���擾����
    /// </summary>
    /// <typeparam name="Type"></typeparam>
    /// <returns></returns>
    public Type GetObject<Type>() where Type : PoolableObject
    {
        PoolableObject returnObject = poolObjects[currentElement];
        returnObject.EnableObject();

        currentElement++;
        if (currentElement >= poolSize)
        {
            currentElement = 0;
        }

        return (Type)returnObject;
    }
}
