using System.IO;
using UnityEngine;
using LitJson;
/// <summary>
/// 라인 장착 유무를 접근할 수 있게 만드는 CLASS 
/// </summary>
namespace Skin
{
    // 1. 장착 데이터 불러오기 
    // 2. 아이템들의 결제유무 정보 불러오기 
    // 3. 아이템들의 결제 유무, 장착 유무 정보 변경하기 
    // ** NOTE: 장착 데이터를 읽는 함수는 따로 존재함 ** 
    public class LineDAO
    {
        private LineVO DATA_SINGLE;
        private LineVO[] DATA;
        private LineEquipVO equip_data;
        private bool is_single = true;
        public LineDAO(bool skin)
        {
            if (skin)
            {
                is_single = false;
                DATA = new LineVO[15];
                for (int i = 0; i < 15; i++) Read_Data(i);
            }
            Read_equip_Data();    
        }
        
        public LineDAO(int index)
        {
            Read_Data(index);
        }

        public LineDAO()
        {
            Read_equip_Data();
        }
            

        #region IO_PURCHASE 
        private void Read_Data(int index)
        {
            var DATA_PATH = Application.persistentDataPath + "/Line/"+ index.ToString() + ".json";
            LineVO DATA = null;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<LineVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Line");
                DATA  = new LineVO();
                if (index != 0)
                    DATA.is_purchased = false;

                else
                    DATA.is_purchased = true;
                var DATA_STR = JsonUtility.ToJson(DATA);
                File.WriteAllText(DATA_PATH,DATA_STR);   
            }

            if (!is_single)
                this.DATA[index] = DATA;

            else
                DATA_SINGLE = DATA;
        }
        
      


        private void Save_Data(int index)
        {
            string DATA ; 
            if(!is_single)
                DATA= JsonMapper.ToJson(this.DATA[index]);
            
            else
                DATA = JsonMapper.ToJson(this.DATA_SINGLE);
            
            var DATA_PATH = Application.persistentDataPath + "/Line/"+ index.ToString() + ".json";
            File.WriteAllText(DATA_PATH,DATA);
            
        }
        #endregion
        
        #region IO_EQUIP
        private void Read_equip_Data()
        {
            var DATA_PATH = Application.persistentDataPath + "/Line/equip/equip.json";
            var PRE_DATA_PATH = "Ingame/Line/equip/equip" ;
            LineEquipVO DATA;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<LineEquipVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Line/equip");
                TextAsset DATA_ASSET = Resources.Load(PRE_DATA_PATH) as TextAsset;
                var _DATA = DATA_ASSET.ToString();
                File.WriteAllText(DATA_PATH, _DATA);
                DATA = JsonMapper.ToObject<LineEquipVO>(_DATA);
            }

            this.equip_data = DATA;
        }

        private void Save_equip_Data()
        {
            var DATA = JsonMapper.ToJson(equip_data);
            var DATA_PATH = Application.persistentDataPath + "/Line/equip/equip.json";
            File.WriteAllText(DATA_PATH,DATA);
            
        }
        #endregion

        /// <summary>
        /// 어떤 라인을 현재 장착하고 있는지를 리턴해주는 변수 
        /// </summary>
        /// <returns></returns>
        public int Get_Equip_Data()
        {
            return equip_data.which_index;
        }

        public Material Get_line_mat()
        {
            Material material = Determine_Line.line_material(Get_Equip_Data());
            return material;
        }

        /// <summary>
        /// 라인의 폭을 리턴해주는 함수 
        /// </summary>
        /// <returns></returns>
        public int Get_width()
        {
            int value = Determine_Line.line_width(Get_Equip_Data());
            return value;
        }

        /// <summary>
        /// 라인을 구입했는지 여부를 리턴해주는 함수 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Get_Is_Locked(int index)
        {
            return DATA[index].is_purchased;
        }

        /// <summary>
        /// 라인 구매시 데이터 반영을 위해 호출되는 함수 
        /// </summary>
        /// <param name="index"></param>
        public void Set_Line_Purchased(int index)
        {
            DATA_SINGLE.is_purchased = true;
            Save_Data(index);
        }
        
        /// <summary>
        /// 장착한 라인을 바꾸어 주는 함수 
        /// </summary>
        /// <param name="index"></param>
        public void Set_Equip_Data(int index)
        {
            equip_data.which_index = index;
            Save_equip_Data();
        }
    }
}
