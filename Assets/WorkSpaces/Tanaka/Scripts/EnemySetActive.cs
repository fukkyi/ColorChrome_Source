using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySetActive : MonoBehaviour
{
    [SerializeField] EnemyUI _enemyUI;
    [SerializeField] GameObject enemyHp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider col)
    {

        // プレイヤーにのみ当たった時
        if (col.gameObject.tag == "Player")
        {
            enemyHp.SetActive(true);
            Debug.Log("攻撃が当たった！");
        }
    }

    // 遠くに行ったら消えるのではなく魔法攻撃もあるので一定時間攻撃がなかったら消える
    public void OnTriggerExit(Collider col)
    {
        // playerにのみ反応、魔法と攻撃に合わせる
        if (col.gameObject.tag == "Player")
        {
            Invoke(nameof(DelayHpDelet), 5.0f);
            Debug.Log("当たらなくなった");
        }
    }

    public void DelayHpDelet()
    {
        enemyHp.SetActive(false);
    }

}
