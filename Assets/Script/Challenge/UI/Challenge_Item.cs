
using System.Collections;
using Alarm;
using UnityEngine;
using UnityEngine.UI;
using Challenge;
using Data;

public class Challenge_Item: MonoBehaviour
{
    private ChallengeDAO data;
    public Transform Item_TR;
    [SerializeField] private Level_UI _levelUI;
    [SerializeField] private Challenge_UI _challengeUI;
    [SerializeField] private QuestManager _questManager;
    [SerializeField] private IAlarmMediator _alarmMediator;
    public AudioSource get_reward;
    
    private bool first_load = true;
    public bool quest_mission = false;
    private void Awake()
    {
        _alarmMediator = GameObject.FindWithTag("alarmcontrol").GetComponent<IAlarmMediator>();
        data = new ChallengeDAO();
        for (int i = 0; i < Item_TR.childCount; i++)
        {
            StartCoroutine(Set_Grid_UI(i));
        }
    }

    /// <summary>
    /// 뽑기 횟수만 따로 검사 
    /// </summary>
    private void OnEnable()
    {
        Set_TR_count();
    }

    public void Purchase_Free_Item_Package()
    {
        data.Set_FreeItem_Purchase();
        quest_mission = true;
        Set_TR_count();
    }

    /// <summary>
    /// 뽑기나 27번 퀘스트 있으면, 바로 반영토록 하는 함수 
    /// </summary>
    private void Set_TR_count()
    {
         if (quest_mission)
        {
            for (int i = 0; i < Item_TR.childCount; i++)
            {
                if(data.Get_Item_index(i) / 3 == 7 || data.Get_Item_index(i)/3 == 8 || data.Get_Item_index(i) == 27)
                {
                    data = new ChallengeDAO(); // 데이터 다시 읽어들임 
                    var target_num = 0;
                    var num = 0;
                    // 1. 보상받기 버튼 활성 유무 결정하기 
                    if (data.Get_Achievement(i))
                    {
                        Item_TR.GetChild(i).GetChild(3).GetChild(3).gameObject.GetComponent<Text>().text = "Complete!";
                        Item_TR.GetChild(i).GetChild(3).gameObject.GetComponent<Slider>().value = 1;
                        Item_TR.GetChild(i).GetChild(3).GetChild(1).GetChild(0).gameObject.GetComponent<Image>().color =
                            Color.green;

                        if (!data.Get_itemget_data(i))
                        {
                            Item_TR.GetChild(i).GetChild(4).gameObject.SetActive(true);
                            Item_TR.GetChild(i).GetChild(4).gameObject.GetComponent<Button>().interactable = true;
                            Item_TR.GetChild(i).GetChild(5).gameObject.SetActive(false);
                            Item_TR.GetChild(i).GetChild(4).GetChild(0).gameObject.SetActive(false);
                        }

                        else
                        {
                            Item_TR.GetChild(i).GetChild(4).gameObject.SetActive(false);
                            Item_TR.GetChild(i).GetChild(5).gameObject.SetActive(true);
                            continue;
                            // 슬라이더 색상 변경해주기 
                        }
                    }

                    else
                    {
                        Item_TR.GetChild(i).GetChild(4).gameObject.GetComponent<Button>().interactable = false;
                        Item_TR.GetChild(i).GetChild(5).gameObject.SetActive(false);
                        Item_TR.GetChild(i).GetChild(4).GetChild(0).gameObject.SetActive(true);
                    }

                    // 2. Progress 정도 UI에 반영하기
                    data.Get_Challenge_data(i, ref target_num, ref num);
                    Item_TR.GetChild(i).GetChild(3).gameObject.GetComponent<Slider>().value = num / (float) target_num;
                    if (target_num < 1000)
                        Item_TR.GetChild(i).GetChild(3).GetChild(3).gameObject.GetComponent<Text>().text =
                            num + "/" + target_num;

                    else
                        Item_TR.GetChild(i).GetChild(3).GetChild(3).gameObject.GetComponent<Text>().text =
                            num + "/" + string.Format("{0:#,0}", target_num);
                    
                }
            }

            quest_mission = false;
        }
        
    }
    public IEnumerator Ad_show()
    {
        data.Init_data(false);
        for (int i = 0; i < Item_TR.childCount; i++)
            StartCoroutine(Set_Grid_UI(i,true));
        
        yield return null;
    }
    

    IEnumerator Set_Grid_UI(int i, bool is_reset = false)
    {
        // 0. Description, title, image 반영해주기 
            Challenge_Descrip descrip = new Challenge_Descrip(data.Get_Item_index(i));
            int reward_num = 0;
            var target_num = 0;
            var num = 0;
            Transform TR = Item_TR.GetChild(i);
            Image image = Item_TR.GetChild(i).GetChild(2).GetChild(0).gameObject.GetComponent<Image>();
            Text title = Item_TR.GetChild(i).GetChild(0).gameObject.GetComponent<Text>();
            Text desc = Item_TR.GetChild(i).GetChild(1).gameObject.GetComponent<Text>();
            Animator animator = TR.GetComponent<Animator>();
            // 타이틀 정보 반영 
            if (data.Get_Item_index(i) == 27)
                quest_mission = true;
            
            descrip.Get_Title(ref title);
            // 설명 정보 반영 
            descrip.Get_Desc(ref desc);

            // 보상 정보 반영 
            descrip.Get_target(ref reward_num);
            TR.GetChild(4).GetChild(1).gameObject.GetComponent<Text>().text =
                (reward_num / 100).ToString();// 젬 정보 반영
            TR.GetChild(4).GetChild(2).gameObject.GetComponent<Text>().text =
                (reward_num % 100).ToString(); // 경험치 정보 적용 
            
            // 맞는 이미지 반영하기 
            TR.GetChild(2).gameObject.GetComponent<Image>().sprite =
                Quest_Img.Get_Quest_Panel(data.Get_Item_index(i));
            
            image.sprite =
                Quest_Img.Get_Quest_main(data.Get_Item_index(i));
            
            image.SetNativeSize();
            if (!is_reset)
            {
                // 1. 보상받기 버튼 활성 유무 결정하기 
                if (data.Get_Achievement(i))
                {
                    TR.GetChild(3).GetChild(3).gameObject.GetComponent<Text>().text = "Complete!";
                    TR.GetChild(3).gameObject.GetComponent<Slider>().value = 1;
                    TR.GetChild(3).GetChild(1).GetChild(0).gameObject.GetComponent<Image>().color = Color.green;
                    if (!data.Get_itemget_data(i))
                    {
                        TR.GetChild(4).gameObject.GetComponent<Button>().interactable = true;
                        TR.GetChild(4).gameObject.SetActive(true);
                        TR.GetChild(5).gameObject.SetActive(false);
                        TR.GetChild(4).GetChild(0).gameObject.SetActive(false);
                    }

                    else
                    {
                        // 스탬프 찍혀 있어야함 
                        TR.GetChild(4).gameObject.SetActive(false);
                        TR.GetChild(5).gameObject.SetActive(true);
                        yield break;
                        // 슬라이더 색상 변경해주기 
                    }
                }

                else
                {
                    TR.GetChild(4).gameObject.GetComponent<Button>().interactable = false;
                    TR.GetChild(4).gameObject.SetActive(true);
                    TR.GetChild(5).gameObject.SetActive(false);
                    TR.GetChild(4).GetChild(0).gameObject.SetActive(true);
                }
            }

            else
            {
                TR.GetChild(4).gameObject.GetComponent<Button>().interactable = false;
                TR.GetChild(4).gameObject.SetActive(true);
                TR.GetChild(5).gameObject.SetActive(false);
                TR.GetChild(4).GetChild(0).gameObject.SetActive(true);
            }

            // 2. Progress 정도 UI에 반영하기
            data.Get_Challenge_data(i, ref target_num, ref num);
            Item_TR.GetChild(i).GetChild(3).gameObject.GetComponent<Slider>().value = num / (float) target_num;
            if(target_num<1000)
                Item_TR.GetChild(i).GetChild(3).GetChild(3).gameObject.GetComponent<Text>().text = num + "/" + target_num;
            
            else
                Item_TR.GetChild(i).GetChild(3).GetChild(3).gameObject.GetComponent<Text>().text = string.Format("{0:#,0}",num) + "/" + string.Format("{0:#,0}",target_num);
            
            yield return null;
    }

    /// <summary>
    /// 보상 받기 버튼을 클릭하면, 보상을 사용자 데이터에 반영해주는 함수 
    /// </summary>
    /// <param name="index"></param>
    public void Get_reward(int index)
    {
        // Step 1. data에 반영여부 등록
        get_reward.Play();
        var num = data.Get_Item_index(index);
        Challenge_Descrip descrip = new Challenge_Descrip(num);
        int reward_num = 0;
        descrip.Get_target(ref reward_num);
        var gem = reward_num / 100;

        if (data.Get_Item_index(index) != 27)
        {
            _questManager.Set_Quest();
        }

        data.Set_reward_get(index);
        Set_TR_count(); // 30번 퀘스트가 있는 경우 리셋해줌 
        Item_TR.GetChild(index).GetChild(4).gameObject.SetActive(false);
        Item_TR.GetChild(index).GetChild(4).gameObject.GetComponent<Button>().interactable = false;
        Item_TR.GetChild(index).GetChild(4).GetChild(0).gameObject.SetActive(true);
        Item_TR.GetChild(index).GetChild(3).GetChild(3).gameObject.GetComponent<Text>().text = "Complete!";
        Item_TR.GetChild(index).GetChild(3).gameObject.GetComponent<Slider>().value = 1;
        Item_TR.GetChild(index).GetChild(3).GetChild(1).GetChild(0).gameObject.GetComponent<Image>().color = Color.green;
        _levelUI.Set_User_Exp(num, index);
        
        // Step 2. 젬 획득 반영. 이 때, 연출이 있어야함 
        _challengeUI.StartCoroutine(_challengeUI.Get_Gem(index,gem));
        Playerdata_DAO.Set_Player_Gem(gem);
        
        // Step 3. 도장이 쾅 찍히는 애니메이션이 들어가야함 
        Item_TR.GetChild(index).GetChild(5).gameObject.SetActive(true);
        Item_TR.GetChild(index).GetChild(5).gameObject.GetComponent<Animator>().SetTrigger("stamp");
        Set_TR_count();
        _alarmMediator.Event_Receieve(Event_Alarm.QUEST_ALARM_FALSE, index);
    }
    
  
}
