using UnityEngine;


namespace Badge
{
    public class Badge_Card_Set
    {
        private Calc_Badge_Index index = new Calc_Badge_Index();

        public Sprite Card_img(int name)
        {
            int card_num = index.Get_Badge_Num(name);
            string path = "Lobby/Badge/Badge_Img/" + card_num.ToString();
            Sprite img = Resources.Load<Sprite>(path);
            return img;
        }

        public Sprite back_panel(int rank)
        {
            string path = "Lobby/Badge/Badge_Img/Panel_" + rank.ToString();
            Sprite img = Resources.Load<Sprite>(path);
            return img;

        }

    }
}
