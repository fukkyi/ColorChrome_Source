using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFlake : PoolableObject
{
    [SerializeField]
    private float eraseTime = 60;
    [SerializeField]
    private ItemType itemType;
    [SerializeField]
    private float addValue;
    [SerializeField]
    private MinMaxRange dropForceRangeX = new MinMaxRange(-1.0f, 1.0f);
    [SerializeField]
    private MinMaxRange dropForceRangeY = new MinMaxRange(-1.0f, 1.0f);
    [SerializeField]
    private MinMaxRange dropForceRangeZ = new MinMaxRange(-1.0f, 1.0f);

    private Rigidbody myRb = null;

    protected void Awake()
    {
        lifeTime = eraseTime;
        myRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLifeTime();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer != LayerMaskUtil.PlayerLayerNumber) return;

        GameSceneUIManager.Instance.ItemGauge.ItemGaugeUpdate(itemType, addValue);
        // 取得時にパーティクル再生
        GameSceneManager.Instance.ColorFlakeGetParticlePool.GetObject<ParticleObject>().PlayOfPosition(transform.position);

        AudioManager.Instance.PlayRandomPitchSE("Fantasy click sound 9", transform.position);
        
        DisableObject();
    }

    /// <summary>
    /// 特定座標でアイテムをドロップする
    /// </summary>
    /// <param name="position"></param>
    /// <param name="force"></param>
    public void DropOfPosition(Vector3 position, float force)
    {
        transform.position = position;

        float xForce = Random.Range(dropForceRangeX.min, dropForceRangeX.max);
        float yForce = Random.Range(dropForceRangeY.min, dropForceRangeY.max);
        float zForce = Random.Range(dropForceRangeZ.min, dropForceRangeZ.max);
        Vector3 dropVec = (Vector3.right * xForce + Vector3.up * yForce + Vector3.forward * zForce).normalized;

        myRb.velocity = dropVec * force;

        EnableObject();
    }
}
