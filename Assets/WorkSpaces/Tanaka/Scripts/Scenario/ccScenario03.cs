using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ccScenario03 : MonoBehaviour
{
    [SerializeField] GameObject scenarioBox;
    //[SerializeField] SpriteRenderer ere;

    [SerializeField] private GameObject Erego;
    [SerializeField] private GameObject Ere_anger01go;
    [SerializeField] private GameObject Ere_anger02go;
    [SerializeField] private GameObject Ere_nothinggo;

    [SerializeField] private GameObject Hyoukago;
    [SerializeField] private GameObject Hyouka_sad01go;
    [SerializeField] private GameObject Hyouka_sad02go;
    [SerializeField] private GameObject Hyouka_angergo;

    [SerializeField] private Image Ere;
    [SerializeField] private Image Ere_anger01;
    [SerializeField] private Image Ere_anger02;
    [SerializeField] private Image Ere_nothing;

    [SerializeField] private Image Hyouka;
    [SerializeField] private Image Hyouka_sad01;
    [SerializeField] private Image Hyouka_sad02;
    [SerializeField] private Image Hyouka_anger;

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
        //cv = cvObj.GetComponent<CV>();

        //audioSource = GetComponent<AudioSource>();

        var pInput = GetComponent<PlayerInput>();
        scenarioBox.SetActive(true);

        Erego.SetActive(true);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(false);

        Hyoukago.SetActive(true);
        Hyouka_sad01go.SetActive(false);
        Hyouka_sad02go.SetActive(false);
        Hyouka_angergo.SetActive(false);

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

        else if (Gamepad.current != null && Gamepad.current.bButton.wasPressedThisFrame)
        {
            click = true;
            Debug.Log("PS4〇");
        }

        //スキップ機能
        else if (Gamepad.current != null && Gamepad.current.selectButton.wasPressedThisFrame)
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
                EreNomalColor();
                cv.VoicePlay(1);
            }
            // 2
            else if (textNumber == 1)
            {
                HyoukaNomalColor();
                cv.VoicePlay(8);
            }
            // 3
            else if (textNumber == 2)
            {
                EreNomalColor();
                cv.VoicePlay(2);
            }
            // 4
            else if (textNumber == 3)
            {
                GrayColor();
                cv.VoicePlay(0);
            }
            // 5
            else if (textNumber == 4)
            {
                HyoukaNomalColor();
                HyoukaNomal();
                cv.VoicePlay(9);
            }
            // 6
            else if (textNumber == 5)
            {
                EreNomalColor();
                EreAnger01();
                cv.VoicePlay(3);
            }
            // 7
            else if (textNumber == 6)
            {
                HyoukaNomalColor();
                cv.VoicePlay(10);
            }
            // 8
            else if (textNumber == 7)
            {
                HyoukaNomalColor();
                cv.VoicePlay(11);
            }
            // 9
            else if (textNumber == 8)
            {
                EreNomalColor();
                EreAnger02();
                cv.VoicePlay(4);
            }
            // 10
            else if (textNumber == 9)
            {
                HyoukaNomalColor();
                cv.VoicePlay(12);
            }
            // 11
            else if (textNumber == 10)
            {
                HyoukaNomalColor();
                HyoukaAnger();
                cv.VoicePlay(13);
            }
            // 12
            else if (textNumber == 11)
            {
                EreNomalColor();
                EreNothing();
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
                HyoukaNomalColor();
                HyoukaAnger();
                cv.VoicePlay(14);
            }
            // 15
            else if (textNumber == 14)
            {
                EreNomalColor();
                cv.VoicePlay(7);
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
        Ere_anger01.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Ere_anger02.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Hyouka_sad01.color = Color.gray;
        Hyouka_sad02.color = Color.gray;
        Hyouka_anger.color = Color.gray;
        Hyouka.color = Color.gray;


    }

    private void HyoukaNomalColor()
    {
        Ere_anger01.color = Color.gray;
        Ere_anger02.color = Color.gray;
        Ere.color = Color.gray;
        Hyouka_sad01.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Hyouka_sad02.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Hyouka_anger.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Hyouka.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    private void GrayColor()
    {
        Ere_anger01.color = Color.gray;
        Ere_anger02.color = Color.gray;
        Ere.color = Color.gray;
        Hyouka_sad01.color = Color.gray;
        Hyouka_sad02.color = Color.gray;
        Hyouka_anger.color = Color.gray;
        Hyouka.color = Color.gray;
    }

    // ↓表情差分用
    private void EreNomal()
    {
        Erego.SetActive(true);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(false);
        Ere_nothinggo.SetActive(false);
    }

    private void EreNothing()
    {
        Erego.SetActive(false);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(false);
        Ere_nothinggo.SetActive(true);
    }

    private void EreAnger01()
    {
        Erego.SetActive(false);
        Ere_anger01go.SetActive(true);
        Ere_anger02go.SetActive(false);
        Ere_nothinggo.SetActive(false);
    }

    private void EreAnger02()
    {
        Erego.SetActive(false);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(true);
        Ere_nothinggo.SetActive(false);
    }

    private void HyoukaNomal()
    {
        Hyoukago.SetActive(true);
        Hyouka_angergo.SetActive(false);
    }

    private void HyoukaAnger()
    {
        Hyoukago.SetActive(false);
        Hyouka_angergo.SetActive(true);
    }

}
