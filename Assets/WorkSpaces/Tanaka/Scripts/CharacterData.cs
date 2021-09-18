using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    int id;
    string name;
    int hp;
    int at;

    public CharacterData(string[] _dataList)
    {
        id = int.Parse(_dataList[0]);
        name = _dataList[1];
        hp = int.Parse(_dataList[2]);
        at = int.Parse(_dataList[3]);
    }
    public void DebugParametaView()
    {
        Debug.Log(String.Format("{0} id:{1} hp:{2} at:{3}", name, id, hp, at));
    }
}
