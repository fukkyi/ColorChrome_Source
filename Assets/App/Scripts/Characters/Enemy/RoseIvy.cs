using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoseIvy : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem attackWaitParticle = null;
    [SerializeField]
    private EnemyAttackCollider[] spawnAttackColliders = null;
    [SerializeField]
    private EnemyAttackCollider[] normalAttackColliders = null;
    [SerializeField]
    private EnemyAttackCollider[] spinAttackColliders = null;

    [SerializeField]
    private float rotateSpeed = 200;
    [SerializeField]
    private float attackWaitTime = 1.0f;
    [SerializeField]
    private int spawnAttackPower = 10;
    [SerializeField]
    private int normalAttackPower = 15;
    [SerializeField]
    private int spinAttackPower = 15;

    private Animator animator = null;
    private Player detectPlayer = null;

    private bool isRotateToPlayer = false;
    private string spawnAttackAnimName = "Attack_1";
    private string normalAttackAnimName = "Attack_2";
    private string spinAttackAnimName = "Attack_3";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        detectPlayer = Player.GetPlayer();

        gameObject.SetActive(false);
    }

    private void Update()
    {
        // プレイヤー追尾中の場合はプレイヤーに向かせる
        if (isRotateToPlayer)
        {
            transform.rotation = RotateTowardsToDetectPlayer(rotateSpeed);
        }
    }

    public void PlaySpawnAttack(Vector3 attackPos)
    {
        PlayAttack(attackPos, spawnAttackAnimName);
    }

    public void PlayNormalAttack(Vector3 attackPos)
    {
        PlayAttack(attackPos, normalAttackAnimName);
    }

    public void PlaySpinAttack(Vector3 attackPos)
    {
        PlayAttack(attackPos, spinAttackAnimName);
    }

    private void PlayAttack(Vector3 attackPos, string triggerName)
    {
        gameObject.SetActive(true);
        transform.position = attackPos;

        if (!gameObject.activeSelf) return;

        StartCoroutine(WaitAttackAnim(attackPos, triggerName));
    }

    private IEnumerator WaitAttackAnim(Vector3 attackPos, string triggerName)
    {
        attackWaitParticle.Play();

        yield return new WaitForSeconds(attackWaitTime);

        animator.SetTrigger(triggerName);
        AudioManager.Instance.PlayRandomPitchSE("Stone Impact 4", attackPos);
    }

    private void SetAttackValueForAllCollider(int power)
    {
        SetAttackPowerForCollider(spawnAttackColliders, power);
        SetAttackPowerForCollider(normalAttackColliders, power);
        SetAttackPowerForCollider(spinAttackColliders, power);
    }

    /// <summary>
    /// 攻撃判定のダメージ量を変える
    /// </summary>
    /// <param name="attackColliders"></param>
    /// <param name="power"></param>
    private void SetAttackPowerForCollider(EnemyAttackCollider[] attackColliders, int power)
    {
        foreach(EnemyAttackCollider attackCollider in attackColliders)
        {
            attackCollider.damageValue = power;
        }
    }

    /// <summary>
    /// 攻撃判定の有効化を替える
    /// </summary>
    /// <param name="attackColliders"></param>
    /// <param name="enable"></param>
    private void SetEnabledAttackColliders(EnemyAttackCollider[] attackColliders, bool enabled)
    {
        foreach (EnemyAttackCollider attackCollider in attackColliders)
        {
            if (enabled)
            {
                attackCollider.EnableCollider();
            }
            else
            {
                attackCollider.DisableCollider();
            }
        }
    }

    /// <summary>
    /// 検知したプレイヤーに向けて徐々に回転する
    /// </summary>
    /// <param name="rotateSpeed"></param>
    /// <returns></returns>
    protected Quaternion RotateTowardsToDetectPlayer(float rotateSpeed, bool igroneYAxis = true)
    {
        if (detectPlayer == null) return Quaternion.identity;

        Vector3 playerPos = detectPlayer.transform.position;
        return RotateTowardsToPosition(playerPos, rotateSpeed, igroneYAxis);
    }

    /// <summary>
    /// 特定の座標に向けて徐々に回転させる
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotateSpeed"></param>
    /// <param name="igroneYAxis"></param>
    /// <returns></returns>
    protected Quaternion RotateTowardsToPosition(Vector3 position, float rotateSpeed, bool igroneYAxis = true)
    {
        Vector3 myPos = transform.position;
        // Y軸を無視する場合はY座標を0として計算する
        if (igroneYAxis)
        {
            position.y = 0;
            myPos.y = 0;
        }
        Vector3 rotateDirection = position - myPos;

        return Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rotateDirection), rotateSpeed * Time.deltaTime);
    }

    private void OnSpawnAttackColliderEneble()
    {
        SetAttackValueForAllCollider(spawnAttackPower);
        SetEnabledAttackColliders(spawnAttackColliders, true);
    }

    private void OnSpawnAttackColliderDisable()
    {
        SetEnabledAttackColliders(spawnAttackColliders, false);
    }

    private void OnNormalAttackColliderEneble()
    {
        SetAttackValueForAllCollider(normalAttackPower);
        SetEnabledAttackColliders(normalAttackColliders, true);
    }

    private void OnNormalAttackColliderDisable()
    {
        SetEnabledAttackColliders(normalAttackColliders, false);
    }

    private void OnSpinAttackColliderEneble()
    {
        SetAttackValueForAllCollider(spinAttackPower);
        SetEnabledAttackColliders(spinAttackColliders, true);
    }

    private void OnSpinAttackColliderDisable()
    {
        SetEnabledAttackColliders(spinAttackColliders, false);
    }

    private void OnEnableRotateToPlayer()
    {
        isRotateToPlayer = true;
    }

    private void OnDisableRotateToPlayer()
    {
        isRotateToPlayer = false;
    }

    private void OnFinishedAttack()
    {
        gameObject.SetActive(false);
    }
}
