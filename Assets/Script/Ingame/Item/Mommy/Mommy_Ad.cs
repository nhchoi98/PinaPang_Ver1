using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Challenge;
using Data;
using Ingame;
using Manager;
using Score;


namespace Item
{
    public class Mommy_Ad : MonoBehaviour, IComponent
    {
        [SerializeField] private QuestManager _questManager;
        [SerializeField] private SoundManager _soundManager;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private SettingManager _settingManager;

        [Header("UI")] public GameObject lockedObj;
        public Button gem_btn, ad_btn, gem_line, gem_score;
        public Button gem_cross;
        public Button mommy_btn, mommy_adBtn;

        [Header("Object_Pool")] [SerializeField]
        private Transform boxPool;

        public Text gemText;
        public Text conditionText;
        private IMediator _mediator;
        void Start()
        {
            EXP_DAO level_data = new EXP_DAO();

            if(level_data.Get_User_Level()<30){
               lockedObj.SetActive(true);
                return;
            }

            conditionText.text = "(1/1)";
            Mommy_Button_Set();
        }
        
        
        public void UserChoseToWatchAd()
        {
            _mediator.Event_Receive(Event_num.MOMMY_AD);
        }
        
        public void OnClick_Gem()
        {
            
            Playerdata_DAO.Set_Player_Gem(-5);
            gemText.text = string.Format("{0:#,0}",Playerdata_DAO.Player_Gem());
            if (Playerdata_DAO.Player_Gem() < 5)
            {
                gem_cross.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().color =
                    new Color(255f / 255f, 28f / 255f, 26f / 255f);
                gem_line.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().color =
                    new Color(255f / 255f, 28f / 255f, 26f / 255f);
                gem_score.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().color =
                    new Color(255f / 255f, 28f / 255f, 26f / 255f);
                gem_line.interactable = false;
                gem_score.interactable = false;
                gem_cross.interactable = false;
            }
            
            mommy_btn.interactable = false;
            mommy_adBtn.interactable = false;
            StartCoroutine(Action());
        }
        
        private void UserEarnedReward()
        {
            gem_btn.interactable = false;
            ad_btn.interactable = false;
            StartCoroutine(Action()); // 효과 

        }

        public void Mommy_Button_Set()
        {
            if (Playerdata_DAO.Player_Gem() < 5)
            {
                mommy_btn.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().color =
                    new Color(255f / 255f, 28f / 255f, 26f / 255f);
                mommy_btn.interactable = false;
            }

            else
            {
                if (PlayerPrefs.GetInt("Daily_Mommy", 0) == 1)
                    mommy_btn.interactable = false;

                else
                {
                    int count = PlayerPrefs.GetInt("Daily_Mommy",0);
                    mommy_btn.interactable = true;
                }
            }
        }
        

        IEnumerator  Action()
        {
            Queue <int> target_index = new Queue<int>();
            List <int> origianl_index = new List <int>();
            Queue <GameObject> Remove_Obj = new Queue<GameObject>();
            int child_num = boxPool.childCount;
            int target_num;
            _settingManager.OnClick_Item_Exit();
            _questManager.Set_Item();
            _soundManager.item.Play();// 사운드 
            conditionText.text = "(0/1)";
            // Step 2. 부실 개수 지정하기 
            if (child_num < 3)
                target_num = 1;

            else
                target_num = child_num / 2;

            for (int i = 0; i < child_num; i++)
            {
                if(boxPool.GetChild(i).gameObject.CompareTag("Box") ||boxPool.GetChild(i).gameObject.CompareTag("Obstacle") )
                    origianl_index.Add(i);
            }


            for (int i = 0; i < target_num; i++)
            {
                int rand_num = UnityEngine.Random.Range(0, origianl_index.Count);
                target_index.Enqueue(origianl_index[rand_num]);
                origianl_index.RemoveAt(rand_num);
                
            }
            
            origianl_index.Clear();
            int target_count = target_index.Count;
            for (int i =0; i< target_count; i++)
            {
                int index = target_index.Dequeue();
                Remove_Obj.Enqueue(boxPool.GetChild(index).gameObject);
                
            }
            
            // 나갈 때 까지 기다림 
            while (true)
            {
                if (Time.timeScale == 0)
                    yield return null;

                else
                    break;
            }

            int remove_target = Remove_Obj.Count;
            for(int i =0; i< remove_target; i++)
                StartCoroutine(Remove_Obj_IEnueratoe(Remove_Obj.Dequeue()));
            

            scoreManager.Set_Count_Zero();
        }

   

        IEnumerator Remove_Obj_IEnueratoe(GameObject Target)
        {
            Target.GetComponent<IDestroy_Action>().Destroy_Action();
            yield return null;
        }

        public void Set_Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Occur(Event_num eventNum)
        {
            UserEarnedReward();
        }
    }
}
