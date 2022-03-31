using System;
using System.Collections;
using System.Collections.Generic;
using Block;
using NewBlock;
using Particle;
using Score;
using UnityEngine;

namespace Ingame
{
    
    /// <summary>
    /// 어떤 박스를 얼마나 , 어디에 생성할지 결정하는 클래스 
    /// </summary>
    public class Determine_BoxType : MonoBehaviour, IComponent
    {
        private IMediator _mediator;
        
        [SerializeField] private RespawnBox _respawnBox;
        [SerializeField] private LocateBox _locateBox;
        private bool newBlock = false;
        
        [Header("Game_Info")]
        private int wave;
        private int stage;
        [SerializeField] private ScoreManager _scoreManager;
        
        [Header("Spawn_Num")]
        private int blockMakeNum;
        private int specialNum;
        
        [Header("Probability")]
        private List<Tuple<int,int>> prob_set = new List<Tuple<int, int>>(); // 박스의 등장 경우의 수를 저장해놓는 Tuple Type lIST. Item 1: 몇 개? Item 2: 몇 개 픽 남음? 
        private Probability_Set probability;
        
        [Header("Candle")]
        private bool crossItem;

        public GameObject newBlockInfo;
        private const int normalVariation_Target = 30;
        private const int x2_Target = 50;
        private const int obstacle_Target = 150;

        [Header("TargetNum_Info")]
        private int transformedNum;// 0 ; 삼각형, 1; 반반 , 2;마름모, 3;원
        private int classyNum;
        private int obstacleNum;
       

        public Transform diePool;

        IEnumerator Remove_DieBox()
        {
            for (int i = 0; i < diePool.childCount; i++)
                Destroy(diePool.GetChild(i).gameObject);
            yield return null;
        }

        /// <summary>
        /// 박스를 뭘 리스폰할지 결정하고, 위치까지 잡아주는 함수 
        /// </summary>
        private void Spawn_Box()
        {
            int boxnum;
            int transform_quantity = 0; // 변형 개수 
            // #1. 뭘 리스폰 할거야? 넘버 타입 지정해주기
            StartCoroutine(Remove_DieBox());
            _respawnBox.Event_Occur(99,stage); // 스폰할 박스 HP 정보 넘겨주기 
            if (stage % 10 == 1 && stage > 10)
            {
                if (stage > normalVariation_Target) Set_Transformed_Num();
                
                if (stage > x2_Target) Set_Classy_Num();

                if (stage > obstacle_Target) Set_Obstacle_num();
                Set_Spawn_Prob();
            }
            
            // #2. 얼마나 리스폰 할거야? 리스폰 개수 지정해주기  
            boxnum = Set_block_make_num();
            if (stage <31)
            {
                specialNum = 0;
                transform_quantity = 0;
            }

            else
            {
                if (stage > 50)
                {
                    specialNum = Set_Special_num();
                    boxnum -= specialNum;
                }

                transform_quantity = Set_Transformed_Quantity();
                boxnum -= transform_quantity;
            }
            
            // #2. group 일단 리스폰해 
            for(int i=0; i<boxnum; i++)
                _respawnBox.Event_Occur(0,stage);

            if (specialNum > 0)
            {
                for(int i =0; i<specialNum; i++)
                    Determine_SpecialBox();
            }

            if (transform_quantity > 0)
            {
                for(int i =0; i<transform_quantity; i++)
                    _respawnBox.Event_Occur(1,transformedNum);
                
            }
            
            // #3. 위치 잡아줘 
            _locateBox.Locate_Box();
            stage++;
        }

        /// <summary>
        /// 검토 필요 
        /// </summary>
        /// <returns></returns>
        private int Set_Special_num()
        {
            int special_prob = UnityEngine.Random.Range(0, 100);// # 1. transform quantity 지정 
            int doubleTarget, singleTarget;
            int specialmNum = 0;
            if (stage < 51)
            {
                doubleTarget = 25;
                singleTarget = doubleTarget + 35;

            }

            else if(stage >= 51 && stage < 101)
            {
                doubleTarget = 30;
                singleTarget = doubleTarget + 35;
            }
            
            else if (stage >= 101 && stage < 201)
            {
                doubleTarget = 40;
                singleTarget = doubleTarget + 35;
            }

            else
            {                
                doubleTarget = 50;
                singleTarget = doubleTarget + 30;
            }
            // # 2. transform quantity 지정 
            if (doubleTarget<special_prob)
            {
                if (!newBlock)
                    specialmNum = 2;
                else
                    specialmNum = 1;
            }
            
            
            else if (doubleTarget >= special_prob && singleTarget < special_prob)
            {
                specialmNum = 1;
            }

            else
                specialmNum = 0;

            return specialmNum;
        }

        private int Set_Transformed_Quantity()
        {
            int transform_prob = UnityEngine.Random.Range(0, 100);// # 1. transform quantity 지정 
            int doubleTarget, singleTarget;
            int transformNum = 0;
            if (stage < 101)
            {
                doubleTarget = 35;
                singleTarget = doubleTarget + 35;
            }

            else if(stage >= 101 && stage < 201)
            {
                doubleTarget = 40;
                singleTarget = doubleTarget + 35;
            }
            
            else
            {                
                doubleTarget = 50;
                singleTarget = doubleTarget + 30;
            }
                // # 2. transform quantity 지정 
            if (doubleTarget<transform_prob)
            {
                if (!newBlock)
                    transformNum = 2;
                
                else
                    transformNum = 1;
            }
            
            
            else if (doubleTarget >= transform_prob && singleTarget < transform_prob)
            {
                transformNum = 1;
            }

            else
                transformNum = 0;

            return transformNum;
        }
        
        
        #region Prob_Set
        private void Set_Spawn_Prob()
        {
            Probability_VO DATA;
            prob_set.Clear();
            if (wave < 16)
            {
                DATA = probability._probabilty_Set(wave);
                wave++;
            }
            
            else 
                DATA = probability._probabilty_Set(16);

            prob_set = new List<Tuple<int, int>>();
            if (DATA.One_Target != 0)
                prob_set.Add(new Tuple<int, int>(1, DATA.One_Target));
    
            if (DATA.Two_Target != 0)
                prob_set.Add(new Tuple<int, int>(2, DATA.Two_Target));

            if (DATA.Three_Target != 0)
                prob_set.Add(new Tuple<int, int>(3, DATA.Three_Target));

            if (DATA.Four_Target != 0)
                prob_set.Add(new Tuple<int, int>(4, DATA.Four_Target));

            if (DATA.Fifth_Target != 0)
                prob_set.Add(new Tuple<int, int>(5, DATA.Fifth_Target));

            if (DATA.Sixth_Target != 0)
                prob_set.Add(new Tuple<int, int>(6, DATA.Sixth_Target));

            if (DATA.Seventh_Target != 0)
                prob_set.Add(new Tuple<int, int>(7, DATA.Seventh_Target));

            
        }
        #endregion

        #region Set_BOXNUMBER
         private void Set_Transformed_Num()
        {
            int firstTarget = 30; // 삼각형이 처음 등장하는 스테이지 
            int secondTarget = 100; // 반반박스가 등장하는 스테이지 
            int last_Target = 250;
            New_Block_Info data;
            if (stage > firstTarget && stage < secondTarget)
            {
                transformedNum = 0;
                if (stage == firstTarget+1)
                {
                    data = new New_Block_Info(0, 0);
                    if (data.Get_value())
                    {
                        _respawnBox.Event_Occur(1,transformedNum);
                        newBlock = true;
                        data.Set_InfoPanel_UI(ref newBlockInfo);
                        _mediator.Event_Receive(Event_num.New_block);
                    }
                }
            }

            else if (stage > secondTarget && stage < last_Target)
            {
                if (stage == secondTarget+1)
                {
                    data = new New_Block_Info(0,1);
                    if (data.Get_value())
                    {
                        transformedNum = 1;
                        _respawnBox.Event_Occur(1,transformedNum);
                        newBlock = true;
                        data.Set_InfoPanel_UI(ref newBlockInfo);
                        _mediator.Event_Receive(Event_num.New_block);
                    }

                    else
                        transformedNum = UnityEngine.Random.Range(0, 2);
                }

                else
                    transformedNum = UnityEngine.Random.Range(0, 2);

            }


            else if (stage > last_Target)
            {
                if (stage == last_Target+1)
                {
                    data = new New_Block_Info(0,2);
                    if (data.Get_value())
                    {
                        transformedNum = 2;
                        _respawnBox.Event_Occur(1,transformedNum);
                        newBlock = true;
                        data.Set_InfoPanel_UI(ref newBlockInfo);
                        _mediator.Event_Receive(Event_num.New_block);
                    }

                    else
                        transformedNum = UnityEngine.Random.Range(0, 3);
                    
                }

                else
                    transformedNum = UnityEngine.Random.Range(0, 3);
                
            }
            else
                transformedNum = -1;
            return;
        }

        private void Set_Classy_Num()
        {
            int firstTarget = 50; // 두 배 박스가 처음 등장하는 스테이지 
            int second_target = 130; // 두 배 삼각형이 처음 등장하는 스테이지 
            int last_Target = 300;
            New_Block_Info data;
            if (stage > firstTarget && stage < second_target)
            {
                classyNum = 0;
                if (stage == firstTarget+1)
                {
                    data = new New_Block_Info(1,0);
                    if (data.Get_value())
                    {
                        _respawnBox.Event_Occur(2,classyNum);
                        newBlock = true;
                        classyNum = 0;
                        data.Set_InfoPanel_UI(ref newBlockInfo);
                        _mediator.Event_Receive(Event_num.New_block);
                    }
                }
            }
            
            
            else if (stage > second_target && stage < last_Target)
            {
                
                if (stage == second_target+1)
                {
                    data = new New_Block_Info(1,1);
                    if (data.Get_value())
                    {
                        classyNum = 1;
                        _respawnBox.Event_Occur(2,classyNum);
                        newBlock= true;
                        data.Set_InfoPanel_UI(ref newBlockInfo);
                        _mediator.Event_Receive(Event_num.New_block);
                    }

                    else
                        classyNum = UnityEngine.Random.Range(0, 2);
                }

                else
                    classyNum = UnityEngine.Random.Range(0, 2);
            }

            else if (stage > last_Target)
            {
                if (stage == last_Target+1)
                {
                    data = new New_Block_Info(1,2);
                    if (data.Get_value())
                    {
                        classyNum = 2;
                        _respawnBox.Event_Occur(2,classyNum);
                        newBlock = true;
                        data.Set_InfoPanel_UI(ref newBlockInfo);
                        _mediator.Event_Receive(Event_num.New_block);
                    }

                    else
                        classyNum = UnityEngine.Random.Range(0, 3);
                }

                else
                    classyNum = UnityEngine.Random.Range(0, 3);
            }
            else
                classyNum = -1;
            return;
        }

        /// <summary>
        /// 장애물 블록을 뜨게 만드는 함수 
        /// </summary>
        private void Set_Obstacle_num()
        {
            int first_target = 150;
            int second_target = 200;
            New_Block_Info data;
            if (stage > first_target && stage < second_target)
            {
                obstacleNum = 0;
                if (stage == first_target+1)
                {
                    data = new New_Block_Info(2,0);
                    if (data.Get_value())
                    {
                        _respawnBox.Event_Occur(3,obstacleNum);
                        newBlock = true;
                        data.Set_InfoPanel_UI(ref newBlockInfo);
                        _mediator.Event_Receive(Event_num.New_block);
                    }
                }
            }

            else if (stage > second_target)
            {                
                
                if (stage == second_target+1)
                {
                    data = new New_Block_Info(2,1);
                    if (data.Get_value())
                    {
                        obstacleNum = 1;
                        _respawnBox.Event_Occur(3,obstacleNum);
                        newBlock= true;
                        data.Set_InfoPanel_UI(ref newBlockInfo);
                        _mediator.Event_Receive(Event_num.New_block);
                    }

                    else
                        obstacleNum = UnityEngine.Random.Range(0, 2);
                }

                else
                    obstacleNum = UnityEngine.Random.Range(0, 2);

            }

            else
                obstacleNum  = -1;
            return;
        }
        

        #endregion
        
        private int Set_block_make_num()
        {
            int block_make_num; 
            int rand_num = UnityEngine.Random.Range(0, prob_set.Count);
            block_make_num = prob_set[rand_num].Item1;
            if (prob_set[rand_num].Item2 - 1 == 0)
                prob_set.RemoveAt(rand_num);
            
            else
                prob_set[rand_num] = new Tuple<int, int>(prob_set[rand_num].Item1, prob_set[rand_num].Item2 - 1);

            if (newBlock)
                block_make_num -= 1;
            
            return block_make_num;
        }

        /// 뭘 리스폰할 건지?

        #region Which_Respawn

        private void Determine_SpecialBox()
        {
            int rand = 0;
            
            // # 1. 나무, 장애물 중 하나 
            
            if (stage >= x2_Target && stage <= obstacle_Target)
                rand = 1;
            
            else if (stage > obstacle_Target)
                rand = UnityEngine.Random.Range(1, 3);

            else
                rand = 0;
            
            switch (rand)
            {
                default:
                    return;

                case 1:
                    _respawnBox.Event_Occur(2,classyNum);
                    break;
                
                case 2:
                    _respawnBox.Event_Occur(3,obstacleNum);
                    break;
            }
        }

        #endregion
        
        #region Mediation_Action
        public void Set_Mediator(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public void Event_Occur(Event_num eventNum)
        {
            switch (eventNum)
            {
                case Event_num.BOX_SPAWN:
                    Spawn_Box();
                    break;  
                
                case Event_num.LOAD_DATA:
                    probability = new Probability_Set();
                    
                    break;
                
                case Event_num.SET_NEW_STAGE:
                    _scoreManager.Set_score_const(stage-1);
                    break;
                
                // 처음 게임 입장했을 경우 
                case Event_num.INIT_DATA:
                    probability = new Probability_Set();
                    wave = 0;   //0
                    stage = 1;   // 1
                    transformedNum = -1;
                    classyNum = -1;
                    obstacleNum = -1;
                    Set_Spawn_Prob();
                    Spawn_Box();
                    break;
                
                case Event_num.CROSS_ITEM:
                    _locateBox.Set_Both_Item();
                    break;
                
                case Event_num.PINATA_DIE:
                    _locateBox.pinata_die();
                    _scoreManager.StartCoroutine(_scoreManager.Get_Pinata_Remove());
                    break;
                
                case Event_num.PINATA_SPAWN:
                    _locateBox.Pinata_Spawn(stage);
                    Event_Occur(Event_num.BOX_SPAWN);
                    break;
                
                case Event_num.Tutorial_INIT:
                    probability = new Probability_Set();
                    wave = 0;
                    stage = 2;
                    transformedNum = -1;
                    classyNum = -1;
                    obstacleNum = -1;
                    Set_Spawn_Prob(); // 캔들박스 안나오게함 
                    _respawnBox.Event_Occur(90,1); // 지정된 위치에 박스가 등장함 
                    _locateBox.Set_Is_tutorial();
                    break;
                

            }
            
            // 다 끝나면 박스 내리는 신호 보내! 
        }

        public int Get_Stage()
        {
            return stage;
        }
        #endregion

        public StageInfoVO Get_StageData()
        {
            StageInfoVO stageData = new StageInfoVO();
            stageData.score = this._scoreManager.score;
            stageData.wave = this.wave;
            stageData.stage = this.stage;
            return stageData;
        }

        public void Set_StageData(StageInfoVO data)
        {
            probability = new Probability_Set();
            this.stage = data.stage;
            this.wave = data.wave;
            _scoreManager.Load_Score_Data(data.score,data.stage);
            if (stage > normalVariation_Target) Set_Transformed_Num();        /// JSON에 정보 추가 필요. Transform num, classy num, obstacle num을 추가해야해! (03.30) 
                
            if (stage > x2_Target) Set_Classy_Num();

            if (stage > obstacle_Target) Set_Obstacle_num();
            Set_Spawn_Prob(); // 박스 확률 정보 수정해줌 
        }
    }
}
