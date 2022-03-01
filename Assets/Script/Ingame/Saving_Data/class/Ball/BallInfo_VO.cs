using UnityEngine;

namespace Ingame_Data
{
    public class BallInfo_VO
    {
        public int ballNum; // 볼의 개수를 저장함 
        public Vector2 ballPos; // 볼의 발사 포지션 
        public Vector2 charPos; // 캐릭터 포지션
        public bool is_Fliped;  // 캐릭터가 어딜 보고 있는지를 저장함.
    }
}
