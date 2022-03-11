using System.Collections;
using System.Collections.Generic;
using Ingame;
using UnityEngine;

namespace Attendance
{
    public class Attendance_DoubleAd : MonoBehaviour, IComponent
    {
        private int index;
        [Serial]
        private IMediator _mediator;
        public void Set_Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Occur(Event_num eventNum)
        {
            Get_Reward();
        }

        public void UserChoseToWatchAd()
        {
            _mediator.Event_Receive(Event_num.ATTENDANCE);
        }

        public void Set_Index(int index)
        {
            this.index = index;
        }

        private void Get_Reward()
        {
            // 젬이 날아가는 연출 코드가 들어가면됨.
        }
    }
}
