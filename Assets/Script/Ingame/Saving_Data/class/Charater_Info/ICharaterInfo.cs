using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Charater
{
    /// <summary>
    /// 양초박스 등장 스테이지와 등장 유무를 저장하는 오브젝트..
    /// 1. 여태까지 어떤게 등장했는지
    /// 2. 이제 뭐가 나와야 하는지
    /// 3. 각 양초박스는 언제 등장하는지
    /// 를 저장한다.. 
    /// </summary>
    public interface ICharaterInfo
    {
        public void Set_Load(); // 이어하기에서 정보 로드시 호출되는 함수  
        public void Get_Info(); // 위의 정보를 불러오는 함수 
        
    }
}
