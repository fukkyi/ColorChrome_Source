using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ScenarioScript : MonoBehaviour
{
    [SerializeField] private GameObject ScenarioPanel;
    [SerializeField] private Image Keo;
    [SerializeField] private Image Ere;

    [SerializeField] private string text0 = string.Empty;
    [SerializeField] private string text1 = string.Empty;
    [SerializeField] private string text2 = string.Empty;
    [SerializeField] private string text3 = string.Empty;
    [SerializeField] private string text4 = string.Empty;
    [SerializeField] private string text5 = string.Empty;
    [SerializeField] private string text6 = string.Empty;
    [SerializeField] private string text7 = string.Empty;
    [SerializeField] private string text8 = string.Empty;
    [SerializeField] private string text9 = string.Empty;
    [SerializeField] private string text10 = string.Empty;
    [SerializeField] private string text11 = string.Empty;
    [SerializeField] private string text12 = string.Empty;
    [SerializeField] private string text13 = string.Empty;
    [SerializeField] private string text14 = string.Empty;
    [SerializeField] private string text15 = string.Empty;
    [SerializeField] private string text16 = string.Empty;
    [SerializeField] private string text17 = string.Empty;
    [SerializeField] private string text18 = string.Empty;
    [SerializeField] private string text19 = string.Empty;

    string[] texts = new string[20];

    int nowNumber;

    // Start is called before the first frame update
    void Start()
    {
        Image Keo = GetComponent<Image>();
        Image Ere = GetComponent<Image>();

        ScenarioPanel.SetActive(true);

        nowNumber = 0;
        texts[0] = text0;
        texts[1] = text1;
        texts[2] = text2;
        texts[3] = text3;
        texts[4] = text4;
        texts[5] = text5;
        texts[6] = text6;
        texts[7] = text7;
        texts[8] = text8;
        texts[9] = text9;
        texts[10] = text10;
        texts[11] = text11;
        texts[12] = text12;
        texts[13] = text13;
        texts[14] = text14;
        texts[15] = text15;
        texts[16] = text16;
        texts[17] = text17;
        texts[18] = text18;
        texts[19] = text19;
    }

    // Update is called once per frame
    void Update()
    {
        Text nowText = GetComponentInChildren<Text>();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            nowNumber++;

            //if (nowText.text == text0)
            //{
            //    Keo.color = Color.gray;
            //    Ere.color = Color.gray;
            //}
            if (nowText.text == text0)
            {
                KeoNomalColor();
            }
            else if(nowText.text == text1)
            {
                EreNomalColor();
            }
            else if (nowText.text == text2)
            {
                KeoNomalColor();
            }
            else if (nowText.text == text3)
            {
                EreNomalColor();
            }
            else if (nowText.text == text4)
            {
                KeoNomalColor();
            }
            else if (nowText.text == text5)
            {
                EreNomalColor();
            }
            else if (nowText.text == text6)
            {
                KeoNomalColor();
            }
            else if (nowText.text == text7)
            {
                GrayColor();
            }
            else if (nowText.text == text8)
            {
                GrayColor();
            }
            else if (nowText.text == text9)
            {
                EreNomalColor();
            }
            else if (nowText.text == text10)
            {
                KeoNomalColor();
            }
            // ケオが連れ去られるポイント
            else if (nowText.text == text11)
            {
                GrayColor();
            }
            else if (nowText.text == text12)
            {
                GrayColor();
            }
            else if (nowText.text == text13)
            {
                EreNomalColor();
            }
            else if (nowText.text == text14)
            {
                GrayColor();
            }
            else if (nowText.text == text15)
            {
                GrayColor();
            }
            else if (nowText.text == text16)
            {
                EreNomalColor();
            }
            else if (nowText.text == text17)
            {
                GrayColor();
            }
            else if (nowText.text == text18)
            {
                GrayColor();
            }

            else if (nowText.text == text19)
            {
                ScenarioPanel.SetActive(false);
                SceneManager.LoadScene("TrialScene");
            }
        }
        //インデックスの範囲外
        nowText.text = texts[nowNumber];
    }

    private void KeoNomalColor()
    {
        Keo.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Ere.color = Color.gray;
    }
    private void EreNomalColor()
    {
        Keo.color = Color.gray;
        Ere.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
    private void GrayColor()
    {
        Keo.color = Color.gray;
        Ere.color = Color.gray;
    }
}
