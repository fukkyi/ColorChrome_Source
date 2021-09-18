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

        // �v���C���[�ɂ̂ݓ���������
        if (col.gameObject.tag == "Player")
        {
            enemyHp.SetActive(true);
            Debug.Log("�U�������������I");
        }
    }

    // �����ɍs�����������̂ł͂Ȃ����@�U��������̂ň�莞�ԍU�����Ȃ������������
    public void OnTriggerExit(Collider col)
    {
        // player�ɂ̂ݔ����A���@�ƍU���ɍ��킹��
        if (col.gameObject.tag == "Player")
        {
            Invoke(nameof(DelayHpDelet), 5.0f);
            Debug.Log("������Ȃ��Ȃ���");
        }
    }

    public void DelayHpDelet()
    {
        enemyHp.SetActive(false);
    }

}
