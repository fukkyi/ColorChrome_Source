using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageGenerator : MonoBehaviour
{
    [SerializeField]
    private Gradient afterImageGradient = null;
    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer = null;
    [SerializeField]
    private MeshFilter meshFilter = null;
    [SerializeField]
    private float generateInterval = 0.05f;
    [SerializeField]
    private float lifeTime = 0.5f;

    private bool isEnable = false;
    private float currentInterval = 0;

    // Update is called once per frame
    void Update()
    {
        MakeAfterImageByInterval();
    }

    /// <summary>
    /// ���Ԋu�Ŏc���𐶐�����
    /// </summary>
    private void MakeAfterImageByInterval()
    {
        if (!isEnable) return;
        if (skinnedMeshRenderer == null && meshFilter == null) return;

        currentInterval = Mathf.Clamp(currentInterval -= Time.deltaTime, 0, generateInterval);
        if (currentInterval > 0) return;

        currentInterval = generateInterval;

        MakeAfterImage();
    }

    /// <summary>
    /// �c���𐶐�����
    /// </summary>
    public void MakeAfterImage()
    {
        AfterImageObject afterImageObject = GameSceneManager.Instance.AfterImagePool.GetObject<AfterImageObject>();

        if (skinnedMeshRenderer != null)
        {
            afterImageObject.MakeBySkinnedMesh(lifeTime, transform, skinnedMeshRenderer, afterImageGradient);
        }
        else if (meshFilter != null)
        {
            afterImageObject.MakeByMesh(lifeTime, transform, meshFilter.mesh, afterImageGradient);
        }
    }

    /// <summary>
    /// �c���𐶐����n�߂�
    /// </summary>
    public void StartGenerate()
    {
        isEnable = true;

        currentInterval = generateInterval;
        MakeAfterImage();
    }

    /// <summary>
    /// �c���̐������~�߂�
    /// </summary>
    public void StopGenerate()
    {
        isEnable = false;
    }
}
