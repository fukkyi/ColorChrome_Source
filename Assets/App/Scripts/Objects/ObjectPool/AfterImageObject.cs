using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshRenderer))]
public class AfterImageObject : PoolableObject
{
    private MeshRenderer meshRenderer = null;
    private MeshFilter meshFilter = null;
    private Mesh bakedMesh = null;
    private Gradient colorGradient = null;

    private float startLifeTime = 0;

    protected void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        bakedMesh = new Mesh();
    }

    protected void Update()
    {
        UpdateLifeTime();
        UpdateImageAlpha();
    }

    /// <summary>
    /// 残像のアルファ値を更新する
    /// </summary>
    protected void UpdateImageAlpha()
    {
        SetAfterImageColor(colorGradient.Evaluate(lifeTime / startLifeTime));
    }

    /// <summary>
    /// ボーンの入ったメッシュから残像を作成する
    /// </summary>
    /// <param name="lifeTime"></param>
    /// <param name="baseTrans"></param>
    /// <param name="smr"></param>
    public void MakeBySkinnedMesh(float lifeTime, Transform baseTrans, SkinnedMeshRenderer smr, Gradient gradient)
    {
        smr.BakeMesh(bakedMesh, true);
        MakeByMesh(lifeTime, baseTrans, bakedMesh, gradient);
    }

    /// <summary>
    /// メッシュから残像を作成する
    /// </summary>
    /// <param name="lifeTime"></param>
    /// <param name="baseTrans"></param>
    /// <param name="mesh"></param>
    public void MakeByMesh(float lifeTime, Transform baseTrans, Mesh mesh, Gradient gradient)
    {
        // subMeshの最大数はMeshRenderにセットされているMaterialの数に対応
        meshFilter.sharedMesh = mesh;

        startLifeTime = lifeTime;
        this.lifeTime = lifeTime;

        transform.position = baseTrans.position;
        transform.rotation = baseTrans.rotation;

        colorGradient = gradient;
    }

    public void SetAfterImageColor(Color color)
    {
        foreach (Material material in meshRenderer.materials)
        {
            material.SetColor("_BaseColor", color);
        }
    }
}
