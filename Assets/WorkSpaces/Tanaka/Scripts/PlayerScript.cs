using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerScript : MonoBehaviour
{
    //[SerializeField] private GameObject Player;
    [SerializeField] private Vector3 velocity;              // 移動方向
    [SerializeField] private float moveSpeed = 15.0f;        // 移動速度
    [SerializeField] private float applySpeed = 0.2f;       // 振り向きの適用速度
    [SerializeField] private float gravity = 20.0f;         // 重力の大きさ
    [SerializeField] private PlayerFollowCamera refCamera;  // カメラの水平転を参照する用

    [SerializeField] private GameObject redFragment;
    [SerializeField] private GameObject greenFragment;
    [SerializeField] private GameObject blueFragment;

    public bool redFrag = false;
    public bool greenFrag = false;
    public bool blueFrag = false;

    private Rigidbody rb;
    private Rigidbody PlayerRigid;

    private ColorGauge colorGauge;

    //[SerializeField] private float Upspeed;

    void Start()
    {
        //PlayerRigid = rb.GetComponent<Rigidbody>();
    }

    void Update()
    {
        /*プレイヤーの移動*/
        // WASD入力から、XZ平面(水平な地面)を移動する方向(velocity)を得ます
        velocity = Vector3.zero;

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            velocity.z += 1;
            //Debug.Log("w");
        }
        else if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            velocity.x -= 1;
            //Debug.Log("a");
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            velocity.z -= 1;
            //Debug.Log("s");
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            velocity.x += 1;
            //Debug.Log("d");
        }

        // 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
        velocity = velocity.normalized * moveSpeed * Time.deltaTime;

        /*プレイヤーの向き*/
        // いずれかの方向に移動している場合
        if (velocity.magnitude > 0)
        {
            //    // プレイヤーの回転(transform.rotation)の更新
            //    // 無回転状態のプレイヤーのZ+方向(後頭部)を、
            //    // カメラの水平回転(refCamera.hRotation)で回した移動の反対方向(-velocity)に回す回転に段々近づけます
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(refCamera.hRotation * -velocity),
                                                  applySpeed);

            //    // プレイヤーの位置(transform.position)の更新
            //    // カメラの水平回転(refCamera.hRotation)で回した移動方向(velocity)を足し込みます
            transform.position += refCamera.hRotation * velocity;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RedFrag")
        {
            redFrag = true;
            colorGauge.ColorRedFrag();
            Debug.Log("赤");
        }
        else if (other.gameObject.tag == "GreenFrag")
        {
            greenFrag = true;
            colorGauge.ColorGreenFrag();
            Debug.Log("緑");
        }
        else if (other.gameObject.tag == "BlueFrag")
        {
            blueFrag = false;
            colorGauge.ColorBlueFrag();
            Debug.Log("青");
        }
    }

    //damageを受けるとき
    //貫通する場合はTrigger系(どちらかにcolliderのis triggerをチェック)衝突しあうものはcollision系(ColliderとRigidbodyが必要)

    // INPUTSYSTEMSメモ----------------------------------------------------------------------------------------------
    // InputSystemはEventTriggerのような感じで使えるかも
    // アタッチしてあるPlayerinputから ▷Events ▷PlayerActionCommandの中の
    //Attackに該当するScriptをアタッチして用意した動きをするように作ったclassを入れておく

    // private void aaa(CallbackContext.context){}
}