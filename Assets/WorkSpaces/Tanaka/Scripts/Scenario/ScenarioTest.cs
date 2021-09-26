using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.IO;

namespace CharacterController
{

    public class ScenarioTest : MonoBehaviour
    {

        public string Name { get; set; }
        public string Text { get; set; }
        public string num { get; set; }

        //[SerializeField] private string[] sentence;
        [SerializeField] private Text uiText;

        [SerializeField, Range(0.01f, 0.3f)]
        float intervalForCharacterDisplay = 0.05f;

        [SerializeField] Image[] CharPos;
        [SerializeField] Sprite[] CharImage;

        [SerializeField] private Canvas[] sortingGroup; // �L�����̕`�揇��ς���

        private int currentLine = 0;
        private string currentText = string.Empty;  // ���݂̕�����
        private float timeUntilDisplay = 0;         // �\���ɂ����鎞��
        private float timeElapsed = 1;              // ������̕\�����J�n��������
        private int lastUpdateCharacter = -1;       // �\�����̕�����

        // �����̕\�����������Ă��邩�ǂ���
        public bool IsCompleteDisplayText
        {
            get { return Time.time > timeElapsed + timeUntilDisplay; }
        }

        private List<string> TextContant = new List<string>();
        private List<string> Character_Name = new List<string>();
        private List<string> Character_num = new List<string>();

        private List<string> Character_IMG = new List<string>();
        private List<string> Character_IMG_Position = new List<string>();

        void Awake()
        {
            SceneDataInit();
        }

        void Start()
        {
            string path = @"cc_Scenario01";
            var csv = Resources.Load(path) as TextAsset;
            var sr = new System.IO.StringReader(csv.text);

            SetNextLine();
        }

        void Update()
        {
            if (IsCompleteDisplayText)
            {
                if (currentLine < TextContant.Count && Mouse.current.leftButton.wasPressedThisFrame)
                {
                    SetNextLine();
                }
            }
            else
            {
                // �������Ă��Ȃ����������ׂĕ\������
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    timeUntilDisplay = 0;
                    //Debug.Log("�}�E�X(��)");
                }
                else if (Gamepad.current.bButton.wasPressedThisFrame)
                {
                    timeUntilDisplay = 0;
                    Debug.Log("PS4�Z");
                }
            }

            // �N���b�N����o�߂������Ԃ��z��\�����Ԃ̉������m�F���A�\�����������o��
            int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);

            // �\�����������O��̕\���������ƈقȂ�Ȃ�e�L�X�g���X�V����
            if (displayCharacterCount != lastUpdateCharacter)
            {
                uiText.text = currentText.Substring(0, displayCharacterCount);
                lastUpdateCharacter = displayCharacterCount;
            }
        }

        private void SetNextLine()
        {
            currentText = TextContant[currentLine];

            string name = Character_Name[currentLine];
            int num = int.Parse(Character_num[currentLine]);

            for (int i = 0; i < CharPos.Length; i++)
            {
                CharPos[i].color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                sortingGroup[i].sortingOrder = 1;       // sortingGroup = �`�揇�𐧌䂷�邽�߂Ɏg�����肷��
            }

            switch (name)
            {
                case "�i���[�V����":
                    break;

                case "�G��":
                    CharPos[0].color = new Color(1, 1, 1, 1.0f);
                    sortingGroup[0].sortingOrder = 10;
                    break;

                case "�P�I":
                    CharPos[1].color = new Color(1, 1, 1, 1.0f);
                    sortingGroup[1].sortingOrder = 10;
                    break;

                case "蹉�":
                    CharPos[1].color = new Color(1, 1, 1, 1.0f);
                    sortingGroup[1].sortingOrder = 10;
                    break;

                case "�Ɖ�":
                    CharPos[1].color = new Color(1, 1, 1, 1.0f);
                    sortingGroup[1].sortingOrder = 10;
                    break;

                case "����K�N":
                    CharPos[1].color = new Color(1, 1, 1, 1.0f);
                    sortingGroup[1].sortingOrder = 10;
                    break;
            }

            currentLine++;

            // �z��\�����Ԃƌ��݂̎������L���b�V��
            timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
            timeElapsed = Time.time;

            // �����J�E���g��������
            lastUpdateCharacter = -1;
        }

        void SceneDataInit()
        {
            // cc_Scenario01 ~ cc_Scenario09
            // cc_Scenario01 �`���A�V�[���ʂň��
            CSVReader("cc_Scenario01", TextContant, Character_Name, Character_num);
            //// cc_Scenario02 �{�ҊJ�n��A�P�I�������Ă���t(�I�u�W�F�N�g)�ɋ߂Â�����
            //CSVReader("cc_Scenario02", TextContant, Character_Name, Character_num);
            //// cc_Scenario03 蹉�(���{�X)�ƐړG������
            //CSVReader("cc_Scenario03", TextContant, Character_Name, Character_num);
            //// cc_Scenario04 蹉ЂƂ̐킢�ɏ���������
            //CSVReader("cc_Scenario04", TextContant, Character_Name, Character_num);
            //// cc_Scenario05 04�̃V�i���I�I����A�P�I�������Ă���t�ɋ߂Â�����
            //CSVReader("cc_Scenario05", TextContant, Character_Name, Character_num);
            //// cc_Scenario06 �G���G����萔�|������A�Ɖ˂Ƃ̉�b
            //CSVReader("cc_Scenario06", TextContant, Character_Name, Character_num);
            //// cc_Scenario07 06�I����A�t�ɋ߂Â�����
            //CSVReader("cc_Scenario07", TextContant, Character_Name, Character_num);
            //// cc_Scenario08.09 ����K�N�Ƃ̐퓬�I����A����ɂ����ED���ς��
            //// End1.BadEnd
            //CSVReader("cc_Scenario08", TextContant, Character_Name, Character_num);
            //// End2.HappyEnd
            //CSVReader("cc_Scenario09", TextContant, Character_Name, Character_num);
        }

        void CSVReader(string CSVPATH,
                    List<string> text,
                    List<string> Name,
                    List<string> num)
        {
            TextAsset data;
            data = Resources.Load(CSVPATH, typeof(TextAsset)) as TextAsset;
            StringReader sr = new StringReader(data.text);

            string line;
            string[] text_split;
            line = sr.ReadLine();

            while (line != null)
            {
                text_split = line.Split(',');
                Name.Add(text_split[0]);
                text.Add(text_split[1]);
                num.Add(text_split[2]);
                line = sr.ReadLine();
            }

            text.RemoveAt(0);
            Name.RemoveAt(0);
            num.RemoveAt(0);
        }

        //[SerializeField] private Text textLabel;
        //[SerializeField] private TextAsset textFile;
        //[SerializeField] private GameObject ScenarioPanel;

        //private string textData;
        //private string[] splitText;


        //// �Q�Ƃ����e�L�X�g�t�@�C���̉��s�ڂ�ǂ�ł���̂�
        //private int currentNum = 0;
        //// �\�����̕�����
        //private int lastUpdateCharCount = -1;

        ////public string[] textMessage;
        ////public string[,] textWords;

        ////private int rowLength;
        ////private int columnLength;

        //private void Start()
        //{
        //    textData = textFile.text;
        //    splitText = textData.Split(char.Parse("\n"));
        //    textLabel.text = splitText[currentNum];
        //    //textData.Substring(0.Length++);

        //    ScenarioPanel.SetActive(true);

        //}

        //private void Update()
        //{
        //    if (Mouse.current.leftButton.wasPressedThisFrame)
        //    {
        //        SetNextScentence();
        //    }
        //}

        //private void SetNextScentence()
        //{
        //    currentNum = (currentNum + 1) % splitText.Length;
        //    textLabel.text = splitText[currentNum];
        //    //Debug.Log(currentNum);

        //    if (currentNum == 20)
        //    {
        //        ScenarioPanel.SetActive(false);
        //    }
        //}

    }
}
