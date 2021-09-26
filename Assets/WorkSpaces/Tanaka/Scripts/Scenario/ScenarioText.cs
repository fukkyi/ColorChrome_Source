using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

//[RequireComponent(typeof(AudioSource))]

public class ScenarioText : MonoBehaviour
{
    [SerializeField] GameObject scenarioBox;
    //[SerializeField] SpriteRenderer ere;

    [SerializeField] private GameObject Erego;
    [SerializeField] private GameObject Ere_smilego;
    [SerializeField] private GameObject Ere_sadgo;
    [SerializeField] private GameObject Ere_anger01go;
    [SerializeField] private GameObject Ere_anger02go;
    [SerializeField] private GameObject Keogo;
    [SerializeField] private GameObject Keo_smilego;
    [SerializeField] private GameObject Keo_sadgo;

    [SerializeField] private Image Ere;
    [SerializeField] private Image Ere_smile;
    [SerializeField] private Image Ere_sad;
    [SerializeField] private Image Ere_anger01;
    [SerializeField] private Image Ere_anger02;
    [SerializeField] private Image Keo;
    [SerializeField] private Image Keo_smile;
    [SerializeField] private Image Keo_sad;


    [SerializeField] private GameObject cvObj;
    [SerializeField] private CV cvScr;

    private bool audioStart = false; // TODO: 簡易的な修正

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
        Image Keo = GetComponent<Image>();
        Image Keo_smile = GetComponent<Image>();
        Image Keo_sad = GetComponent<Image>();

        //GetComponent<TextMesh>().alignment = TextAlignment.Left;

        cvObj = GameObject.Find("Voice");
        cvScr = cvObj.GetComponent<CV>();

        var pInput = GetComponent<PlayerInput>();
        scenarioBox.SetActive(true);

        //Ere.enabled = true;
        Erego.SetActive(true);
        //Ere_anger01.enabled = false;
        Ere_anger01go.SetActive(false);
        //Ere_anger02.enabled = false;
        Ere_anger02go.SetActive(false);
        //Ere_smile.enabled = false;
        Ere_smilego.SetActive(false);
        //Ere_sad.enabled = false;
        Ere_sadgo.SetActive(false);
        //Keo.enabled = true;
        Keogo.SetActive(true);
        //Keo_sad.enabled = false;
        Keo_sadgo.SetActive(false);
        //Keo_smile.enabled = false;
        Keo_smilego.SetActive(false);

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
            // Debug.Log("エンター");
        }

        //
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
            // Debug.Log("PS4〇");
        }

        //スキップ機能
        if (Gamepad.current != null && Gamepad.current.selectButton.wasPressedThisFrame)
        {
            skip = true;
            // Debug.Log("PS4SHARE");
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
                            audioStart = false; // クリックされたらオーディオ再生可能にする
                            cvScr.VoiceReset();
                            displayText = "";
                            textCharNumber = 0;
                            textNumber = textNumber + 1;
                            //ebug.Log(texts[textNumber].Length);

                        }
                        else if (skip == true)
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
                            cvScr.VoiceReset();
                            audioStart = false;　//
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
                cvScr.VoicePlay(11);
            }
            else if (textNumber == 1)
            {
                KeoNomalColor();
                KeoNomal();
                //keo1_1 = true;
                // ボイス再生
                if (!audioStart) cvScr.Keo1_1();
                audioStart = true;
                //audioSource.PlayOneShot(sound[6]);
            }
            else if (textNumber == 2)
            {
                EreNomalColor();
                EreNomal();
                if (!audioStart) cvScr.Ere1_1();
                audioStart = true;
                //audioSource.PlayOneShot(sound[0]);
            }
            else if (textNumber == 3)
            {
                KeoNomalColor();
                KeoNomal();
                if (!audioStart) cvScr.Keo1_2();
                audioStart = true;
                //audioSource.PlayOneShot(sound[7]);
            }
            else if (textNumber == 4)
            {
                EreNomalColor();
                EreSad();
                if (!audioStart) cvScr.Ere1_2();
                audioStart = true;
                //audioSource.PlayOneShot(sound[1]);
            }
            else if (textNumber == 5)
            {
                KeoNomalColor();
                KeoSmile();
                if (!audioStart) cvScr.Keo1_3();
                audioStart = true;
                //audioSource.PlayOneShot(sound[8]);
            }
            else if (textNumber == 6)
            {
                // 「わかったー」↓一つずつ流れる音声がずれているため、一個上に入ってる
                EreNomalColor();
                EreSmile();
                if (!audioStart) cvScr.Ere1_3();
                audioStart = true;
                //audioSource.PlayOneShot(sound[2]);

                //cvScr.Keo1_4();
            }
            else if (textNumber == 7)
            {
                KeoNomalColor();
                KeoSmile();
                if (!audioStart) cvScr.Keo1_4();
                audioStart = true;
                //audioSource.PlayOneShot(sound[9]);
            }
            else if (textNumber == 8)
            {
                GrayColor();
                cvScr.VoicePlay(12);
            }
            else if (textNumber == 9)
            {
                GrayColor();
                cvScr.VoicePlay(13);
            }
            else if (textNumber == 10)
            {
                EreNomalColor();
                EreAnger02();
                if (!audioStart) cvScr.Ere1_4();
                audioStart = true;
                //audioSource.PlayOneShot(sound[3]);
            }
            else if (textNumber == 11)
            {
                KeoNomalColor();
                KeoSad();
                if (!audioStart) cvScr.Keo1_5();
                audioStart = true;
                //audioSource.PlayOneShot(sound[10]);
            }
            else if (textNumber == 12)
            {
                GrayColor();
                cvScr.VoicePlay(14);
            }
            else if (textNumber == 13)
            {
                GrayColor();
                cvScr.VoicePlay(15);
            }
            else if (textNumber == 14)
            {
                EreNomalColor();
                EreAnger01();
                if (!audioStart) cvScr.Ere1_5();
                audioStart = true;
                //audioSource.PlayOneShot(sound[4]);
            }
            else if (textNumber == 15)
            {
                GrayColor();
                cvScr.VoicePlay(16);
            }
            else if (textNumber == 16)
            {
                GrayColor();
                cvScr.VoicePlay(17);

            }
            else if (textNumber == 17)
            {
                EreNomalColor();
                EreAnger01();
                if (!audioStart) cvScr.Ere1_6();
                audioStart = true;
                //audioSource.PlayOneShot(sound[5]);
            }
            else if (textNumber == 18)
            {
                GrayColor();
                cvScr.VoicePlay(18);
            }
            else if (textNumber == 19)
            {
                GrayColor();
                cvScr.VoicePlay(19);
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

    private void KeoNomalColor()
    {
        Keo.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Keo_sad.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Keo_smile.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Ere.color = Color.gray;
        Ere_anger01.color = Color.gray;
        Ere_anger02.color = Color.gray;
        Ere_smile.color = Color.gray;
        Ere_sad.color = Color.gray;
    }

    private void EreNomalColor()
    {
        Keo.color = Color.gray;
        Keo_sad.color = Color.gray;
        Keo_smile.color = Color.gray;
        Ere.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Ere_anger01.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Ere_anger02.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Ere_smile.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Ere_sad.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);


    }

    private void GrayColor()
    {
        Keo.color = Color.gray;
        Keo_sad.color = Color.gray;
        Keo_smile.color = Color.gray;
        Ere_anger01.color = Color.gray;
        Ere_anger02.color = Color.gray;
        Ere_smile.color = Color.gray;
        Ere_sad.color = Color.gray;
        Ere.color = Color.gray;
    }

    // ↓表情差分用
    private void EreNomal()
    {
        Erego.SetActive(true);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(false);
        Ere_smilego.SetActive(false);
        Ere_sadgo.SetActive(false);
    }

    private void EreSmile()
    {
        Erego.SetActive(false);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(false);
        Ere_smilego.SetActive(true);
        Ere_sadgo.SetActive(false);
    }

    private void EreSad()
    {
        Erego.SetActive(false);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(false);
        Ere_smilego.SetActive(false);
        Ere_sadgo.SetActive(true);
    }

    private void EreAnger01()
    {
        Erego.SetActive(false);
        Ere_anger01go.SetActive(true);
        Ere_anger02go.SetActive(false);
        Ere_smilego.SetActive(false);
        Ere_sadgo.SetActive(false);
    }

    private void EreAnger02()
    {
        Erego.SetActive(false);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(true);
        Ere_smilego.SetActive(false);
        Ere_sadgo.SetActive(false);
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
