using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [SerializeField] GameObject _enemy;
    [SerializeField] Image _image;
    public float _myHp = 200.0f;

    // Start is called before the first frame update
    void Start()
    {
       // _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.LeftArrow))
        //    _myHp--;
        //else if (Input.GetKey(KeyCode.RightArrow))
        //    _myHp++;

        //HPのcolorを緑から赤にする
        if (_image.fillAmount > 0.75f)
            _image.color = Color.green;
        else if (_image.fillAmount > 0.3f)
            _image.color = new Color(1f, 127f / 255f, 39f / 255f);
        else
            _image.color = Color.red;

        _image.fillAmount = _myHp / 200.0f;
    }
    
    //ぶつかったらfillamoutを減らせばいい
    //ぶつかった相手はタグで探す
    void OnCollisionEnter(Collision collision)
    {
        //Enemyのtagを探してhpを減らす
        if (collision.gameObject.tag == "Enemy")
        {
            //Debug.Log("if通ってるよ");
            _myHp -= 10f;
            //Debug.Log(_myHp);
        }
    }

}
