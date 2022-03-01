
using UnityEngine;
using Manager;
using Ingame;
using Log;

namespace Ad
{
    public class Revive_Ad : MonoBehaviour, IComponent
    {
        [SerializeField]
        private Revive RV;
        public GameObject RevivePanel;
        private IMediator _mediator;
        public void UserEarnedReward()
        {
            Time.timeScale = 1f;
            RevivePanel.SetActive(false);
            this.gameObject.SetActive(false);
        }

        public void UserChoseToWatchAd()
        {
            _mediator.Event_Receive(Event_num.REVIVE_AD);
        }

        public void Set_Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Occur(Event_num eventNum)
        {
            RV.yes_pushed = true;
            Ingame_Log.Revive(false);
            UserEarnedReward();
            RV.OnClick_Ad_Revive();
        }
    }
}
