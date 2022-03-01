using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public interface IComponent
    {
        public void Set_Mediator(IMediator mediator);
        public void Event_Occur(Event_num eventNum);
    }
}
