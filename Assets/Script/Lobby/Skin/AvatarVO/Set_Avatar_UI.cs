
using UnityEngine;



namespace Avatar
{
    /// <summary>
    /// 인게임과 로비에서 캐릭터의 UI를 불러오기 위한 정적 클래스 
    /// </summary>
    public static class Set_Avatar_UI 
    {
        /// <summary>
        /// 볼 이미지를 업로드하는 class 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>

        #region Avatar
        public static RuntimeAnimatorController Set_Charater_GameObject(int index)
        {
            RuntimeAnimatorController avatar; ;
            string path = "Charater/" + index + "/char_ani";
            avatar = Resources.Load<RuntimeAnimatorController>(path);
            return avatar;
        }

        public static Sprite Set_Avatar_Img(int index)
        {
            int avatar_num =  Calc_Index.Get_Avatar_Num(index);
            string path = "Lobby/UI/Skin/Avatar_Preview/" + (avatar_num).ToString();
            Sprite Img;
            Img = Resources.Load<Sprite>(path);
            return Img;
        }

        #endregion

        #region Ball
        public static Sprite Set_Ball_Img(int index)
        {
            int ball_num = Calc_Index.Get_Ball_Num(index);
            string path = "Ball/" + ball_num.ToString();
            Sprite Ball_Img;
            Ball_Img = Resources.Load<Sprite>(path);
            return Ball_Img;
        }

        #endregion

        public static Sprite Set_LineItem_Img(int index)
        {
            string path = "Line/" + index.ToString();
            Sprite Img;
            Img = Resources.Load<Sprite>(path);
            return Img;
        }

        public static int target_card(int index)
        {
            return 30;

        }
        
    }
}
