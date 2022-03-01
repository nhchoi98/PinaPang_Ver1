using System;
using System.Collections;
using System.Collections.Generic;
using Avatar;
using Progetile;
using Skin;
using UnityEngine;

namespace Shop
{
    public class Shop_Avatar_Package
    {
        private BallPurDAO ballData;
        private AvatarDAO avatarData;
        private LineDAO lineData;
        private Shop_AlarmSetter _alarmSetter;
        private Panel_Control _panelControl;
        
        public Shop_Avatar_Package()
        {
            _alarmSetter = new Shop_AlarmSetter();
        }

        /// <summary>
        /// 패키지를 획득하면 호출되는 함수. 인덱스를 통해 어떤 패키지를 획득했는지 구분한다.
        /// </summary>
        /// <param name="index"></param>
        public void Get_Package(int index)
        {
            Package_DataDAO packageDataDao = new Package_DataDAO(index);
            packageDataDao.Set_Data();
            _panelControl = GameObject.FindWithTag("Panel_Control").GetComponent<Panel_Control>();
            // 1. 패키지 구매하면 해당 인덱스 상품 사라지게 하기.
            // 2. 슈퍼 세일 상품 -> 슈퍼세일 다른 상품으로 바꾸어주고, 없을 경우에는 더 이상 상품을 띄워주지 않는 로직 추가하기.
            // 3. 두 개 추가가 필요함. 01.12에 꼭 진행할 것.
            switch (index)
            {
                // 우주비행사 패키지 
                case 3:
                    avatarData = new AvatarDAO(2000);
                    ballData = new BallPurDAO(4002);
                    lineData = new LineDAO(14);
                    lineData.Set_Line_Purchased(14);
                    _alarmSetter.Set_Alarm(0);
                    break;
                
                // 곰 패키지
                case 4:
                    avatarData = new AvatarDAO(2002);
                    ballData = new BallPurDAO(4001);
                    lineData = new LineDAO(13);
                    lineData.Set_Line_Purchased(13);
                    _alarmSetter.Set_Alarm(2);
                    break;
                
                // 파티광 패키지
                case 2:
                    avatarData = new AvatarDAO(1006);
                    ballData = new BallPurDAO(4000);
                    _alarmSetter.Set_Alarm(1);
                    break;
                
                // 과학자 패키지 
                case 5:
                    avatarData = new AvatarDAO(13);
                    ballData = new BallPurDAO(4003);
                    _alarmSetter.Set_Alarm(3);
                    break;
            }

            try
            {
                avatarData.Set_Locked_DATA(); // 아바타 잠금 해제 
                ballData.Purchase(); // 볼 잠금 해제 
                _panelControl.Determine_Avatar_On();
            }
            
            catch(Exception ex)
            {
                Debug.Log(ex);
                throw;
            }
        }
    }
}
