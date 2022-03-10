
using Ad;
using Avatar;
using Badge;
using Timer;
using UnityEngine;

namespace Shop
{
    public class Panel_Control : MonoBehaviour
    {
        [SerializeField] private Transform tr;
        [SerializeField] private Transform superSale_tr;
        [SerializeField] private GameObject emptyPanel;
        [SerializeField] private Transform Noads_Btn;
        [SerializeField] private CharaterPack_Timer timer;
        [SerializeField] private Transform package_panel;
        [SerializeField] private GameObject starter_panel;

        [Header("Free_Item_Package")] 
        [SerializeField] private Challenge_Item _challengeItem;
        [SerializeField] private Badge_Data _badgeData;
        private bool firstSet;
        void Start()
        {
            Determine_StarterOn();
            Determine_Avatar_On();
        }

        public void Determine_StarterOn()
        {
            Package_DataDAO starterData = new Package_DataDAO(0);
            Determine_StarterDAO _starter_Time = new Determine_StarterDAO();
            if (starterData.Get_Data() || !_starter_Time.Get_Purchasable()) // 결제 한 경우
                starter_panel.SetActive(false);
            
        }
        
        private void Determine_Beginner_On(bool is_avatar_none = false)
        {
            int available_count = 0;

            // 구버전 Noads 구매시
            if (Noads_instance.Get_Is_Noads())
            {
                available_count+=2;
                tr.GetChild(2).gameObject.SetActive(false);
                tr.GetChild(3).gameObject.SetActive(false);
            }

            // 신버전 Noads 구매시 
            if (Noads_instance.Get_Is_Noads_New())
            {
                ++available_count;
                tr.GetChild(2).gameObject.SetActive(false);
            }

            if (Noads_instance.Get_ItemAds())
            {
                ++available_count;
                tr.GetChild(3).gameObject.SetActive(false);
            }

            if (available_count == 2)
            {
                tr.GetChild(1).gameObject.SetActive(false); // 구분선을 꺼줌
                if(is_avatar_none)
                    emptyPanel.SetActive(true);
            }
        }
        
        public void Determine_Avatar_On()
        {
            int available_Count = 0;
            // 1. 과학자 패키지 띄울것인지 결정 
            IsLockedDAO science = new IsLockedDAO(13);
            IsLockedDAO party = new IsLockedDAO(1006);
            IsLockedDAO astro = new IsLockedDAO(2000);
            IsLockedDAO teddy = new IsLockedDAO(2002);
            /*
            tr.GetChild(5).gameObject.SetActive(true);
            tr.GetChild(6).gameObject.SetActive(true);
            tr.GetChild(7).gameObject.SetActive(true);
            tr.GetChild(8).gameObject.SetActive(true);
            */
            
            if (!science.Get_Locked())
            {
                tr.GetChild(5).gameObject.SetActive(false);
                ++available_Count;
            }

            if (!party.Get_Locked())
            {
                tr.GetChild(6).gameObject.SetActive(false);
                ++available_Count;
            }
            
            if (!astro.Get_Locked())
            {
                tr.GetChild(7).gameObject.SetActive(false);
                ++available_Count;
            }
            
            if (!teddy.Get_Locked())
            {
                tr.GetChild(8).gameObject.SetActive(false);
                ++available_Count;
            }

            if (firstSet) // firstSet이 아닌 경우. 초기화가 아닌경우. 
            {
                if (science.Get_Locked())
                {
                    tr.GetChild(5).gameObject.SetActive(true);
                }

                if (party.Get_Locked())
                {
                    tr.GetChild(6).gameObject.SetActive(true);
                }
            
                if (astro.Get_Locked())
                {
                    tr.GetChild(7).gameObject.SetActive(true);
                }
            
                if (teddy.Get_Locked())
                {
                    tr.GetChild(8).gameObject.SetActive(true);
                }

            }

            else
            {
                firstSet = true;
            }


            // 4개 다 구매한 경우 
            if (available_Count == 4)
            {
                tr.GetChild(4).gameObject.SetActive(false); // 띠를 없애줌 
                tr.GetChild(0).gameObject.SetActive(false); // 슈퍼세일도 꺼줌 
                Determine_Beginner_On(true);
            }

            else
            {
                Determine_Beginner_On(false);
            }
        }

        public void Purchase_Noads()
        {
            Noads_Btn.GetChild(0).gameObject.SetActive(false);
            Noads_Btn.GetChild(1).gameObject.SetActive(true);
            Purchase_FreeItem();
        }

        public void Purchase_FreeItem()
        {
            _challengeItem.Purchase_Free_Item_Package(); // # 1. 아이템 사용 퀘스트와 관련된 내용이 있다면, 클리어 처리.
            _badgeData.Purchase_FreeItem_Package();
            // # 2. 뱃지 획득 처리
        }
        public void Init_SuperSale(int index)
        {
            superSale_tr.GetChild(0).gameObject.SetActive(false);
            superSale_tr.GetChild(1).gameObject.SetActive(false);
            superSale_tr.GetChild(2).gameObject.SetActive(false);
            superSale_tr.GetChild(3).gameObject.SetActive(false);
            switch (index)
            {
                case 0:
                    superSale_tr.GetChild(0).gameObject.SetActive(true);
                    break;
                
                case 1:
                    superSale_tr.GetChild(1).gameObject.SetActive(true);
                    break;
                
                case 2:
                    superSale_tr.GetChild(2).gameObject.SetActive(true);
                    break;
                
                case 3:
                    superSale_tr.GetChild(3).gameObject.SetActive(true);
                    break;
                
            }
        }
        
        /// <summary>
        /// 아바타를 구매하면, 슈퍼세일을 갈아 엎을 준비. 
        /// </summary>
        public void Set_SuperSale()
        {
            Determine_Avatar_On();
            superSale_tr.GetChild(0).gameObject.SetActive(false);
            superSale_tr.GetChild(1).gameObject.SetActive(false);
            superSale_tr.GetChild(2).gameObject.SetActive(false);
            superSale_tr.GetChild(3).gameObject.SetActive(false);
            package_panel.GetChild(0).gameObject.SetActive(false);
            package_panel.GetChild(1).gameObject.SetActive(false);
            package_panel.GetChild(2).gameObject.SetActive(false);
            package_panel.GetChild(3).gameObject.SetActive(false);
            timer.Reset_SuperSale();

        }
    }
}
