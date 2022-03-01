using UnityEngine.UI;
using UnityEngine;

namespace Challenge
{
    public class Medal_Img
    {
        public Sprite Img(int rank)
        {
            if (rank > 8) // 버그 방지 임시 코드 
                rank = 8;
            
            Sprite img = Resources.Load<Sprite>("Lobby/Challenge/Medal/"+ rank.ToString());
            return img;
        }

    }
}
