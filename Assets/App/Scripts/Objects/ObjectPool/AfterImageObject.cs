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
    /// �c���̃A���t�@�l���X�V����
    /// </summary>
    protected void UpdateImageAlpha()
    {
        SetAfterImageColor(colorGradient.Evaluate(lifeTime / startLifeTime));
    }

    /// <summary>
    /// �{�[���̓��������b�V������c�����쐬����
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
    /// ���b�V������c�����쐬����
    /// </summary>
    /// <param name="lifeTime"></param>
    /// <param name="baseTrans"></param>
    /// <param name="mesh"></param>
    public void MakeByMesh(float lifeTime, Transform baseTrans, Mesh mesh, Gradient gradient)
    {
        // subMesh�̍ő吔��MeshRender�ɃZ�b�g����Ă���Material�̐��ɑΉ�
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
