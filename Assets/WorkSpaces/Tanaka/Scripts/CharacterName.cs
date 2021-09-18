using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterName : MonoBehaviour
{
    [SerializeField] private Text textNameLabel;
    [SerializeField] private TextAsset textNameFile;

    private string textData;
    private string[] splitText;

    private int currentNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        textData = textNameFile.text;
        splitText = textData.Split(char.Parse("\n"));
        textNameLabel.text = splitText[currentNum];
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            currentNum = (currentNum + 1) % splitText.Length;
            textNameLabel.text = splitText[currentNum];
        }
    }
}
