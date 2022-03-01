using System.Collections.Generic;
using System;
using Badge;
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
            candleConst = AbilityDAO.Get_Candle(); // 데이터 잘 반영되는지 확인해보기 
            Set_Charater_Target(1);
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
        public void Set_Charater_Target(int nowStage)
        {
            var startStage = nowStage;
            int targetStage;
            int firstCharater;
            int charaterListCount;
            charater_target = new List<Tuple<int, int>>(); // 리스트 새로 만들어줌 
            List<int> candidate_stage = new List<int>(); // 문자들이 뜰 charater stage 
            List<int> charaterList = new List<int>();

            Set_TargetInterval(); // Pre. interval 초기화 후 계산. (튜토리얼 때문)
            targetStage = Set_TargetStage(startStage);

            for (int i = 0; i < 6; i++)
                charaterList.Add(i);

            for (int i = startStage+1; i < targetStage; i++)
            {
                candidate_stage.Add(i);
            }

            firstCharater = UnityEngine.Random.Range(0, charaterList.Count);
            charater_target.Add (new Tuple<int, int>(charaterList[firstCharater], targetStage));
            charaterList.RemoveAt(firstCharater);
            if (Tutorial_Candle_Data.Get_respawn_Data() && startStage == 5)
            {
                firstCharater = UnityEngine.Random.Range(0, charaterList.Count);
                charater_target.Add (new Tuple<int, int>(charaterList[firstCharater], startStage));
                charaterList.RemoveAt(firstCharater);
            }

            charaterListCount = charaterList.Count;
            
            for (int i = 0; i <charaterListCount ; i++)
            {
                int charater = UnityEngine.Random.Range(0, charaterList.Count);
                int stage = UnityEngine.Random.Range(0, candidate_stage.Count);
                charater_target.Add( new Tuple<int, int>(charaterList[charater], candidate_stage[stage]));
                charaterList.RemoveAt(charater);
                candidate_stage.RemoveAt(stage);
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
