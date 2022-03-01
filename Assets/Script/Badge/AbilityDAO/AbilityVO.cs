using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Badge
{
    public class AbilityVO
    {
        public int duration; // 아이템 지속시간 관련 변수 
        public int bonus; // 보너스 스코어 관련 변수 
        public int candle; // 캔들 관련 변수 
        public int speed; // ball speed 관련 변수 
        public bool life; // 부활을 자동으로 시켜주는지에 관한 변수.
    }
}
