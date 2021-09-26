using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeObject : MonoBehaviour
{
    [SerializeField]
    private MinMaxRange fragmentSize = new MinMaxRange(0.01f, 10.0f);
    [SerializeField]
    private MinMaxRange explodeSize = new MinMaxRange(0, 20.0f);

    [SerializeField]
    private int fragmentCount = 20;
    [SerializeField]
    private int explodeCount = 5;
    [SerializeField]
    private float explodeInterval = 0.05f;
    [SerializeField]
    private bool unGray = false;

    /// <summary>
    /// 爆発を起こす
    /// </summary>
    public void StartExplode()
    {
        StartCoroutine(Explode());
    }

    private void GenerateFragment()
    {
        for (int i = fragmentCount; i > 0; i--)
        {
            Vector3 explodeOrigin = transform.position;
            ExplosionFragment fragment = GameSceneManager.Instance.GetGrayExpFragmentPool().GetObject<ExplosionFragment>();

            // 1発だけは爆発地点中心に生成し、爆発範囲近くは確実に色が塗られるようにする
            if (i == fragmentCount)
            {
                fragment.DrawOfPosition(explodeOrigin, explodeSize.min, unGray);
            }
            else
            {
                Vector3 fragmentPos = explodeOrigin + Random.onUnitSphere * explodeSize.RandOfRange();
                fragment.DrawOfPosition(fragmentPos, fragmentSize.RandOfRange(), unGray);
            }
        }
    }
    
    private IEnumerator Explode()
    {
        for(int i = 0; i < explodeCount; i++)
        {
            GenerateFragment();
            yield return new WaitForSeconds(explodeInterval);
        }
    }
}
