using LitJson;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


namespace Challenge
{
    public class Challenge_Descrip
    {
        private string title; // 제목과 설명을 여기에 담음
        private string desc;
        private int target_num; // 보상과 젬을 여기에 담음 


        public Challenge_Descrip(int index)
        {
            Read_Title(index);
            Read_Desc(index);
            Read_Target(index);
        }

        // 제목
        // 설명
        // 보상 - 젬
        // 보상 - 경험치 
        public void Get_Title(ref Text title) => title.text = this.title;
        public void Get_Desc(ref Text desc) => desc.text = this.desc;
        public void Get_target(ref int target) => target = this.target_num;

        #region IO

        public void Read_Title(int index)
        {
            string DATA_PATH = "Challenge/Desc/title/" + index.ToString();
            DescVO DATA = null;
            if (Resources.Load(DATA_PATH))
            {
                TextAsset DATA_ASSET = Resources.Load(DATA_PATH) as TextAsset;
                var _DATA = DATA_ASSET.ToString();
                DATA = JsonMapper.ToObject<DescVO>(_DATA);
                this.title = DATA.title;
            }

            else
                title = "exception";
        }

        public void Read_Desc(int index)
        {
            string DATA_PATH = "Challenge/Desc/desc/" + index.ToString();
            DescVO DATA = null;
            if (Resources.Load(DATA_PATH))
            {
                TextAsset DATA_ASSET = Resources.Load(DATA_PATH) as TextAsset;
                var _DATA = DATA_ASSET.ToString();
                DATA = JsonMapper.ToObject<DescVO>(_DATA);
                this.desc = DATA.title;
            }

            else
                desc = "exception";

        }
        
        
        public void Read_Target(int index)
        {
            var target_index = index % 3;
            if (index > 15)
            {
                target_index = (3 + (index % 3));
                if (index == 27)
                    target_index = 5;
            }

            string DATA_PATH = "Challenge/Desc/exp/" + target_index.ToString();
            RewardVO DATA = null;
            if (Resources.Load(DATA_PATH))
            {
                TextAsset DATA_ASSET = Resources.Load(DATA_PATH) as TextAsset;
                var _DATA = DATA_ASSET.ToString();
                DATA = JsonMapper.ToObject<RewardVO>(_DATA);
                this.target_num = DATA.target;
            }

            else
                target_num = -1;
        }
        
        #endregion
    }
}
