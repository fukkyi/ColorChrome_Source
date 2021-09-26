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

        [SerializeField] private Canvas[] sortingGroup; // キャラの描画順を変える

        private int currentLine = 0;
        private string currentText = string.Empty;  // 現在の文字列
        private float timeUntilDisplay = 0;         // 表示にかかる時間
        private float timeElapsed = 1;              // 文字列の表示を開始した時間
        private int lastUpdateCharacter = -1;       // 表示中の文字列

        // 文字の表示が完了しているかどうか
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
                // 完了していない文字をすべて表示する
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    timeUntilDisplay = 0;
                    //Debug.Log("マウス(左)");
                }
                else if (Gamepad.current.bButton.wasPressedThisFrame)
                {
                    timeUntilDisplay = 0;
                    Debug.Log("PS4〇");
                }
            }

            // クリックから経過した時間が想定表示時間の何％か確認し、表示文字数を出す
            int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);

            // 表示文字数が前回の表示文字数と異なるならテキストを更新する
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
                sortingGroup[i].sortingOrder = 1;       // sortingGroup = 描画順を制御するために使ったりする
            }

            switch (name)
            {
                case "ナレーション":
                    break;

                case "エレ":
                    CharPos[0].color = new Color(1, 1, 1, 1.0f);
                    sortingGroup[0].sortingOrder = 10;
                    break;

                case "ケオ":
                    CharPos[1].color = new Color(1, 1, 1, 1.0f);
                    sortingGroup[1].sortingOrder = 10;
                    break;

                case "雹禍":
                    CharPos[1].color = new Color(1, 1, 1, 1.0f);
                    sortingGroup[1].sortingOrder = 10;
                    break;

                case "業架":
                    CharPos[1].color = new Color(1, 1, 1, 1.0f);
                    sortingGroup[1].sortingOrder = 10;
                    break;

                case "一輪薔薇":
                    CharPos[1].color = new Color(1, 1, 1, 1.0f);
                    sortingGroup[1].sortingOrder = 10;
                    break;
            }

            currentLine++;

            // 想定表示時間と現在の時刻をキャッシュ
            timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
            timeElapsed = Time.time;

            // 文字カウントを初期化
            lastUpdateCharacter = -1;
        }

        void SceneDataInit()
        {
            // cc_Scenario01 ~ cc_Scenario09
            // cc_Scenario01 冒頭、シーン別で一つ
            CSVReader("cc_Scenario01", TextContant, Character_Name, Character_num);
            //// cc_Scenario02 本編開始後、ケオが囚われている荊(オブジェクト)に近づいたら
            //CSVReader("cc_Scenario02", TextContant, Character_Name, Character_num);
            //// cc_Scenario03 雹禍(中ボス)と接敵したら
            //CSVReader("cc_Scenario03", TextContant, Character_Name, Character_num);
            //// cc_Scenario04 雹禍との戦いに勝利したら
            //CSVReader("cc_Scenario04", TextContant, Character_Name, Character_num);
            //// cc_Scenario05 04のシナリオ終了後、ケオが囚われている荊に近づいたら
            //CSVReader("cc_Scenario05", TextContant, Character_Name, Character_num);
            //// cc_Scenario06 雑魚敵を一定数倒した後、業架との会話
            //CSVReader("cc_Scenario06", TextContant, Character_Name, Character_num);
            //// cc_Scenario07 06終了後、荊に近づいたら
            //CSVReader("cc_Scenario07", TextContant, Character_Name, Character_num);
            //// cc_Scenario08.09 一輪薔薇との戦闘終了後、分岐によってEDが変わる
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


        //// 参照したテキストファイルの何行目を読んでいるのか
        //private int currentNum = 0;
        //// 表示中の文字列
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
