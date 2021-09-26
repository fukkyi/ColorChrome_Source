using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ccScenario02 : MonoBehaviour
{
    [SerializeField] GameObject scenarioBox;
    //[SerializeField] SpriteRenderer ere;

    [SerializeField] private GameObject Glass;

    [SerializeField] private GameObject Erego;
    [SerializeField] private GameObject Ere_sad01go;

    [SerializeField] private Image Ere;
    [SerializeField] private Image Ere_sad01;

    //[SerializeField] private GameObject cvObj;
    [SerializeField] private CV cv;

    // Unity上で入力するstringの配列
    public string[] texts;
    // 表示させるstring
    string displayText;

    // 何番目のtexts[]を表示させるか
    int textNumber;
    // 何文字目をdisplayTextに追加するか
    int textCharNumber;
    // 全体のフレームレートを落とす(文字の速さ)
    int displayTextSpeed;

    bool click;

    bool skip;
    // テキストの表示を始めるか
    bool textStop;

    // Start is called before the first frame update
    void Start()
    {
        //cvObj = GameObject.Find("Voice");
        //cv = cvObj.GetComponent<CV>();

        var pInput = GetComponent<PlayerInput>();
        scenarioBox.SetActive(true);

        Glass.SetActive(false);

        Erego.SetActive(true);
        Ere_sad01go.SetActive(false);

        //for (int i = 0; i < sentence.Length; i++)
        //    messageList.Add(sentence[i]);
    }

    void Update()
    {
        var gamepad = Gamepad.current;

        // 文字送り
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            click = true;
            //Debug.Log("マウス(左)");
        }
        else if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            click = true;
            Debug.Log("エンター");
        }

        // スキップ
        else if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            skip = true;
            //Debug.Log("スペース");
        }
        else if (Keyboard.current[Key.Q].wasPressedThisFrame)
        {
            skip = true;
            Debug.Log("q");
        }

        else if (Gamepad.current != null && Gamepad.current.bButton.wasPressedThisFrame)
        {
            click = true;
            Debug.Log("PS4〇");
        }

        //スキップ機能
        if (Gamepad.current != null && Gamepad.current.selectButton.wasPressedThisFrame)
        {
            skip = true;
            Debug.Log("PS4SHARE");
        }

        // テキストを表示させるif文
        if (textStop == false)
        {
            displayTextSpeed++;

            // 5回に一回プログラムを実行するif文
            if (displayTextSpeed % 5 == 0)
            {
                // もし text[textNumber]の文字列の最後の文字じゃなければ
                if (textCharNumber != texts[textNumber].Length)
                {
                    // displayTextに文字を追加していく
                    displayText = displayText + texts[textNumber][textCharNumber];
                    // 次の文字にする
                    textCharNumber = textCharNumber + 1;

                    //if (Gamepad.current.bButton.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
                    //{

                    //}

                }
                // もし text[textNumber]の文字列の最後の文字だったら
                else
                {
                    // もしtext[]が最後のセリフじゃなければ
                    if (textNumber != texts.Length - 1)
                    {
                        if (click == true)
                        {
                            cv.VoiceReset();
                            displayText = "";
                            textCharNumber = 0;
                            textNumber = textNumber + 1;
                        }
                        else if (skip == true)
                        {
                            displayText = "";
                            textCharNumber = 0;
                            textStop = true;
                            scenarioBox.SetActive(false);
                        }
                    }
                    // 次のシーンに遷移、もしくは次に進むための場面に移行する
                    else
                    {
                        if (click == true)
                        {
                            cv.VoiceReset();
                            displayText = "";
                            textCharNumber = 0;
                            textStop = true;
                            scenarioBox.SetActive(false);
                        }
                    }
                }

                GetComponent<Text>().text = displayText;
                click = false;
            }

            // 1
            if (textNumber == 0)
            {
                GrayColor();
                cv.VoicePlay(10);
            }
            // 2
            else if (textNumber == 1)
            {
                GrayColor();
                cv.VoicePlay(11);
            }
            // 3
            else if (textNumber == 2)
            {
                GrayColor();
                cv.VoicePlay(12);
            }
            // 4
            else if (textNumber == 3)
            {
                EreNomalColor();
                EreSad01();
                cv.VoicePlay(0);
            }
            // 5
            else if (textNumber == 4)
            {
                GrayColor();
                Glass.SetActive(true);
                cv.VoicePlay(13);
            }
            // 6
            else if (textNumber == 5)
            {
                GrayColor();
                cv.VoicePlay(14);
            }
            // 7
            else if (textNumber == 6)
            {
                EreNomalColor();
                EreNomal();
                cv.VoicePlay(1);
            }
            // 8
            else if (textNumber == 7)
            {
                EreNomalColor();
                cv.VoicePlay(2);
            }
            // 9
            else if (textNumber == 8)
            {
                GrayColor();
                cv.VoicePlay(15);
            }
            // 10
            else if (textNumber == 9)
            {
                EreNomalColor();
                EreSad01();
                cv.VoicePlay(3);
            }
            // 11
            else if (textNumber == 10)
            {
                EreNomalColor();
                cv.VoicePlay(4);
            }
            // 12
            else if (textNumber == 11)
            {
                EreNomalColor();
                cv.VoicePlay(5);
            }
            // 13
            else if (textNumber == 12)
            {
                EreNomalColor();
                cv.VoicePlay(6);
            }
            // 14
            else if (textNumber == 13)
            {
                EreNomalColor();
                cv.VoicePlay(7);
            }
            // 15
            else if (textNumber == 14)
            {
                EreNomalColor();
                cv.VoicePlay(8);
            }
            // 16
            else if (textNumber == 15)
            {
                EreNomalColor();
                EreNomal();
                cv.VoicePlay(9);
            }

            //--------------------------------------------------------
            //if(keo1_1 == true)
            //{
            //    keo1_1 = false;
            //    cvScr.Keo1_1();
            //}
            //--------------------------------------------------------
        }
    }

    private void EreNomalColor()
    {
        Ere.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Ere_sad01.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    private void GrayColor()
    {
        Ere.color = Color.gray;
        Ere_sad01.color = Color.gray;
    }

    // ↓表情差分用
    private void EreNomal()
    {
        Erego.SetActive(true);
        Ere_sad01go.SetActive(false);
    }

    private void EreSad01()
    {
        Erego.SetActive(false);
        Ere_sad01go.SetActive(true);
    }

}
