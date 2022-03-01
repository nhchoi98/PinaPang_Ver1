using Block;
using UnityEngine;

namespace Ingame_Data
{
    // 필드에 있는 박스 or 플러스볼의 위치를 저장하는 형식 
    public class BoxInfoVO
    {
        public blocktype type; // 박스의 타입을 저장함
        public int hp; // 박스의 hp를 저장함 
        public bool isCandle; // 캔들박스인지 아닌지의 유무를 저장함
        public Vector2 pos; // 박스의 위치를 저장함. 
    }
}

