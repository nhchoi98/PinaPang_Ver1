using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public class RemoveBox : MonoBehaviour, IComponent
    {
        private IMediator _mediator;
        
        [SerializeField] private GameObject Train;
        public void Set_Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Occur(Event_num eventNum)
        {
            switch (eventNum)
            {
                case Event_num.BOX_REMOVE:
                case Event_num.BADGE_BOX_REMOVE:
                    Train.SetActive(true);
                    break;
            }
        }
    }
}
