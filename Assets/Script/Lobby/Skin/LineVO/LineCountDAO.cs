using System.IO;
using LitJson;
using UnityEngine;

namespace  Skin
{
    public class LineCountDAO
    {
        private LineCountVO data;
        private int index;

        public LineCountDAO(int index)
        {
            this.index = index;
            Read_Data();
        }

        private void Read_Data()
        {
            var DATA_PATH = Application.persistentDataPath + "/Line/count/" + index.ToString() + ".json"; ;
            LineCountVO DATA = null;
            if (File.Exists(DATA_PATH))
            {
                var json_string = File.ReadAllText(DATA_PATH);
                DATA = JsonMapper.ToObject<LineCountVO>(json_string);
            }

            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Line/count");
                DATA  = new LineCountVO();
                DATA.count = 0;
                var DATA_STR = JsonUtility.ToJson(DATA);
                File.WriteAllText(DATA_PATH,DATA_STR);   
            }
            
            this.data = DATA;
            
        }


        private void Set_Data()
        {
            var DATA = JsonMapper.ToJson(this.data);
            var DATA_PATH = Application.persistentDataPath + "/Line/count/" + index.ToString() + ".json";
            File.WriteAllText(DATA_PATH,DATA);
        }


        public void Set_Count()
        {
            data.count += 1;
            Set_Data();
        }

        public int Get_Count()
        {
            return data.count;
        }



    }

}
