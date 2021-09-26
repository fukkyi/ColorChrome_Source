using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ccScenario05 : MonoBehaviour
{
    [SerializeField] GameObject scenarioBox;
    //[SerializeField] SpriteRenderer ere;
    [SerializeField] private GameObject Glass;

    [SerializeField] private GameObject Erego;
    [SerializeField] private GameObject Ere_sad01go;
    [SerializeField] private GameObject Ere_anger01go;

    [SerializeField] private Image Ere;
    [SerializeField] private Image Ere_sad01;
    [SerializeField] private Image Ere_anger01;

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
        //cv = cvObj.GetComponent<CV>();

        //audioSource = GetComponent<AudioSource>();

        var pInput = GetComponent<PlayerInput>();
        scenarioBox.SetActive(true);

        Erego.SetActive(true);
        Ere_sad01go.SetActive(false);
        Ere_anger01go.SetActive(false);
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
                EreNomalColor();
                Glass.SetActive(true);
                cv.VoicePlay(0);
            }
            // 2
            else if (textNumber == 1)
            {
                GrayColor();
                cv.VoicePlay(6);
            }
            // 3
            else if (textNumber == 2)
            {
                EreNomalColor();
                cv.VoicePlay(1);
            }
            // 4
            else if (textNumber == 3)
            {
                EreNomalColor();
                EreSad01();
                cv.VoicePlay(2);
            }
            // 5
            else if (textNumber == 4)
            {
                EreNomalColor();
                EreSad01();
                Glass.SetActive(false);
                cv.VoicePlay(3);
            }
            // 6
            else if (textNumber == 5)
            {
                GrayColor();
                cv.VoicePlay(7);
            }
            // 7
            else if (textNumber == 6)
            {
                GrayColor();
                cv.VoicePlay(8);
            }
            // 8
            else if (textNumber == 7)
            {
                GrayColor();
                cv.VoicePlay(9);
            }
            // 9
            else if (textNumber == 8)
            {
                EreNomalColor();
                EreAnger01();
                cv.VoicePlay(4);
            }
            // 10
            else if (textNumber == 9)
            {
                EreNomalColor();
                cv.VoicePlay(5);
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
        Ere_anger01.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    private void GrayColor()
    {
        Ere_sad01.color = Color.gray;
        Ere_anger01.color = Color.gray;
        Ere.color = Color.gray;
    }

    // ���\����p
    private void EreNomal()
    {
        Erego.SetActive(true);
        Ere_sad01go.SetActive(false);
        Ere_anger01go.SetActive(false);
    }

    private void EreSad01()
    {
        Erego.SetActive(false);
        Ere_sad01go.SetActive(true);
        Ere_anger01go.SetActive(false);
    }

    private void EreAnger01()
    {
        Erego.SetActive(false);
        Ere_sad01go.SetActive(false);
        Ere_anger01go.SetActive(true);
    }

}
