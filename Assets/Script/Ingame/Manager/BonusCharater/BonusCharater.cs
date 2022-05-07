using System;
using System.IO;
using Charater;
using Fire;
using Ingame;
using UnityEngine;
using Score;

namespace Manager
{
    public class BonusCharater : MonoBehaviour
    {
        [Header("Bonus")]
        private bool[] _Bonus_Check = new bool[6]; // 보너스 글자 모음 여부를 체크하는 변수 
        private bool[] _Bonus_Respawn = new bool[6];
        public Transform _Bonus_TR;
        public GameObject charater_obj;
        private bool bonus_check;

        [Header("UI")] public GameObject Bonus_Time;

        [SerializeField] private LocateBox _locateBox;

        private void Start()
        {
            if (PlayerPrefs.GetInt("Still_Game", 0) == 1)
                Load_SaveData();
        }

        /// <summary>
        /// 이어하기 시 호출되는 함수. 
        /// </summary>
        public void Load_SaveData()
        {
            CandleCharaterVO data;
            var DATA_PATH = Application.persistentDataPath + "/Ingame_Data/CandleShown_Data.json";
            
            // Step 1. 정보를 불러옴
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                data = JsonUtility.FromJson<CandleCharaterVO>(json_string); 
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Ingame_Data");
                data = new CandleCharaterVO();
                var DATA_STR = JsonUtility.ToJson(data);
                File.WriteAllText(DATA_PATH,DATA_STR);   
            }
            // Step 2. UI상에  및 데이터 반영해줌 
            _Bonus_Check[0] = data.firstCharater ;
            _Bonus_Check[1] = data.secondCharater;
            _Bonus_Check[2] = data.thirdCharater;
            _Bonus_Check[3] = data.forthCharater;
            _Bonus_Check[4] = data.fifthCharter;
            _Bonus_Check[5] = data.sixthCharater;

            for (int i = 0; i < 6; i++)
            {
                if(_Bonus_Check[i])
                    _Bonus_TR.GetChild(i).gameObject.SetActive(true);
            }

        }

        // 어떤 글자가 습득되었는지를 저장하는 함수 
        public void Save_Charater_Data()
        {
            CandleCharaterVO data = new CandleCharaterVO();
            data.firstCharater = _Bonus_Check[0];
            data.secondCharater = _Bonus_Check[1];
            data.thirdCharater = _Bonus_Check[2];
            data.forthCharater = _Bonus_Check[3];
            data.fifthCharter = _Bonus_Check[4];
            data.sixthCharater = _Bonus_Check[5];
            
            
            var DATA_PATH = Application.persistentDataPath + "/Ingame_Data/CandleShown_Data.json";
            var DATA = JsonUtility.ToJson(data);
            File.WriteAllText(DATA_PATH,DATA);   
           
        }
        
        
        /// <summary>
        /// 글자를 획득하면 호출되는 함수 
        /// </summary>
        /// <param name="type"></param>
        public void Set_Bonus_Charater(int type, Vector2 pos, bool is_destroy)
        {
            if (!_locateBox.is_pinata())
            {
                GameObject obj;
                Charater_Flight obj_script;
                if (is_destroy)
                {
                    obj = Instantiate(charater_obj, pos, Quaternion.identity);
                    obj_script = obj.GetComponent<Charater_Flight>();
                    obj_script.index = type;
                    obj_script.target_obj = _Bonus_TR.GetChild(type).gameObject;
                    _Bonus_Check[type] = true;
                }

                
                else
                {
                    _Bonus_Respawn[type] = true;
                }
                
                
            }
        }

        public void Set_Bonus_Charater_init()
        {
            for (int i = 0; i < _Bonus_Check.Length; i++)
            {
                _Bonus_Check[i] = false;
                _Bonus_Respawn[i] = false;
            }
            
        }
        
        public bool Get_Bonus_Charater(int type)
        {
            return _Bonus_Respawn[type];
        }
        
        /// <summary>
        /// 피냐타가 파괴되면 호출되는 함수 
        /// </summary>
        public void Bonus_Extinguish()
        {
            bonus_check = false;
            for (int i = 0; i < _Bonus_TR.childCount; i++)
                _Bonus_TR.GetChild(i).gameObject.GetComponent<Flame_Down>().Extinquish();
            
        }


        #region Get

        /// <summary>
        /// 피냐타 소환 여부를 리턴하는 함수 
        /// </summary>
        /// <returns></returns>
        public bool Get_BonusCheck()
        {
            if (bonus_check)
            {
                Bonus_Time.SetActive(true);
                Set_Bonus_Charater_init();
            }
            
            return bonus_check;
        }

        public void Do_BonusCheck()
        {            
            for (int i = 0; i < _Bonus_Check.Length; i ++ )
            {
                if (!_Bonus_Check[i])
                {
                    bonus_check = false;
                    return;
                }
            }
            bonus_check = true;
        }
        

        #endregion
        
    }
    
}
