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
        // ���^���̎��͓Ghp��\�������Ȃ�
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
    //�Ԃ�������fillamout�����点�΂���
    //�Ԃ���������̓^�O�ŒT��
    void OnCollisionEnter(Collision collision)
    {
        //Enemy��tag��T����hp�����炷
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("if�ʂ��Ă��");
            enemyMaxHp -= 10f;
        }
    }
    */

    /// <summary>
    /// �Q�[�W�ʂ��X�V����
    /// </summary>
    public void UpdateGaugeFillAmount(float hp, float maxHp)
    {
        //HP��color��΂���Ԃɂ���
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

    //    // �v���C���[�ɂ̂ݓ���������
    //    if (col.gameObject.tag == "Player")
    //    {
    //        enemyHp.SetActive(true);
    //        Debug.Log("�U�������������I");
    //    }
    //}

    //// �����ɍs�����������̂ł͂Ȃ����@�U��������̂ň�莞�ԍU�����Ȃ������������
    //public void OnTriggerExit(Collider col)
    //{
    //    // player�ɂ̂ݔ����A���@�ƍU���ɍ��킹��
    //    if (col.gameObject.tag == "Player")
    //    {
    //        Invoke(nameof(DelayHpDelet), 5.0f);
    //        Debug.Log("������Ȃ��Ȃ���");
    //    }
    //}

    //public void DelayHpDelet()
    //{
    //    enemyHp.SetActive(false);
    //}
}
