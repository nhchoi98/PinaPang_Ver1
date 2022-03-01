using System.IO;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

namespace Skin
{
    /// <summary>
    /// 장착한 스킨 데이터를 가지고 있는 클래스. 아바타의 Equip 클래스 역할을 가지고 있으며, 경로 또한 가지고 있는 클래스이다. 
    /// </summary>
    public class SkinDAO 
    {
        private SkinVO skinData; // 값을 보여줄 스킨 데이터 vo를 저장하는 변수
        private string PATH = "Skin/Background_Ingame/";
        public SkinDAO()
        {
            Read_Data();

        }
        
        
        /// <summary>
        /// 테마에 맞는 배경을 리턴해주는 함수 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public Sprite Get_Background(int num)
        {
            Sprite img;
            img = Resources.Load<Sprite>(PATH+ skinData.themeName+"/"+num);
            return img;
        }
        

        public int Themename()
        {
            return skinData.themeName;
        }


        public void Set_Theme_Data()
        {
            skinData.themeName = UnityEngine.Random.Range(0,2);
        }

        public int Get_Theme_Data()
        {
            return skinData.themeName;
        }
        
        
        #region  IO

        private void Read_Data()
        {
            string DATA_PATH = Application.persistentDataPath + "/Skin/DATA.json";
            string PRE_DATA_PATH = "Skin/DATA/DATA";
            SkinVO DATA = null;
            if (File.Exists(DATA_PATH))
            {
                string json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<SkinVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Skin");
                TextAsset DATA_ASSET = Resources.Load(PRE_DATA_PATH) as TextAsset;
                string _DATA = DATA_ASSET.ToString();
                File.WriteAllText(DATA_PATH, _DATA);
                DATA = JsonMapper.ToObject<SkinVO>(_DATA);
            }

            skinData = DATA;
        }

        #endregion
        //skinData를 읽어오는 class 
       
    }

}
