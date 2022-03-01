
using Firebase.Analytics;
using Ingame;
using Skin;
using UnityEngine;
using UnityEngine.UI;

namespace Ad
{
    public class LineAd : MonoBehaviour, IComponent
    {
        [SerializeField] private Skin_Line line;
        [SerializeField] private Transform lineTR;
        public Text count_text;
        private int index;
        private IMediator _mediator;
        public AudioSource line_get;

        private LineCountDAO[] count_data;
        private void Start()
        {
            count_data = new LineCountDAO[15];
            int count;
            for (int i = 0; i < count_data.Length; i++)
            {
                index = i;
                count_data[i] = new LineCountDAO(i);
                count = count_data[i].Get_Count();
                lineTR.GetChild(i).GetChild(7).GetChild(2).gameObject.GetComponent<Text>().text
                    =  "<color=red>" + count.ToString() +"/"+ Target_num().ToString() + "</color>";
            }
        }

        public void Set_Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        private void UserEarnedReward()
        {
            count_data[index].Set_Count();
            int count = count_data[index].Get_Count();
            FirebaseAnalytics.LogEvent("Line_Buy_Ad");
            if (count == Target_num())
            {
                LineDAO data = new LineDAO(index);
                data.Set_Line_Purchased(index); // # 1. 데이터 구매 처리
                Skin_Log.Buy_Line(index);
                // # 2. 버튼 꺼주기 
                line.Get_Line(index);
                // # 3. 그리드 처리 
                lineTR.GetChild(index).GetChild(7).gameObject.SetActive(false);
                lineTR.GetChild(index).GetChild(6).gameObject.SetActive(true);
                lineTR.GetChild(index).GetChild(2).gameObject.GetComponent<Image>().material = null;
                line_get.Play();
            }
            
            else
            {
                count_text.text = count.ToString() + "/" + Target_num().ToString();
            }
            
            lineTR.GetChild(index).GetChild(7).GetChild(2).gameObject.GetComponent<Text>().text
                =  "<color=red>" + count.ToString() +"/"+ Target_num().ToString() + "</color>";
        }
        
        
        public void Event_Occur(Event_num eventNum)
        {
            UserEarnedReward();
        }
        
        public void UserChoseToWatchAd()
        {
            _mediator.Event_Receive(Event_num.LINE_PURCHASE);
        }

        private int Target_num()
        {
            switch (index)
            {
                default:
                    return 1;
                
                case 4:
                case 5:
                case 6:
                case 7:
                    return 3;
            
                case 8:
                case 9:
                case 10:
                case 11:
                    return 4;
                
                case 12:
                    return 5;
            }
        }

        public void Set_Index(int index)
        {
            int count = count_data[index].Get_Count();
            this.index = index;
            count_text.text = count.ToString() + "/" + Target_num().ToString();
        }
        
    }
    
}
