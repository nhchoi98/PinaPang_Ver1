using UnityEngine;
using Challenge;

namespace Alarm
{
    public class Quest_Alarm {
        private bool[] alarm_info;
        // #1. 배열을 저장하는 변수
        // #2. UI 컨트롤에서 Add listener 하고 (QuestManager에서) 알림을 띄워줘야겠네. 뱃지도 마찬가지고. 

        public Quest_Alarm(ref ChallengeDAO data)
        {
            alarm_info = new bool[5];
            Read_Data(ref data); // 데이터를 읽어들임 
        }


        /// <summary>
        /// 데이터를 읽어들이는 함수 
        /// </summary>
        public void Read_Data(ref ChallengeDAO data)
        {
            for (int i = 0; i < 5; i++)
            {
                if (data.Get_Achievement(i) && !data.Get_itemget_data(i))
                    alarm_info[i] = true;

                else
                    alarm_info[i] = false;
            }
        }

        public void Set_Data(int index, bool is_false)
        {
            if (is_false)
                alarm_info[index] = false;

            else
                alarm_info[index] = true;
        }

        public bool Get_Data(int index)
        {
            return alarm_info[index];
        }
        

    }
}
