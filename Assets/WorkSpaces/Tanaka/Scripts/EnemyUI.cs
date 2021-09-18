using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] Image enemyHPGauge;
    [SerializeField] Transform enemyHPGaugeTrans;
    [SerializeField] GameObject enemyHp;

    private Transform cameraTrans = null;

    //------------------------------------------------------------------------------------
    [SerializeField] Vector3 baseScale;
    //------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        // 満タンの時は敵hpを表示させない
        enemyHp.SetActive(false);

        // _image = GetComponent<Image>();
        cameraTrans = PlayerCamera.GetPlayerCameraTrans();

        //------------------------------------------------------------------------------------
        //baseScale = transform.localScale / GetDistance();
        //------------------------------------------------------------------------------------
    }

    // Update is called once per frame
    void Update()
    {
        enemyHPGaugeTrans.LookAt(cameraTrans);
        //------------------------------------------------------------------------------------
        transform.localScale = baseScale * GetDistance();
        //------------------------------------------------------------------------------------
    }

    //------------------------------------------------------------------------------------
    float GetDistance()
    {
        //return (transform.position - Camera.main.transform.position).magnitude;
        return (transform.position - cameraTrans.position).magnitude;
    }
    //------------------------------------------------------------------------------------

    /*
    //ぶつかったらfillamoutを減らせばいい
    //ぶつかった相手はタグで探す
    void OnCollisionEnter(Collision collision)
    {
        //Enemyのtagを探してhpを減らす
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("if通ってるよ");
            enemyMaxHp -= 10f;
        }
    }
    */

    /// <summary>
    /// ゲージ量を更新する
    /// </summary>
    public void UpdateGaugeFillAmount(float hp, float maxHp)
    {
        //HPのcolorを緑から赤にする
        if (enemyHPGauge.fillAmount > 0.75f)
        {
            enemyHPGauge.color = Color.green;
            //enemyHp.SetActive(true);
        }
        else if (enemyHPGauge.fillAmount > 0.3f)
            enemyHPGauge.color = new Color(1f, 127f / 255f, 39f / 255f, 0.7f);
        else
            enemyHPGauge.color = Color.red;

        if (enemyHPGauge.fillAmount == 0f)
            CancelInvoke();

        enemyHPGauge.fillAmount = hp / maxHp;
        //hp / maxHp = enemyHp.SetActive(true);
    }

    public void ShowHp()
    {
        gameObject.SetActive(true);
    }

    public void HideHp()
    {
        gameObject.SetActive(false);
    }

    //public void OnTriggerStay(Collider col)
    //{

    //    // プレイヤーにのみ当たった時
    //    if (col.gameObject.tag == "Player")
    //    {
    //        enemyHp.SetActive(true);
    //        Debug.Log("攻撃が当たった！");
    //    }
    //}

    //// 遠くに行ったら消えるのではなく魔法攻撃もあるので一定時間攻撃がなかったら消える
    //public void OnTriggerExit(Collider col)
    //{
    //    // playerにのみ反応、魔法と攻撃に合わせる
    //    if (col.gameObject.tag == "Player")
    //    {
    //        Invoke(nameof(DelayHpDelet), 5.0f);
    //        Debug.Log("当たらなくなった");
    //    }
    //}

    //public void DelayHpDelet()
    //{
    //    enemyHp.SetActive(false);
    //}
}
