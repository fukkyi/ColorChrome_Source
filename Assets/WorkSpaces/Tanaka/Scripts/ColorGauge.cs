using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGauge : MonoBehaviour
{
    [SerializeField] Image redGauge;
    [SerializeField] Image greenGauge;
    [SerializeField] Image blueGauge;

    [SerializeField] Transform target;
    public Vector3 offset;

    [SerializeField] private GameObject redFragment;
    [SerializeField] private GameObject greenFragment;
    [SerializeField] private GameObject blueFragment;

    //private PlayerScript playerScript;

    public bool redFrag = false;
    public bool greenFrag = false;
    public bool blueFrag = false;

    //private Enemy enemy;

    public float pos_y;

    // Start is called before the first frame update
    void Start()
    {
        redGauge.fillAmount = 0f;
        greenGauge.fillAmount = 0f;
        blueGauge.fillAmount = 0f;

        redFragment.SetActive(false);
        greenFragment.SetActive(false);
        blueFragment.SetActive(false);

        //pos_y = transform.position.y;
    }

    void Update()
    {
        //dOnActive();
    }

    // 色に伴ったキャラが倒されたとき出現するように任意のタイミングで呼ぶ↓-----------------------------------------
    public void RedOnActive()
    {
        //Transform myTransform = redFragment.gameObject.GetComponent<Transform>();
        redFragment.SetActive(true);

        offset = new Vector3(0, 0, 0);
        redFragment.transform.position = target.position + offset;

        // アイテムっぽくふわふわさせる
        //redFragment.transform.position = new Vector3(myTransform.position.x, myTransform.position.y + Mathf.PingPong(Time.time / 3, 0.3f), transform.position.z);
    }

    public void GreenOnActive()
    {
        Transform myTransform = greenFragment.gameObject.GetComponent<Transform>();
        //pos_y = transform.position.y;
        greenFragment.SetActive(true);
        greenFragment.transform.position = new Vector3(myTransform.position.x, myTransform.position.y + Mathf.PingPong(Time.time / 3, 0.3f), transform.position.z);
    }

    public void BlueOnActive()
    {
        Transform myTransform = blueFragment.gameObject.GetComponent<Transform>();
        //pos_y = transform.position.y;
        blueFragment.SetActive(true);
        blueFragment.transform.position = new Vector3(myTransform.position.x, myTransform.position.y + Mathf.PingPong(Time.time / 3, 0.3f), transform.position.z);
    }
    // -------------------------------------------------------------------------------------------------------------

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "RedFrag")
        {
            redFrag = true;
            ColorRedFrag();
            //
            redFragment.SetActive(false);
        }
        else if (other.gameObject.tag == "GreenFrag")
        {
            greenFrag = true;
            ColorGreenFrag();
            //
            greenFragment.SetActive(false);
        }
        else if (other.gameObject.tag == "BlueFrag")
        {
            blueFrag = true;
            ColorBlueFrag();
            //
            blueFragment.SetActive(false);
        }

    }

    public void ColorRedFrag()
    {
        if (redFrag == true)
        {
            redGauge.fillAmount += 0.1f;
            redFrag = false;
        }
    }

    public void ColorGreenFrag()
    {
        if (greenFrag == true)
        {
            greenGauge.fillAmount += 0.1f;
            greenFrag = false;
        }
    }

    public void ColorBlueFrag()
    {
        if (blueFrag == true)
        {
            blueGauge.fillAmount += 0.1f;
            blueFrag = false;
        }
    }
}
