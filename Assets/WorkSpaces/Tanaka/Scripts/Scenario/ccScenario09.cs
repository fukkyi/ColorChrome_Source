using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ccScenario09 : MonoBehaviour
{
    [SerializeField] GameObject scenarioBox;
    //[SerializeField] SpriteRenderer ere;

    [SerializeField] private GameObject Erego;
    [SerializeField] private GameObject Ere_nothinggo;
    [SerializeField] private GameObject Ere_anger01go;
    [SerializeField] private GameObject Ere_anger02go;
    [SerializeField] private GameObject Ere_sad01go;
    [SerializeField] private GameObject Ere_smile01go;
    [SerializeField] private GameObject Ere_smile02go;
    [SerializeField] private GameObject Ere_smile03go;

    [SerializeField] private GameObject Keogo;
    [SerializeField] private GameObject Keo_smilego;
    [SerializeField] private GameObject Keo_sadgo;

    [SerializeField] private GameObject Rosego;
    [SerializeField] private GameObject Rose_Laughgo;
    [SerializeField] private GameObject Rose_Mockerygo; // 嘲笑

    [SerializeField] private Image Ere;
    [SerializeField] private Image Ere_nothing;
    [SerializeField] private Image Ere_anger01;
    [SerializeField] private Image Ere_anger02;
    [SerializeField] private Image Ere_sad01;
    [SerializeField] private Image Ere_smile01;
    [SerializeField] private Image Ere_smile02;
    [SerializeField] private Image Ere_smile03;

    [SerializeField] private Image Keo;
    [SerializeField] private Image Keo_smile;
    [SerializeField] private Image Keo_sad;

    [SerializeField] private Image Rose;
    [SerializeField] private Image Rose_Laugh;
    [SerializeField] private Image Rose_Mockery;

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
        Image Ere = GetComponent<Image>();
        Image Ere_anger01 = GetComponent<Image>();
        Image Ere_anger02 = GetComponent<Image>();
        Image Ere_smile = GetComponent<Image>();
        Image Ere_sad = GetComponent<Image>();

        //cvObj = GameObject.Find("Voice");
        //cvScr = cvObj.GetComponent<CV>();

        //audioSource = GetComponent<AudioSource>();

        var pInput = GetComponent<PlayerInput>();
        scenarioBox.SetActive(true);

        Erego.SetActive(true);
        Ere_nothinggo.SetActive(false);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(false);
        Ere_sad01go.SetActive(false);
        Ere_smile01go.SetActive(false);
        Ere_smile02go.SetActive(false);
        Ere_smile03go.SetActive(false);

        Keogo.SetActive(false);
        Keo_sadgo.SetActive(false);
        Keo_smilego.SetActive(false);

        Rosego.SetActive(true);
        Rose_Laughgo.SetActive(false);
        Rose_Mockerygo.SetActive(false);

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

        //else if (Gamepad.current.bButton.wasPressedThisFrame)
        //{
        //    click = true;
        //    Debug.Log("PS4〇");
        //}

        ////スキップ機能
        //if (Gamepad.current.selectButton.wasPressedThisFrame)
        //{
        //    skip = true;
        //    Debug.Log("PS4SHARE");
        //}

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
                RoseNomalColor();
                RoseNomal();
                cv.VoicePlay(11);
            }
            // 2
            else if (textNumber == 1)
            {
                EreNomalColor();
                EreAnger02();
                cv.VoicePlay(0);
            }
            // 3
            else if (textNumber == 2)
            {
                RoseNomalColor();
                RoseMockery();
                cv.VoicePlay(12);
            }
            // 4
            else if (textNumber == 3)
            {
                RoseNomalColor();
                RoseNomal();
                cv.VoicePlay(13);
            }
            // 5
            else if (textNumber == 4)
            {
                EreNomalColor();
                EreNothing();
                cv.VoicePlay(1);
            }
            // 6
            else if (textNumber == 5)
            {
                RoseNomalColor();
                RoseMockery();
                cv.VoicePlay(14);
            }
            // 7
            else if (textNumber == 6)
            {
                Rose_Mockerygo.SetActive(false);
                GrayColor();
                cv.VoicePlay(15);
            }
            // 8
            else if (textNumber == 7)
            {
                GrayColor();
                cv.VoicePlay(16);
            }
            // 9
            else if (textNumber == 8)
            {
                Keogo.SetActive(true);
                EreNomalColor();
                EreSad01();
                cv.VoicePlay(3);
                //cv.VoicePlay(2);
            }
            //10
            else if (textNumber == 9)
            {
                GrayColor();
                cv.VoicePlay(17);
            }
            //11
            else if (textNumber == 10)
            {
                GrayColor();
                cv.VoicePlay(18);
            }
            //12
            else if (textNumber == 11)
            {
                GrayColor();
                cv.VoicePlay(19);
            }
            //13
            else if (textNumber == 12)
            {
                GrayColor();
                cv.VoicePlay(20);
            }
            //14
            else if (textNumber == 13)
            {
                KeoNomalColor();
                KeoSad();
                cv.VoicePlay(23);
            }
            //15
            else if (textNumber == 14)
            {
                EreNomalColor();
                EreSmile03();
                cv.VoicePlay(4);
                //cv.VoicePlay(3);
            }
            //16
            else if (textNumber == 15)
            {
                KeoNomalColor();
                KeoNomal();
                cv.VoicePlay(24);
            }
            //17
            else if (textNumber == 16)
            {
                EreNomalColor();
                EreSmile03();
                cv.VoicePlay(5);
            }
            //18
            else if (textNumber == 17)
            {
                KeoNomalColor();
                KeoSad();
                cv.VoicePlay(25);
            }
            //19
            else if (textNumber == 18)
            {
                EreNomalColor();
                EreSad01();
                cv.VoicePlay(6);
            }
            //20
            else if (textNumber == 19)
            {
                KeoNomalColor();
                KeoSad();
                cv.VoicePlay(26);
            }
            //21
            else if (textNumber == 20)
            {
                EreNomalColor();
                EreSmile02();
                cv.VoicePlay(7);
            }
            //22
            else if (textNumber == 21)
            {
                KeoNomalColor();
                KeoSmile();
                cv.VoicePlay(27);
            }
            //23
            else if (textNumber == 22)
            {
                EreNomalColor();
                cv.VoicePlay(8);
            }
            //24
            else if (textNumber == 23)
            {
                KeoNomalColor();
                KeoSmile();
                cv.VoicePlay(28);
            }
            //25
            else if (textNumber == 24)
            {
                GrayColor();
                cv.VoicePlay(21);
            }
            //26
            else if (textNumber == 25)
            {
                GrayColor();
                cv.VoicePlay(22);
            }
            //27
            else if (textNumber == 26)
            {
                EreNomalColor();
                EreSad01();
                cv.VoicePlay(9);
            }
            //28
            else if (textNumber == 27)
            {
                KeoNomalColor();
                KeoSmile();
                cv.VoicePlay(29);
            }
            //29
            else if (textNumber == 28)
            {
                EreNomalColor();
                EreSad01();
                cv.VoicePlay(10);
            }
            //30
            else if (textNumber == 29)
            {
                EreNomalColor();
                cv.VoicePlay(30);
            }
            //31
            else if (textNumber == 30)
            {
                GrayColor();
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
        Ere_nothing.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Ere_anger01.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Ere_anger02.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Ere_sad01.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Ere_smile01.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Ere_smile02.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Ere_smile03.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        Keo.color = Color.gray;
        Keo_sad.color = Color.gray;
        Keo_smile.color = Color.gray;

        Rose_Laugh.color = Color.gray;
        Rose_Mockery.color = Color.gray;
        Rose.color = Color.gray;
    }

    private void KeoNomalColor()
    {
        Keo.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Keo_sad.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Keo_smile.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
       
        Ere.color = Color.gray;
        Ere_anger01.color = Color.gray;
        Ere_anger02.color = Color.gray;
        Ere_smile01.color = Color.gray;
        Ere_smile02.color = Color.gray;
        Ere_smile03.color = Color.gray;
        Ere_sad01.color = Color.gray;
    }

    private void RoseNomalColor()
    {
        Ere_nothing.color = Color.gray;
        Ere_anger01.color = Color.gray;
        Ere_anger02.color = Color.gray;
        Ere_sad01.color = Color.gray;
        Ere_smile01.color = Color.gray;
        Ere_smile02.color = Color.gray;
        Ere_smile03.color = Color.gray;
        Ere.color = Color.gray;

        Rose_Laugh.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Rose_Mockery.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Rose.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    private void GrayColor()
    {
        Ere_nothing.color = Color.gray;
        Ere_anger01.color = Color.gray;
        Ere_anger02.color = Color.gray;
        Ere_sad01.color = Color.gray;
        Ere_smile01.color = Color.gray;
        Ere_smile02.color = Color.gray;
        Ere_smile03.color = Color.gray;
        Ere.color = Color.gray;

        Keo.color = Color.gray;
        Keo_sad.color = Color.gray;
        Keo_smile.color = Color.gray;

        Rose_Laugh.color = Color.gray;
        Rose_Mockery.color = Color.gray;
        Rose.color = Color.gray;
    }

    // ↓表情差分用
    private void EreNomal()
    {
        Erego.SetActive(true);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(false);
        Ere_sad01go.SetActive(false);
        Ere_nothinggo.SetActive(false);
        Ere_smile01go.SetActive(false);
        Ere_smile02go.SetActive(false);
        Ere_smile03go.SetActive(false);
    }

    private void EreSad01()
    {
        Erego.SetActive(false);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(false);
        Ere_sad01go.SetActive(true);
        Ere_nothinggo.SetActive(false);
        Ere_smile01go.SetActive(false);
        Ere_smile02go.SetActive(false);
        Ere_smile03go.SetActive(false);
    }

    private void EreAnger02()
    {
        Erego.SetActive(false);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(true);
        Ere_sad01go.SetActive(false);
        Ere_nothinggo.SetActive(false);
        Ere_smile01go.SetActive(false);
        Ere_smile02go.SetActive(false);
        Ere_smile03go.SetActive(false);
    }

    private void EreNothing()
    {
        Erego.SetActive(false);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(false);
        Ere_sad01go.SetActive(false);
        Ere_nothinggo.SetActive(true);
        Ere_smile01go.SetActive(false);
        Ere_smile02go.SetActive(false);
        Ere_smile03go.SetActive(false);
    }

    private void EreSmile01()
    {
        Erego.SetActive(false);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(false);
        Ere_sad01go.SetActive(false);
        Ere_nothinggo.SetActive(false);
        Ere_smile01go.SetActive(true);
        Ere_smile02go.SetActive(false);
        Ere_smile03go.SetActive(false);
    }

    private void EreSmile02()
    {
        Erego.SetActive(false);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(false);
        Ere_sad01go.SetActive(false);
        Ere_nothinggo.SetActive(false);
        Ere_smile01go.SetActive(false);
        Ere_smile02go.SetActive(true);
        Ere_smile03go.SetActive(false);
    }

    private void EreSmile03()
    {
        Erego.SetActive(false);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(false);
        Ere_sad01go.SetActive(false);
        Ere_nothinggo.SetActive(false);
        Ere_smile01go.SetActive(false);
        Ere_smile02go.SetActive(false);
        Ere_smile03go.SetActive(true);
    }

    private void RoseNomal()
    {
        Rosego.SetActive(true);
        Rose_Mockerygo.SetActive(false);
        Rose_Laughgo.SetActive(false);
    }

    private void RoseLaugh()
    {
        Rosego.SetActive(false);
        Rose_Mockerygo.SetActive(false);
        Rose_Laughgo.SetActive(true);
    }

    private void RoseMockery()
    {
        Rosego.SetActive(false);
        Rose_Mockerygo.SetActive(true);
        Rose_Laughgo.SetActive(false);
    }

    private void KeoNomal()
    {
        Keogo.SetActive(true);
        Keo_sadgo.SetActive(false);
        Keo_smilego.SetActive(false);
    }

    private void KeoSmile()
    {
        Keogo.SetActive(false);
        Keo_sadgo.SetActive(false);
        Keo_smilego.SetActive(true);
    }

    private void KeoSad()
    {
        Keogo.SetActive(false);
        Keo_sadgo.SetActive(false);
        Keo_smilego.SetActive(false);
        Keo_sadgo.SetActive(true);
    }
}


