
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using Ad;
using LitJson;


/// <summary>
/// 로비로 들어 왔을 때, 이어하기 유무를 물어보는 패널의 스크립트. 
/// </summary>
public class Continuos_Stage_Panel : MonoBehaviour
{
    private StageInfoVO stageData;
    public Text stageText;
    [SerializeField] private Lobby_Ad_Mediator _adMediator;
    public void OnClick_Yes()
    {
        _adMediator.Remove_CallBack(); // 광고 이벤트 제거 
        PlayerPrefs.SetInt("Ingame",1);
        SceneManager.LoadScene("Loading_Scene_Game");
    }

    public void OnClick_No()
    {
        PlayerPrefs.SetInt("Still_Game", 0);
        this.gameObject.SetActive(false);
    }
    
    void Start()
    {
        Read_Data(); // 정보를 읽어와서..
        stageText.text = "STAGE " + (stageData.stage-1).ToString(); // 스테이지 정보를 텍스트로써 반영해줌.
    }

    
    public void Read_Data()
    {
        var DATA_PATH = Application.persistentDataPath + "/Ingame_Data/StageInfo.json";
        StageInfoVO DATA = null;
        if (File.Exists(DATA_PATH))
        {
            var json_string = File.ReadAllText(DATA_PATH);
            DATA = JsonMapper.ToObject<StageInfoVO>(json_string);
        }

        else
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Ingame_Data");
            DATA = new StageInfoVO();
            DATA.is_Best = false;
            DATA.score = 0;
            DATA.stage = 0;
            DATA.wave = 0;
            DATA.is_Revive = false;
            var DATA_STR = JsonUtility.ToJson(DATA) ;
            File.WriteAllText(DATA_PATH,DATA_STR);   
        }

        stageData = DATA;
    }

}
