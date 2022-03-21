using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public interface IComponent
    {
        /// <summary>
        /// 중재자에 접근할 수 있도록 컴포넌트에 중재자 주소를 전달해주는 함수 
        /// </summary>
        /// <param name="mediator"></param>
        public void Set_Mediator(IMediator mediator);
        
        /// <summary>
        /// 중재자가 이 함수를 호출하게 되면, EVENT_NUM에 맞게 액션을 취해주게 하는 함수. 
        /// </summary>
        /// <param name="eventNum"></param>
        public void Event_Occur(Event_num eventNum);
    }
}
