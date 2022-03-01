using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Runtime.Serialization.Json;
using Ingame_Data;

public class Temp_BallScript : MonoBehaviour
{
    private BallInfo_VO ballData;

    private void Awake()
    {
        ballData = new BallInfo_VO();
        ballData.ballNum = 0;
        ballData.ballPos = new Vector2(1f, 1f);
        ballData.charPos = new Vector2(1f, 1f);
        ballData.is_Fliped = false;
        var DATA = JsonUtility.ToJson(ballData);
        Debug.Log(DATA);
        var DATA_PATH = Application.persistentDataPath + "/Ingame_Data/BallInfo.json";
        File.WriteAllText(DATA_PATH,DATA);   
    }


}
