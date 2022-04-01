using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    /// <summary>
    /// 필드형 아이템에 대해서, 데이터를 저장하고 로드하는데 필요한 정보를 가져다줌  
    /// </summary>
    public interface IItem_Data
    {
        public void Set_Row(int value); // 행 값을 초기화해줌 
        public int Get_Row();
        public Vector2 Get_Pos();
        public void Set_Load(); // 이어하기에서 호출시 불러와지는 함수  
    }
}
