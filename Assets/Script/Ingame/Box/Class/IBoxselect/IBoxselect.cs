using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Box
{
    /// <summary>
    /// 박스들을 선택하는 스킬이 들어갈 경우, 애니메이션을 표현해주는 interface
    /// </summary>
    public interface IBoxselect
    {
        public void Activate_Grid(); // 그리드 선택 가능하게 활성화
        public void Deactivate_Grid(); // 그리스 선택 비활성화

    }
}
