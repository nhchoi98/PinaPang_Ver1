
using UnityEngine;
using System.IO;
using LitJson;

namespace Theme
{
    /// <summary>
    /// Theme에 대한 정보를 수정하기 위한 DAO
    /// 0: Home
    /// 1: Amuse
    /// </summary>
    public class ThemepurDAO 
    {
        private ThemepurVO theme_OBJ;
        private int which_Theme;
        // 어떤 Theme의 정보를 가져올지 클래스 인스턴스화시 매개변수를 통해 가져옴 
        public ThemepurDAO(int which_theme)
        {
            string DATA_PATH = Application.persistentDataPath + "/Info/Theme/"+which_theme.ToString() + ".json";
            string PRE_DATA_PATH = "Info/Theme/" + which_theme.ToString();
            ThemepurVO DATA = null;
            if (File.Exists(DATA_PATH))
            {
                string json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<ThemepurVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Info/Theme");
                TextAsset DATA_ASSET = Resources.Load(PRE_DATA_PATH) as TextAsset;
                string _DATA = DATA_ASSET.ToString();
                File.WriteAllText(DATA_PATH, _DATA);
                DATA = JsonMapper.ToObject<ThemepurVO>(_DATA);
            }

            this.which_Theme = which_theme;
            theme_OBJ = DATA;
        }

        /// <summary>
        /// 결제 여부를 저장해줌 . 결재 시 클릭하면됨 
        /// </summary>
        public void Set_Purchased()
        {
            theme_OBJ.Is_Purchased = true;
            Save_Data();
            return;
        }

        public bool Get_Is_Purchased()
        {
            return theme_OBJ.Is_Purchased;
        }
        


        private void Save_Data()
        {
            string DATA = JsonMapper.ToJson(theme_OBJ);
            string DATA_PATH = Application.persistentDataPath + "/Info/Theme/" + which_Theme.ToString() + ".json";
            File.WriteAllText(DATA_PATH, DATA);

        }

    }
}
