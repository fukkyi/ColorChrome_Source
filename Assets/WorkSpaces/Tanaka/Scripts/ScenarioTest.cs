using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ScenarioTest : MonoBehaviour
{
    [SerializeField] private Text textLabel;
    [SerializeField] private TextAsset textFile;
    [SerializeField] private GameObject ScenarioPanel;

    private string textData;
    private string[] splitText;

    
    // 参照したテキストファイルの何行目を読んでいるのか
    private int currentNum = 0;
    // 表示中の文字列
    private int lastUpdateCharCount = -1;

    //public string[] textMessage;
    //public string[,] textWords;

    //private int rowLength;
    //private int columnLength;

    private void Start()
    {
        textData = textFile.text;
        splitText = textData.Split(char.Parse("\n"));
        textLabel.text = splitText[currentNum];
        //textData.Substring(0.Length++);

        ScenarioPanel.SetActive(true);

    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            SetNextScentence();
        }
    }

    private void SetNextScentence()
    {
        currentNum = (currentNum + 1) % splitText.Length;
        textLabel.text = splitText[currentNum];
        //Debug.Log(currentNum);

        if (currentNum == 20)
        {
            ScenarioPanel.SetActive(false);
        }
    }

}
