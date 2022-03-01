
using Ingame;
using Lobby;
using UnityEngine;
using UnityEngine.UI;

namespace Battery
{
    public class BatteryAd : MonoBehaviour, IComponent
    {
        private IMediator _mediator;
        [SerializeField] private PlayerBtn_Set setPlayBtn;
        
        public void Set_Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Occur(Event_num eventNum)
        {
            UserEarnedReward();
        }

        private void UserEarnedReward()
        {
            setPlayBtn.Ad_Btn_Clicked();
            setPlayBtn.Set_Ad_Btn();
        }
        
        public void UserChoseToWatchAd()
        {
            _mediator.Event_Receive(Event_num.BATTERY);
        }
    }
}
