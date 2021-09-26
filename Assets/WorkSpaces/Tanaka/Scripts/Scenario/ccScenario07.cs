using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ccScenario07 : MonoBehaviour
{

    [SerializeField] GameObject scenarioBox;
    //[SerializeField] SpriteRenderer ere;
    [SerializeField] private GameObject Glass;

    [SerializeField] private GameObject Erego;
    [SerializeField] private GameObject Ere_nothinggo;
    [SerializeField] private GameObject Ere_anger01go;
    [SerializeField] private GameObject Ere_anger02go;
    [SerializeField] private GameObject Ere_sad01go;
    [SerializeField] private GameObject Ere_smile01go;

    [SerializeField] private GameObject Rosego;
    [SerializeField] private GameObject Rose_Laughgo;
    [SerializeField] private GameObject Rose_Mockerygo; // �}��

    [SerializeField] private Image Ere;
    [SerializeField] private Image Ere_nothing;
    [SerializeField] private Image Ere_anger01;
    [SerializeField] private Image Ere_anger02;
    [SerializeField] private Image Ere_sad01;
    [SerializeField] private Image Ere_smile01;

    [SerializeField] private Image Rose;
    [SerializeField] private Image Rose_Laugh;
    [SerializeField] private Image Rose_Mockery;

    //[SerializeField] private GameObject cvObj;
    [SerializeField] private CV cv;

    // Unity��œ��͂���string�̔z��
    public string[] texts;
    // �\��������string
    string displayText;

    // ���Ԗڂ�texts[]��\�������邩
    int textNumber;
    // �������ڂ�displayText�ɒǉ����邩
    int textCharNumber;
    // �S�̂̃t���[�����[�g�𗎂Ƃ�(�����̑���)
    int displayTextSpeed;

    bool click;

    bool skip;
    // �e�L�X�g�̕\�����n�߂邩
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

        Rosego.SetActive(false);
        Rose_Laughgo.SetActive(false);
        Rose_Mockerygo.SetActive(false);

        //for (int i = 0; i < sentence.Length; i++)
        //    messageList.Add(sentence[i]);
    }

    void Update()
    {
        var gamepad = Gamepad.current;

        // ��������
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            click = true;
            //Debug.Log("�}�E�X(��)");
        }
        else if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            click = true;
            Debug.Log("�G���^�[");
        }
        
        else if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            skip = true;
            //Debug.Log("�X�y�[�X");
        }
        else if (Keyboard.current[Key.Q].wasPressedThisFrame)
        {
            skip = true;
            Debug.Log("q");
        }

        else if (Gamepad.current != null && Gamepad.current.bButton.wasPressedThisFrame)
        {
            click = true;
            Debug.Log("PS4�Z");
        }

        //�X�L�b�v�@�\
        if (Gamepad.current != null && Gamepad.current.selectButton.wasPressedThisFrame)
        {
            skip = true;
            Debug.Log("PS4SHARE");
        }

        // �e�L�X�g��\��������if��
        if (textStop == false)
        {
            displayTextSpeed++;

            // 5��Ɉ��v���O���������s����if��
            if (displayTextSpeed % 5 == 0)
            {
                // ���� text[textNumber]�̕�����̍Ō�̕�������Ȃ����
                if (textCharNumber != texts[textNumber].Length)
                {
                    // displayText�ɕ�����ǉ����Ă���
                    displayText = displayText + texts[textNumber][textCharNumber];
                    // ���̕����ɂ���
                    textCharNumber = textCharNumber + 1;
                }
                // ���� text[textNumber]�̕�����̍Ō�̕�����������
                else
                {
                    // ����text[]���Ō�̃Z���t����Ȃ����
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
                GrayColor();
                Glass.SetActive(true);
                cv.VoicePlay(21);
            }
            // 2
            else if (textNumber == 1)
            {
                EreNomalColor();
                EreSmile01();
                cv.VoicePlay(0);
            }
            // 3
            else if (textNumber == 2)
            {
                GrayColor();
                Glass.SetActive(false);
                cv.VoicePlay(22);
            }
            // 4
            else if (textNumber == 3)
            {
                GrayColor();
                cv.VoicePlay(23);
            }
            // 5
            else if (textNumber == 4)
            {
                EreNomalColor();
                EreSad01();
                cv.VoicePlay(1);
            }
            // 6
            else if (textNumber == 5)
            {
                EreNomalColor();
                EreSad01();
                cv.VoicePlay(2);
            }
            // 7
            else if (textNumber == 6)
            {
                Rosego.SetActive(true);
                RoseNomalColor();
                RoseLaugh();
                cv.VoicePlay(8);
            }
            // 8
            else if (textNumber == 7)
            {
                RoseNomalColor();
                RoseNomal();
                cv.VoicePlay(9);
            }
            // 9
            else if (textNumber == 8)
            {
                RoseNomalColor();
                RoseMockery();
                cv.VoicePlay(10);
            }
            //10
            else if (textNumber == 9)
            {
                EreNomalColor();
                EreNothing();
                cv.VoicePlay(3);
            }
            //11
            else if (textNumber == 10)
            {
                RoseNomalColor();
                RoseNomal();
                cv.VoicePlay(11);
            }
            //12
            else if (textNumber == 11)
            {
                RoseNomalColor();
                RoseMockery();
                cv.VoicePlay(12);
            }
            //13
            else if (textNumber == 12)
            {
                EreNomalColor();
                EreAnger01();
                cv.VoicePlay(4);
            }
            //14
            else if (textNumber == 13)
            {
                EreNomalColor();
                EreAnger01();
                cv.VoicePlay(5);
            }
            //15
            else if (textNumber == 14)
            {
                EreNomalColor();
                EreAnger02();
                cv.VoicePlay(6);
            }
            //16
            else if (textNumber == 15)
            {
                RoseNomalColor();
                RoseLaugh();
                cv.VoicePlay(13);
            }
            //17
            else if (textNumber == 16)
            {
                RoseNomalColor();
                RoseNomal();
                cv.VoicePlay(14);
            }
            //18
            else if (textNumber == 17)
            {
                RoseNomalColor();
                RoseMockery();
                cv.VoicePlay(15);
            }
            //19
            else if (textNumber == 18)
            {
                GrayColor();
                cv.VoicePlay(24);
            }
            //20
            else if (textNumber == 19)
            {
                RoseNomalColor();
                RoseLaugh();
                cv.VoicePlay(16);
            }
            //21
            else if (textNumber == 20)
            {
                RoseNomalColor();
                RoseLaugh();
                cv.VoicePlay(17);
            }
            //22
            else if (textNumber == 21)
            {
                RoseNomalColor();
                RoseNomal();
                cv.VoicePlay(18);
            }
            //23
            else if (textNumber == 22)
            {
                RoseNomalColor();
                RoseMockery();
                cv.VoicePlay(19);
            }
            //24
            else if (textNumber == 23)
            {
                GrayColor();
                EreNothing();
                cv.VoicePlay(25);
            }
            //25
            else if (textNumber == 24)
            {
                GrayColor();
                cv.VoicePlay(26);
            }
            //26
            else if (textNumber == 25)
            {
                EreNomalColor();
                cv.VoicePlay(7);
            }
            //27
            else if (textNumber == 26)
            {
                RoseNomalColor();
                RoseLaugh();
                cv.VoicePlay(20);
            }
            //28
            else if (textNumber == 27)
            {
                GrayColor();
                cv.VoicePlay(27);
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
        Rose_Laugh.color = Color.gray;
        Rose_Mockery.color = Color.gray;
        Rose.color = Color.gray;
    }

    private void RoseNomalColor()
    {
        Ere_nothing.color = Color.gray;
        Ere_anger01.color = Color.gray;
        Ere_anger02.color = Color.gray;
        Ere_sad01.color = Color.gray;
        Ere_smile01.color = Color.gray;
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
        Ere.color = Color.gray;
        Rose_Laugh.color = Color.gray;
        Rose_Mockery.color = Color.gray;
        Rose.color = Color.gray;
    }

    // ���\����p
    private void EreNomal()
    {
        Erego.SetActive(true);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(false);
        Ere_sad01go.SetActive(false);
        Ere_nothinggo.SetActive(false);
        Ere_smile01go.SetActive(false);
    }

    private void EreSad01()
    {
        Erego.SetActive(false);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(false);
        Ere_sad01go.SetActive(true);
        Ere_nothinggo.SetActive(false);
        Ere_smile01go.SetActive(false);
    }

    private void EreAnger01()
    {
        Erego.SetActive(false);
        Ere_anger01go.SetActive(true);
        Ere_anger02go.SetActive(false);
        Ere_nothinggo.SetActive(false);
        Ere_smile01go.SetActive(false);
    }

    private void EreAnger02()
    {
        Erego.SetActive(false);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(true);
        Ere_nothinggo.SetActive(false);
        Ere_smile01go.SetActive(false);
    }

    private void EreNothing()
    {
        Erego.SetActive(false);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(false);
        Ere_nothinggo.SetActive(true);
        Ere_smile01go.SetActive(false);
    }

    private void EreSmile01()
    {
        Erego.SetActive(false);
        Ere_anger01go.SetActive(false);
        Ere_anger02go.SetActive(false);
        Ere_nothinggo.SetActive(false);
        Ere_smile01go.SetActive(true);
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


}
