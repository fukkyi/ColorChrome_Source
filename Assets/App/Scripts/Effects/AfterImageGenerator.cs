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
    /// 一定間隔で残像を生成する
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
    /// 残像を生成する
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
    /// 残像を生成し始める
    /// </summary>
    public void StartGenerate()
    {
        isEnable = true;

        currentInterval = generateInterval;
        MakeAfterImage();
    }

    /// <summary>
    /// 残像の生成を止める
    /// </summary>
    public void StopGenerate()
    {
        isEnable = false;
    }
}
