using System;
using LitJson;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace Challenge
{
    public class ChallengeDAO
    {
        private ChallengeVO[] data;
        private Chal_Target target_data;
        private List<int> bronze_list;
        private List<int> silver_list;
        private List<int> gold_list;
        public ChallengeDAO()
        {
            data = new ChallengeVO[5]; // 배열 생성
            for (int i = 0; i < 5; i++)
                Read_Data(i);
            
            target_data = new Chal_Target();
        }

        /// <summary>
        /// 하나의 데이터만 Set,Get 하고 싶을 경우 
        /// </summary>
        /// <param name="index"></param>
        public ChallengeDAO(int index)
        {
            data = new ChallengeVO[1];
            Read_Data(index);
            target_data = new Chal_Target();
        }
    
        #region IO

        /// <summary>
        /// 데이터 로드 
        /// </summary>
        private void Read_Data(int index)
        {
            ChallengeVO data;
            var DATA_PATH = Application.persistentDataPath + "/Challenge/daily/" + index.ToString() + ".json";
            var PRE_DATA_PATH = "Challenge/daily/" +index ;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                data = JsonMapper.ToObject<ChallengeVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Challenge/daily");
                TextAsset DATA_ASSET = Resources.Load(PRE_DATA_PATH) as TextAsset;
                var _DATA = DATA_ASSET.ToString();
                File.WriteAllText(DATA_PATH, _DATA);
                data = JsonMapper.ToObject<ChallengeVO>(_DATA);
            }

            this.data[index] = data;
        }

        /// <summary>
        /// 데이터 저장 
        /// </summary>
        private void Set_Data(int index)
        {
            var DATA = JsonMapper.ToJson(this.data[index]);
            var DATA_PATH = Application.persistentDataPath + "/Challenge/daily/" + index.ToString() + ".json";
            File.WriteAllText(DATA_PATH,DATA);
        }
        
        
        #endregion

        #region Get

        /// <summary>
        /// 달성 여부를 리턴해주는 함수 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Get_Achievement(int index)
        {
            return data[index].achi;
        }

        public bool Get_itemget_data(int index)
        {
            return data[index].get;
        }

        public int Get_Item_index(int index)
        {
            return data[index].item;
        }

        /// <summary>
        /// UI에 반영해줄 데이터들을 넘겨줌 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="target_num"></param>
        /// <param name="num"></param>
        public void Get_Challenge_data(int index, ref int target_num, ref int num)
        {
            target_num = target_data.Get_Chal_Target(data[index].item);
            num = data[index].count;
            if (num > target_num)
                num = target_num;
        }
        
        #endregion

        #region Reset
        /// <summary>
        /// 기준시가 지나면, 데이터가 초기화될 때 호출되는 함수 
        /// </summary>
        public void Init_data(bool loading)
        {
            int user_rank = Challenge_Slot.Calc_Rank(); // 사용자의 tier 값을 저장함 
            int return_value = 0; // 슬롯이 바뀔 때의 값을 저장하는 변수 
            bronze_list = new List<int>();
            silver_list = new List<int>();
            gold_list = new List<int>();

            for (int i = 0; i < 9; i++)
            {
                bronze_list.Add(i*3);
                silver_list.Add((i*3) +1);
                gold_list.Add((i*3)+2);
            }
            gold_list.Add(27); // 퀘스트 클리어 미션 
            
            // #1. 각 슬롯의 아이템 index를 결정해줌 
            //case 1 첫 번째 슬롯을 랜덤하게 선택함 
            if (PlayerPrefs.GetInt("Tutorial_Exchange", 0) == 1)
            {
                data[0].item = Set_List(ref bronze_list);
                Remove_Specific_Quest(data[0].item, ref bronze_list, ref silver_list, ref gold_list);
            }

            else // 아직 교환소 튜토리얼을 완료하지 못한경우 
            {
                data[0].item = 24;
                Set_Specific_List(24,ref bronze_list);
                Remove_Specific_Quest(data[0].item, ref bronze_list, ref silver_list, ref gold_list);
            }

            // case 2. 두 번째 슬롯~ 다섯 번째 슬롯을 static 함수를 통해 초기화 해줌 
            // 두번째 슬롯
            return_value = Challenge_Slot.Set_Second_Slot(user_rank);
            Set_Slot(1, return_value, ref data);
            Remove_Specific_Quest(data[1].item, ref bronze_list, ref silver_list, ref gold_list);
            // 세번째 슬롯 
            return_value = Challenge_Slot.Set_Third_Slot(user_rank);
            Set_Slot(2, return_value, ref data);
            Remove_Specific_Quest(data[2].item, ref bronze_list, ref silver_list, ref gold_list);
            // 네번째 슬롯 
            return_value = Challenge_Slot.Set_Forth_Slot(user_rank);
            Set_Slot(3, return_value, ref data);
            Remove_Specific_Quest(data[3].item, ref bronze_list, ref silver_list, ref gold_list);
            // 다섯번째 슬롯 
            return_value = Challenge_Slot.Set_Fifth_Slot(user_rank);
            Set_Slot(4, return_value, ref data);


            // 모든 데이터 초기화 
            if (loading)
            {
                PlayerPrefs.SetInt("chal_count", 0); // 광고 시청 회수 초기화
            }

            for (int i = 0; i < 5; i++)
            {
                data[i].achi = false;
                data[i].get = false;
                data[i].count = 0;
                Set_Data(i);
            }
           
        }
        /// <summary>
        /// 슬롯 밸류를 토대로 데이터를 반영해주는 함수 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="slot_value"></param>
        private void Set_Slot(int index, int slot_value, ref ChallengeVO[] data)
        {
            // 브론즈 등급 아이템일 때 
            if(slot_value == 0)
                data[index].item = Set_List(ref bronze_list);
            
            else if (slot_value == 1)
                data[index].item = Set_List(ref silver_list);
            
            else
                data[index].item = Set_List(ref gold_list);
        }

        /// <summary>
        /// 하나가 뽑히면, 그 카테고리 상의 미션은 다시 뽑히면 안되므로 그 카테고리를 모든 리스트에서 제거함 
        /// </summary>
        /// <param name="which_rank"></param>
        /// <param name="bronze"></param>
        /// <param name="silver"></param>
        /// <param name="gold"></param>
        private void Remove_Specific_Quest(int which_rank, ref List<int> bronze, ref List<int> silver, ref List<int> gold)
        {
            int mode = which_rank / 3;
            bool bronze_flag = false, silver_flag = false, gold_flag = false;
            switch (which_rank%3)
            {
                default:
                    bronze_flag = true;
                    break;
                
                case 1:
                    silver_flag = true;
                    break;
                
                case 2:
                    gold_flag = true;
                    break;
            }
            
            
            if (which_rank == 27)
            {
                for (int i = 0; i < gold.Count; i++)
                {
                    if (gold[i] == 27)
                    {
                        gold.RemoveAt(i);
                        return;
                    }
                }
            }

            else
            {

                if (!bronze_flag)
                {
                    for (int i = 0; i < bronze.Count; i++)
                    {
                        if (bronze[i] / 3 == mode)
                        {
                            bronze.RemoveAt(i);
                            break;
                        }
                    }
                }
                
                if (!silver_flag)
                {
                    for (int i = 0; i < silver.Count; i++)
                    {
                        if (silver[i] / 3 == mode)
                        {
                            silver.RemoveAt(i);
                            break;
                        }
                    }
                }
                
                if (!gold_flag)
                {
                    for (int i = 0; i < gold.Count; i++)
                    {
                        if (gold[i] / 3 == mode && gold[i] != 27)
                        {
                            gold.RemoveAt(i);
                            break;
                        }
                    }
                }

            }
        }
        

        #endregion
        
        #region Set
        /// <summary>
        /// 보상 획득하면 호출되는 함수 
        /// </summary>
        /// <param name="index"></param>
        public void Set_reward_get(int index)
        {
            data[index].get = true;
            var exp_data = new EXP_DAO(); // 경험치 관련 처리
            exp_data.Set_User_Exp(index);
            // 젬 관련 함수 처리 들어가야함 
            // 보상 처리 관련 코드 들어가야함 
            Set_Data(index);
            
        }
        
        private int Set_List(ref List<int> list)
        {
            var rand_value = UnityEngine.Random.Range(0, list.Count);
            var return_value = list[rand_value];
            if(list.Count!=0)
                list.RemoveAt(rand_value);
            return return_value;
        }

        private void Set_Specific_List(int value,ref List<int> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == value)
                {
                    list.RemoveAt(i);
                    return;
                }
            }
        }


        public bool Set_Revive(ref Set_Achi data, ref Queue<int> reward_list)
        {
            bool return_value = false;
            for (int i = 0; i < this.data.Length; i++)
            {
                if (this.data[i].item/3 !=6)
                    continue;

                if (data.Set_Revive(ref this.data[i]))
                {
                    reward_list.Enqueue(i);
                    return_value = true;
                }

                Set_Data(i); // 변경사항이 있는경우, 데이터 저장 
            }

            return return_value;
        }
        
        public bool Set_Item(ref Set_Achi data, ref Queue<int> reward_list)
        {
            bool return_value = false;
            for (int i = 0; i < this.data.Length; i++)
            {
                if (this.data[i].item/3 !=5)
                    continue;

                if (data.Set_Item_Use(ref this.data[i]))
                {
                    reward_list.Enqueue(i);
                    return_value = true;
                }

                Set_Data(i); // 변경사항이 있는경우, 데이터 저장 
            }

            return return_value;
        
            
        }
        /// <summary>
        /// 박스와 관련된 달성 유무를 체크해주는 함수. 달성하면 bool을 리턴하고, 메시지를 띄워준다.
        /// </summary>
        public bool Set_Ingame_Sqaure(ref Set_Achi data,  ref Queue<int> reward_list )
        {
            bool return_value = false;
            for (int i = 0; i < this.data.Length; i++)
            {
                if (this.data[i].item/3 !=1)
                    continue;
                
                if (data.Set_box(ref this.data[i]))
                {
                    reward_list.Enqueue(i);
                    return_value = true;
                }

                Set_Data(i); // 변경사항이 있는경우, 데이터 저장 
            }

            return return_value;
        }
        
        public bool Set_Ingame_Tri(ref Set_Achi data,  ref Queue<int> reward_list )
        {
            bool return_value = false;
            for (int i = 0; i < this.data.Length; i++)
            {
                if (this.data[i].item/3 !=2)
                    continue;
                
                if (data.Set_Tri(ref this.data[i]))
                {
                    reward_list.Enqueue(i);
                    return_value = true;
                }

                Set_Data(i); // 변경사항이 있는경우, 데이터 저장 
            }

            return return_value;
        }


        public bool Set_Combo(ref Set_Achi data, ref Queue<int> reward_list)
        {
            bool return_value = false;
            for (int i = 0; i < this.data.Length; i++)
            {
                if (this.data[i].item/3 !=3)
                    continue;
                
                if (data.Set_Combo(ref this.data[i]))
                {
                    reward_list.Enqueue(i);
                    return_value = true;
                }

                Set_Data(i); // 변경사항이 있는경우, 데이터 저장 
            }
            return return_value;
        }

        public bool Set_Score(ref Set_Achi data, int score, ref Queue<int> reward_list )
        {
            bool return_value = false;
            for (int i = 0; i < this.data.Length; i++)
            {
                if (this.data[i].item/3 !=4)
                    continue;

                if (data.Set_Score(ref this.data[i], score))
                {
                    reward_list.Enqueue(i);
                    return_value = true;
                }

                Set_Data(i); // 변경사항이 있는경우, 데이터 저장 
            }

            return return_value;
        }

        public bool Set_Pinata(ref Set_Achi data, ref Queue<int> reward_list)
        {
            bool return_value = false;
            for (int i = 0; i < this.data.Length; i++)
            {
                if (this.data[i].item/3 !=0)
                    continue;
                
                if (data.Set_Pinata(ref this.data[i]))
                {
                    reward_list.Enqueue(i);
                    return_value = true;
                }

                Set_Data(i); // 변경사항이 있는경우, 데이터 저장 
            }

            return return_value;
            
        }
        
        
        public bool Set_Quest(ref Set_Achi data, ref Queue<int> reward_list, ref int index)
        {
            bool return_value = false;
            for (int i = 0; i < this.data.Length; i++)
            {
                if(this.data[i].item != 27)
                    continue;
                
                if (data.Set_Quest(ref this.data[i]))
                {
                    index = i;
                    reward_list.Enqueue(i);
                    return_value = true;
                }
                Set_Data(i);
            }

            return return_value;
        }
        
        public bool Set_Collection(ref Set_Achi data, ref Queue<int> reward_list, int collection_num)
        {
            bool return_value = false;
            for (int i = 0; i < this.data.Length; i++)
            {
                if(this.data[i].item/3 != 7)
                    continue;
                
                if (data.Set_Collection(ref this.data[i], collection_num))
                {
                    reward_list.Enqueue(i);
                    return_value = true;
                }
                Set_Data(i);
            }

            return return_value;
        }

        public bool Set_Trade_Gem(ref Set_Achi data, ref Queue<int> reward_list, ref int index)
        {
            bool return_value = false;
            for (int i = 0; i < this.data.Length; i++)
            {
                if(this.data[i].item/3 != 8)
                    continue;
                
                if (data.Set_Trade_Gem(ref this.data[i]))
                {
                    reward_list.Enqueue(i);
                    return_value = true;
                    index = i;
                }
                Set_Data(i);
            }

            return return_value;
        }
        

        #endregion

    }
}
