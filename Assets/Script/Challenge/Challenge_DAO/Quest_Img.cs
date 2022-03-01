
using UnityEngine;

namespace Challenge
{
    /// <summary>
    /// 퀘스트별 이미지 Sprite를 리턴해주는 함수 
    /// </summary>
    public static class Quest_Img 
    {
        /// <summary>
        /// 뒷 판을 리턴해주는 함수 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Sprite Get_Quest_Panel(int index)
        {
            Sprite img;
            string path;
            if (index != 30)
                path = "Challenge/Quest_img/Panel/" + (index % 3).ToString();

            else
                path = "Challenge/Quest_img/Panel/2";
                
            
            img = Resources.Load<Sprite>(path);
            return img;
        }

        /// <summary>
        /// 앞의 무늬를 리턴해주는 함수 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Sprite Get_Quest_main(int index)
        {
            Sprite img;
            string path = "Challenge/Quest_img/Main/" + (index/3).ToString();
            img = Resources.Load<Sprite>(path);
            return img;
        }
    }
}
