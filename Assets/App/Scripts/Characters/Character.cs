using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    protected Animator animator = null;
    [SerializeField]
    protected Renderer[] myRenderers = null;
    [SerializeField]
    protected int maxHp = 100;

    protected bool isDead = false;
    protected int currentHp = 0;
    protected int deadHp = 0;

    protected string deadAnimName = "Dead";

    protected void SetHpToMax()
    {
        currentHp = maxHp;
    }

    /// <summary>
    /// ダメージを受けた時の処理
    /// </summary>
    /// <param name="attackPower"></param>
    public virtual void TakeDamage(int attackPower)
    {
        currentHp = Mathf.Clamp(currentHp - attackPower, 0, maxHp);

        if (currentHp <= deadHp)
        {
            Dead();
        }
    }

    [ContextMenu("Dead")]
    /// <summary>
    /// 敵を倒させる
    /// </summary>
    protected virtual void Dead()
    {
        if (isDead) return;

        isDead = true;
        OnDead();
    }

    /// <summary>
    /// 倒された際の処理
    /// </summary>
    protected virtual void OnDead() {}

    /// <summary>
    /// レンダラーの表示、非表示を設定する
    /// </summary>
    protected void SetEnableRenderers(bool enabled)
    {
        foreach (Renderer renderer in myRenderers)
        {
            renderer.enabled = enabled;
        }
    }

    public int GetMaxHp()
    {
        return maxHp;
    }

    public int GetCurrentHp()
    {
        return currentHp;
    }
}
