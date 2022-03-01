
using Firebase.Analytics;
using Ad;
using UnityEngine;
using UnityEngine.UI;
using Ingame;

namespace Challenge
{
    public class Reset_Ad : MonoBehaviour, IComponent
    {
        [SerializeField] private Challenge_Item item;
        public Transform buttonTR;
        private Button item_Btn;
        public AudioSource click;
        private IMediator _mediator;
        public AudioSource resetSound;
        void Awake()
        {
            if (Noads_instance.Get_Is_Noads())
            {
                buttonTR.GetChild(1).gameObject.SetActive(true);
                item_Btn = buttonTR.GetChild(1).gameObject.GetComponent<Button>();
            }

            else
            {
                buttonTR.GetChild(0).gameObject.SetActive(true);
                item_Btn = buttonTR.GetChild(0).gameObject.GetComponent<Button>();
            }

            if (PlayerPrefs.GetInt("chal_count", 0) == 0)
            {
                item_Btn.interactable = true;
                item_Btn.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "(1/1)";
            }

            else
            {
                item_Btn.interactable = false;
                item_Btn.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "(0/1)";
            }

            
        }

        public void Noads_Click()
        {
            UserEarnedReward();
            FirebaseAnalytics.LogEvent("Reset_Ad");
            item.StartCoroutine(item.Ad_show());
        }


        public void UserChoseToWatchAd()
        {
            _mediator.Event_Receive(Event_num.QUEST_RESET);
            click.Play();
        }
        
        private void UserEarnedReward()
        {
            item_Btn.interactable = false;
            item_Btn.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "(0/1)";
            PlayerPrefs.SetInt("chal_count", 1);
            resetSound.Play();
        }


        public void Set_Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Occur(Event_num eventNum)
        {
            UserEarnedReward();
            item.StartCoroutine(item.Ad_show());
        }

        public void Click_Noads()
        {
            if (Noads_instance.Get_Is_Noads())
            {
                buttonTR.GetChild(1).gameObject.SetActive(true);
                item_Btn = buttonTR.GetChild(1).gameObject.GetComponent<Button>();
            }

            else
            {
                buttonTR.GetChild(0).gameObject.SetActive(true);
                item_Btn = buttonTR.GetChild(1).gameObject.GetComponent<Button>();
            }
        }
    }
}
