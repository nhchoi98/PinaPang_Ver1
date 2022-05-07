
using System;
using System.Collections.Generic;
using Ingame_Data;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Collection;
using Data;

public class Observer_CollectionInfo : MonoBehaviour, IObserver_Ingame
{
    
    private bool isInit = false;
    
    [Header("Setting")]
    JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();

    [SerializeField] private CollectionManager _collectionManager;
    
    public void Update_Status()
    {
        if (!isInit)
        {
            isInit = true;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }
    }

    public void LoadData_ToIngame()
    {
        List<Tuple<int, int>> collectionList = new List<Tuple<int, int>>();
        Read_Data(ref collectionList); // 데이터를 불러옴 
        _collectionManager.Load_Data(ref collectionList);
    }

    public void Get_CollectionData(ref List<Tuple<int, int>> collectionList)
    {
        Write_Data(ref collectionList);
    }
    
    private void Read_Data(ref List<Tuple<int, int>> collectionList)
    {
        // Step 1. 파일이 없으면, 만들고 시작 
        var DATA_PATH = Application.persistentDataPath + "/Ingame_Data/Collection_data.json";
        if (!File.Exists(DATA_PATH)) // Path에 파일이 없다면...
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Ingame_Data");
            var str = JsonConvert.SerializeObject(collectionList,settings); // 아예 비어있으면?
            File.WriteAllText(DATA_PATH,str);    
        }

        else
        {
            var json_string = File.ReadAllText(DATA_PATH);
            collectionList = JsonConvert.DeserializeObject<List<Tuple<int,int>>>(json_string);
        }
    }

    private void Write_Data(ref List<Tuple<int, int>> collectionList)
    {
        // Step 1. 파일이 없으면, 만들고 시작 
        var DATA_PATH = Application.persistentDataPath + "/Ingame_Data/Collection_data.json";
        if (!File.Exists(DATA_PATH)) // Path에 파일이 없다면...
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Ingame_Data");
            var str = JsonConvert.SerializeObject(collectionList,settings);
            File.WriteAllText(DATA_PATH,str);   
        }

        else
        {
            // 호출됨 
            var str = JsonConvert.SerializeObject(collectionList,settings);
            File.WriteAllText(DATA_PATH,str); 
        }
        // 저장할 애들을 List에서 빼와서 Arr의 형태로 저장함.
    }

    
    internal class Wrapper
    {
        [JsonProperty("JsonValues")]
        public List<Tuple<int, int>> collectionList { get; set; }
            
    }
}
