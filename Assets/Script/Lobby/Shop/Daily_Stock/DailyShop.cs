using System;
using System.Collections;
using System.Collections.Generic;
using Alarm;
using Challenge;
using Daily_Reward;
using Ingame;
using shop;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class DailyShop : MonoBehaviour
    {
        private int index;
        [SerializeField] private Gem_Goods _gemGoods;
        [SerializeField] private Transform dailyBtn;
        public DailyShopDAO countData;

        [Header("Level_Data")] [SerializeField] private Level_UI _levelUI;

        [Header("Timer")] public Text Timer_hour, Timer_minute, Timer_second;
        [Header("AlarmMediation")] private IAlarmMediator _mediator;
        private bool first_set = true;
        
        
        private void OnEnable()
        {
            countData = new DailyShopDAO();
            _mediator = GameObject.FindWithTag("alarmcontrol").GetComponent<IAlarmMediator>();
            int level;
            if (first_set)
            {
                EXP_DAO leveldata = new EXP_DAO();
                level = leveldata.Get_User_Level();
            }

            else
            {
                level = _levelUI.Get_Level();
            }

            int count = countData.Get_Count(0);
            if (count == 0)
            {
                dailyBtn.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "<color=red> 0/3 </color>";
                dailyBtn.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
            }

            else dailyBtn.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = count.ToString() + "/3";

            count = countData.Get_Count(1);
            if (count == 0)
            {
                dailyBtn.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = "<color=red> 0/2 </color>";
                dailyBtn.GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }

            else
            {
                dailyBtn.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = count.ToString() + "/2";
                if (level < 16)
                {
                    dailyBtn.GetChild(1).gameObject.GetComponent<Button>().interactable = false;
                    dailyBtn.GetChild(1).GetChild(3).gameObject.SetActive(true);
                }
                
                else
                {
                    dailyBtn.GetChild(1).gameObject.GetComponent<Button>().interactable = true;
                    dailyBtn.GetChild(1).GetChild(3).gameObject.SetActive(false);
                }
            }

        

            count = countData.Get_Count(2);
            if (count == 0)
            {
                dailyBtn.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = "<color=red> 0/1 </color>";
                dailyBtn.GetChild(2).gameObject.GetComponent<Button>().interactable = false;
            }

            else
            {
                dailyBtn.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = count.ToString() + "/1";
                if (level < 31)
                {
                    dailyBtn.GetChild(2).gameObject.GetComponent<Button>().interactable = false;
                    dailyBtn.GetChild(2).GetChild(3).gameObject.SetActive(true);
                }

                else
                {
                    dailyBtn.GetChild(2).gameObject.GetComponent<Button>().interactable = true;
                    dailyBtn.GetChild(2).GetChild(3).gameObject.SetActive(false);
                }
            }

            StartCoroutine(Timer_set());
            // UI 반영하기 
        }
        

        public void Event_Occur()
        {

            _gemGoods.Get_Merchandise_Gem(index);
            // # Step 2. 해당 Count 데이터 조정해주기 
            countData.Set_Count(index);
            _mediator.Event_Receieve(Event_Alarm.SHOP_ALARM_OFF);
            switch (index)
            {
                case 0:
                    if (countData.Get_Count(index) == 0)
                    {
                        dailyBtn.GetChild(index).gameObject.GetComponent<Button>().interactable = false;
                        dailyBtn.GetChild(index).GetChild(0).gameObject.GetComponent<Text>().text = "<color=#F11414>" + countData.Get_Count(index).ToString()+ "/3</color>";
                    }
                    else
                        dailyBtn.GetChild(index).GetChild(0).gameObject.GetComponent<Text>().text = countData.Get_Count(index).ToString()+ "/3";
                    break;
                
                case 1:
                    if (countData.Get_Count(index) == 0)
                    {
                        dailyBtn.GetChild(index).gameObject.GetComponent<Button>().interactable = false;
                        dailyBtn.GetChild(index).GetChild(0).gameObject.GetComponent<Text>().text = "<color=#F11414>" + countData.Get_Count(index).ToString()+ "/2</color>";
                    }
                    else
                        dailyBtn.GetChild(index).GetChild(0).gameObject.GetComponent<Text>().text = countData.Get_Count(index).ToString()+ "/2";
                    break;
                
                case 2:
                    if (countData.Get_Count(index) == 0)
                    {
                        dailyBtn.GetChild(index).gameObject.GetComponent<Button>().interactable = false;
                        dailyBtn.GetChild(index).GetChild(0).gameObject.GetComponent<Text>().text = "<color=#F11414>" + countData.Get_Count(index).ToString()+ "/1</color>";
                    }
                    else 
                        dailyBtn.GetChild(index).GetChild(0).gameObject.GetComponent<Text>().text = countData.Get_Count(index).ToString()+ "/1";
                    break;
            }
            
            // # Step 3. UI에 반영하기 
        }

        public void Set_Index(int index)
        {
            this.index = index;
        }
        
        IEnumerator Timer_set()
        {
            DateTime _dateTime_now = DateTime.UtcNow;
            TimeSpan delte = new TimeSpan(_dateTime_now.Hour, _dateTime_now.Minute, _dateTime_now.Second);
            TimeSpan Plus = new TimeSpan(1,0,0,0);
            DateTime target = _dateTime_now - delte+Plus;
            while (true)
            {
                DateTime now = DateTime.UtcNow;
                TimeSpan delta_target = target - now;
                Timer_hour.text = delta_target.ToString(@"hh");
                Timer_minute.text = delta_target.ToString(@"mm");
                Timer_second.text = delta_target.ToString(@"ss");
                if (delta_target < TimeSpan.FromSeconds(1))
                    break;
                
                yield return new WaitForSeconds(1.0f);
            }
            
            ChallengeDAO data = new ChallengeDAO();
            data.Init_data(true);
            countData = new DailyShopDAO();
            int count = countData.Get_Count(0);
            if (count == 0)
            {
                dailyBtn.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "<color=red> 0/3 </color>";
                dailyBtn.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
            }

            else dailyBtn.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = count.ToString() + "/3";

            count = countData.Get_Count(1);
            if (count == 0)
            {
                dailyBtn.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = "<color=red> 0/2 </color>";
                dailyBtn.GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            }

            else
            {
                dailyBtn.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = count.ToString() + "/2";
                if(_levelUI.Get_Level()<16)
                    dailyBtn.GetChild(1).gameObject.GetComponent<Button>().interactable = false;
                
                else
                    dailyBtn.GetChild(1).gameObject.GetComponent<Button>().interactable = true;
            }

        

            count = countData.Get_Count(2);
            if (count == 0)
            {
                dailyBtn.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = "<color=red> 0/1 </color>";
                dailyBtn.GetChild(2).gameObject.GetComponent<Button>().interactable = false;
            }

            else
            {
                dailyBtn.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = count.ToString() + "/1";
                if(_levelUI.Get_Level()<31)
                    dailyBtn.GetChild(2).gameObject.GetComponent<Button>().interactable = false;
                
                else
                    dailyBtn.GetChild(2).gameObject.GetComponent<Button>().interactable = true;
            }

            StartCoroutine(Timer_set());
        }
    }
}
