using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(AudioSource))]

public class ScenarioText : MonoBehaviour
{
    [SerializeField] GameObject scenarioBox;
    //[SerializeField] SpriteRenderer ere;

    [SerializeField] private Image Keo;
    [SerializeField] private Image Ere;


    [SerializeField] private GameObject cvObj;
    [SerializeField] private CV cvScr;

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
        Image Keo = GetComponent<Image>();
        Image Ere = GetComponent<Image>();

        cvObj = GameObject.Find("Voice");
        cvScr = cvObj.GetComponent<CV>();

        //audioSource = GetComponent<AudioSource>();

        var pInput = GetComponent<PlayerInput>();
        scenarioBox.SetActive(true);

        //for (int i = 0; i < sentence.Length; i++)
        //    messageList.Add(sentence[i]);
    }

    void Update()
    {

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
                            displayText = "";
                            textCharNumber = 0;
                            textNumber = textNumber + 1;
                        }
                        else if(skip == true)
                        {
                            displayText = "";
                            textCharNumber = 0;
                            textStop = true;
                            scenarioBox.SetActive(false);
                            SceneTransitionManager.Instance.StartTransitionByName(SceneTransitionManager.GameSceneName);
                        }

                    }
                    else
                    {
                        if (click == true)
                        {
                            displayText = "";
                            textCharNumber = 0;
                            textStop = true;
                            scenarioBox.SetActive(false);
                            SceneTransitionManager.Instance.StartTransitionByName(SceneTransitionManager.GameSceneName);
                        }
                    }
                }

                GetComponent<Text>().text = displayText;
                click = false;
            }

            // キャラの立ち絵
            if (textNumber == 0)
            {
                GrayColor();
            }
            else if(textNumber == 1)
            {
                KeoNomalColor();
                //keo1_1 = true;
                cvScr.Keo1_1();
                //audioSource.PlayOneShot(sound[6]);
            }
            else if(textNumber == 2)
            {
                EreNomalColor();
                cvScr.Ere1_1();
                //audioSource.PlayOneShot(sound[0]);
            }
            else if(textNumber == 3)
            {
                KeoNomalColor();
                cvScr.Keo1_2();
                //audioSource.PlayOneShot(sound[7]);
            }
            else if(textNumber == 4)
            {
                EreNomalColor();
                cvScr.Ere1_2();
                //audioSource.PlayOneShot(sound[1]);
            }
            else if(textNumber == 5)
            {
                KeoNomalColor();
                cvScr.Keo1_3();
                //audioSource.PlayOneShot(sound[8]);
            }
            else if(textNumber == 6)
            {
                // 「わかったー」↓一つずつ流れる音声がずれているため、一個上に入ってる
                EreNomalColor();
                cvScr.Ere1_3();
                //audioSource.PlayOneShot(sound[2]);
                
                cvScr.Keo1_4();
            }
            else if(textNumber == 7)
            {
                KeoNomalColor();
                //audioSource.PlayOneShot(sound[9]);
            }
            else if(textNumber == 8)
            {
                GrayColor();
            }
            else if(textNumber == 9)
            {
                GrayColor();
            }
            else if(textNumber == 10)
            {
                EreNomalColor();
                cvScr.Ere1_4();
                //audioSource.PlayOneShot(sound[3]);
                
                cvScr.Keo1_5();
            }
            else if(textNumber == 11)
            {
                KeoNomalColor();
                //audioSource.PlayOneShot(sound[10]);
            }
            else if(textNumber == 12)
            {
                GrayColor();
            }
            else if(textNumber == 13)
            {
                GrayColor();
                
                cvScr.Ere1_5();
            }
            else if(textNumber == 14)
            {
                EreNomalColor();
                //audioSource.PlayOneShot(sound[4]);
            }
            else if(textNumber == 15)
            {
                GrayColor();
            }
            else if(textNumber == 16)
            {
                GrayColor();
                
                cvScr.Ere1_6();
            }
            else if(textNumber == 17)
            {
                EreNomalColor();
                //audioSource.PlayOneShot(sound[5]);
            }
            else if(textNumber == 18)
            {
                GrayColor();
            }
            else if(textNumber == 19)
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
            else if (Gamepad.current.bButton.wasPressedThisFrame)
            {
                click = true;
                Debug.Log("PS4〇");
            }

            // スキップ機能
            else if (Gamepad.current.selectButton.wasPressedThisFrame)
            {
                skip = true;
                Debug.Log("PS4SHARE");
            }
            else if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                skip = true;
                //Debug.Log("スペース");
            }

        }
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
