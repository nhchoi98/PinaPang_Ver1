using UnityEngine;

/// <summary>
/// 양초 비행을 위한 정보를 리턴해주는 클래스 
/// </summary>

namespace Charater
{
    public class Flight_Info
    {
        private int index;

        public Flight_Info(int index)
        {
            this.index = index;
        }

        /// <summary>
        /// 비행체의 이미지를 리턴해주는 함수 
        /// </summary>
        /// <returns></returns>
        public Sprite charater_img()
        {
            Sprite img;
            string path = "Ingame/UI/Upper_bar/Charater_info/";
            img = Resources.Load<Sprite>(path + index.ToString());
            return img;
        }

        public Vector2 flight_Pos()
        {
            Vector2 target;
            switch (index)
            {
                default:
                    target = new Vector2(-479.975f, -903.4f);
                    break;

                case 1:
                    target = new Vector2(-437.891f, -903.4f);
                    break;

                case 2:
                    target = new Vector2(-397.76f, -903.9f);
                    break;

                case 3:
                    target = new Vector2(-347.5f, -908.7f);
                    break;

                case 4:
                    target = new Vector2(-299.732f, -908.9f);
                    break;

                case 5:
                    target = new Vector2(-252.6f, -911.4f);
                    break;

            }

            return target;
        }

    }
}
