using System.Collections.Generic;
using System;
using System.IO;
using Badge;
using Charater;
using Tutorial;
using UnityEngine;

namespace Ingame
{
    // 인게임의 캔들박스 등장확률을 결정짓는 로직이 포함된 class. 
    public class Determine_CandleObj
    {
        private float candleConst;
        private int targetInterval; // 고정적으로 가진 target Interval
        private int target_width; // 위 아래 최대 얼마정도의 변동 폭을 가질 수 있는지를 저장하고 있는 변수 
        private List<Tuple<int, int>> charater_target; // 어느 문자가 어느 턴에 뜰지를 가지고 있는 List. item 1: 어떤 문자? item 2:몇 턴?
        public Determine_CandleObj()
        {
            candleConst = AbilityDAO.Get_Candle();  
            if (PlayerPrefs.GetInt("Still_Game", 0) == 1) // 이어하기 중이라면..
                Load_Data();
            

            else // 이어하기로 시작하는게 아니라면 
                Set_Charater_Target(1);
            
        }
        
        // 정보를 저장해주는 함수
        private void Load_Data()
        {
            CharaterInfoVO data;
            Set_TargetInterval(); // Pre. interval 초기화 
            var DATA_PATH = Application.persistentDataPath + "/Ingame_Data/CharaterData.json";
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                data = JsonUtility.FromJson<CharaterInfoVO>(json_string); 
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Ingame_Data");
                data = new CharaterInfoVO();
                var DATA_STR = JsonUtility.ToJson(data);
                File.WriteAllText(DATA_PATH,DATA_STR);   
            }
            
            
            if (data.pinata_isOn) // 정보를 불러와 피냐타가 떴다고 한 상태면,, stage정보를 불러와 리스트를 새로 만들어줌.. 분기문 필요..
                Set_Charater_Target(data.nowStage);

            else
            {
                charater_target = new List<Tuple<int, int>>(); // 리스트 새로 만들어줌 
                charater_target.Add(new Tuple<int, int>(data.first_type,data.first_stage));
                charater_target.Add(new Tuple<int, int>(data.second_type,data.second_stage));
                charater_target.Add(new Tuple<int, int>(data.third_type,data.third_stage));
                charater_target.Add(new Tuple<int, int>(data.forth_type,data.forth_stage));
                charater_target.Add(new Tuple<int, int>(data.fifth_type,data.fifth_stage));
                charater_target.Add(new Tuple<int, int>(data.sixth_type,data.sixth_stage));
            }
        }
        // 정보를 로드해주는 함수. 이 때, 튜토리얼 스테이지 진행 전 일경우에는 별도로 초기화를 진행하지 않는다.. 

        
        
        public void Save_Data(int stage, bool pinata_isOn)
        {
            CharaterInfoVO data = new CharaterInfoVO();
            data.nowStage = stage;
            data.pinata_isOn = pinata_isOn;
            
            data.first_stage = charater_target[0].Item2;
            data.first_type = charater_target[0].Item1;
            
            data.second_stage = charater_target[1].Item2;
            data.second_type = charater_target[1].Item1;
            
            data.third_stage = charater_target[2].Item2;
            data.third_type = charater_target[2].Item1;
            
            data.forth_stage = charater_target[3].Item2;
            data.forth_type = charater_target[3].Item1;
            
            data.fifth_stage = charater_target[4].Item2;
            data.fifth_type = charater_target[4].Item1;
            
            data.sixth_stage = charater_target[5].Item2;
            data.sixth_type = charater_target[5].Item1;
            
            
            var DATA_PATH = Application.persistentDataPath + "/Ingame_Data/CharaterData.json";
            var DATA = JsonUtility.ToJson(data);
            File.WriteAllText(DATA_PATH,DATA);   
            // 경로 지정해주기 
        }
        
        
        private void Set_TargetInterval()
        {
            float _const;
            float solution; 
            // 피냐타 튜토리얼 진행중일 경우, 확률을 강제적으로 높임 
            if (PlayerPrefs.GetInt("Tutorial_Pinata", 0) == 0)
                _const = (candleConst * 3f)/100f;
            
            else
                _const = (candleConst)/100f; 
            
            // Step 2. 인터벌을 지정해줌 
            solution = 10f / _const;
            targetInterval = (int) (solution*6);
            if (targetInterval < 7)
                targetInterval = 7;
            
            target_width = (int)(5f * (targetInterval / 35f));
            if (target_width < 1)
                target_width = 1;
        }
        
        public bool Determine_Candle(bool pinata_ison, int stage)
        {
            if (pinata_ison || !Tutorial_Candle_Data.Get_Done_Data())
            {
                return false;
            }

            else
            {
                if (charater_target.Count == 0)
                    return false;
                
                for (int i = 0; i < charater_target.Count; i++)
                {
                    if (charater_target[i].Item2 == stage)
                        return true;
                    
                }
                
                return false;
            }
        }

        public int which_charater(int stage)
        {
            for (int i = 0; i < charater_target.Count; i++)
            {
                if (charater_target[i].Item2 == stage)
                    return charater_target[i].Item1;
                    
            }

            return -1;
        }


        /// <summary>
        /// 박스가 어느 턴에 뜰 지를 List<int,int>로 묶어 저장하게 해주는 func
        /// </summary>
        public void Set_Charater_Target(int nowStage, bool is_Load = false)
        {
            var startStage = nowStage;
            int targetStage;
            int firstCharater;
            int charaterListCount;
            charater_target = new List<Tuple<int, int>>(); // 리스트 새로 만들어줌 
            List<int> candidate_stage = new List<int>(); // 문자들이 뜰 charater stage 
            List<int> charaterList = new List<int>();

            if (!is_Load)
            {
                Set_TargetInterval(); // Pre. interval 초기화 후 계산. (튜토리얼 때문)
                targetStage = Set_TargetStage(startStage);

                for (int i = 0; i < 6; i++)
                    charaterList.Add(i);

                for (int i = startStage + 1; i < targetStage; i++)
                {
                    candidate_stage.Add(i);
                }

                firstCharater = UnityEngine.Random.Range(0, charaterList.Count);
                charater_target.Add(new Tuple<int, int>(charaterList[firstCharater], targetStage));
                charaterList.RemoveAt(firstCharater);
                if (Tutorial_Candle_Data.Get_respawn_Data() && startStage == 5)
                {
                    firstCharater = UnityEngine.Random.Range(0, charaterList.Count);
                    charater_target.Add(new Tuple<int, int>(charaterList[firstCharater], startStage));
                    charaterList.RemoveAt(firstCharater);
                }

                charaterListCount = charaterList.Count;

                for (int i = 0; i < charaterListCount; i++)
                {
                    int charater = UnityEngine.Random.Range(0, charaterList.Count);
                    int stage = UnityEngine.Random.Range(0, candidate_stage.Count);
                    charater_target.Add(new Tuple<int, int>(charaterList[charater], candidate_stage[stage]));
                    charaterList.RemoveAt(charater);
                    candidate_stage.RemoveAt(stage);
                }
            }

            // Step 1. 현재 턴 수로 부터 피냐타 글자가 다 완성되는 포인트를 정함. 마지막에 들어가는 무조건 여기 들어가야함.


        }


        /// <summary>
        /// 다음 피냐타가 뜨기까지의 Target Stage를 계산해주는 함수. 타깃 인터벌을 기준으로 +-의 폭을 지정해준다.
        /// </summary>
        /// <returns></returns>
        private int Set_TargetStage(int nowStage)
        {
            int targetStage;
            if (targetInterval > 7)
            {
                if (UnityEngine.Random.Range(0, 2) == 1)
                    targetStage = nowStage + targetInterval + UnityEngine.Random.Range(0, target_width);

                else
                    targetStage = nowStage + targetInterval - UnityEngine.Random.Range(0, target_width);
            }

            else
            {
                targetStage = nowStage + targetInterval + UnityEngine.Random.Range(0, target_width);
            }
            
            return targetStage;

        }
    }
}
