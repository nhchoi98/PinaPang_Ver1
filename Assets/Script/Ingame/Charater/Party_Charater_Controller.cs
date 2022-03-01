using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avatar;
using Ingame;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class Party_Charater_Controller : MonoBehaviour , IComponent
    {
        private IMediator _mediator;
        public Animator chara_ani;
        public AudioSource launchSound;
        private bool[,] Target_Pos;
        private Transform boxTR;
        private Transform itemTR;
        private Skill_1006 skillComp;

        private Vector2 targetPos;
        private GameObject launch_particle;
        private void Awake()
        {
            var equipDATA = new EquippedDAO();
            int index;
            index = equipDATA.Get_Equipped_index();
            chara_ani = this.GetComponent<Animator>();
            chara_ani.runtimeAnimatorController = Set_Avatar_UI.Set_Charater_GameObject(index);// 애니메이션 바꾸어줌. 임시로 원숭이 
            SpriteRenderer shadow = chara_ani.gameObject.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
            launchSound = this.gameObject.transform.GetChild(3).gameObject.GetComponent<AudioSource>();
            boxTR = GameObject.FindWithTag("BoxGroup").transform;
            itemTR = GameObject.FindWithTag("itemgroup").transform;
            _mediator = GameObject.FindWithTag("GameController").GetComponent<IMediator>();
            skillComp = GameObject.FindWithTag("Skill").GetComponent<Skill_1006>();
            GameManage manage = GameObject.FindWithTag("GameController").GetComponent<GameManage>();
            manage.Party_Set(this);
            launch_particle = Resources.Load<GameObject>("Ingame/Skill/Party/PartyAnimal");

        }

        public void Set_Launch()
        {
            launchSound.Play();
            _mediator.Event_Receive(Event_num.Launch_MOTION);
        }

        public void Set_Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Event_Occur(Event_num eventNum)
        {
            switch (eventNum)
            {
                case Event_num.AVATAR_SET:
                    _mediator.Event_Receive(Event_num.Launch_Red);
                    Gem_Launch();
                    break;
            }
        }
        
        #region Gem_Action
        private void Gem_Launch()
        {
            Init_TargetPos();
            targetPos = Set_Find_TargetPos();
            if (targetPos == Vector2.zero)
            {
                _mediator.Event_Receive(Event_num.Launch_Green);
                return;
            }
            chara_ani.SetTrigger("Gem_Launch");
        }

        public void Set_Gem_Launch()
        {
            Vector2 startPos;
            GameObject particle =Instantiate(launch_particle) ;
            particle.transform.SetParent(this.transform);
            if (this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX)
            {
                startPos = new Vector2(transform.position.x - 30, transform.position.y + 170f);
                particle.transform.localPosition = new Vector2(-23f, 185f);
            }
            else
            {
                startPos = new Vector2(transform.position.x + 30, transform.position.y + 170f);
                particle.transform.localPosition = new Vector2(33f, 185f);
            }

            particle.SetActive(true);
            skillComp.Launch_Gem(targetPos, startPos);
        }
        
        public void Gem_Launch_Done()
        {
            _mediator.Event_Receive(Event_num.Launch_Green);
            
        }
        
        // # 1. 젬 비행 시키는거

        #endregion

        #region Calc_Target
        private Vector2 Set_Find_TargetPos()
        {
            int rand;
            List<Tuple<int, int>> candidate_pos = new List<Tuple<int, int>>();
            Vector2 target;
            
            for (int i = 0; i < 8; i++)
            {
                for (int k = 1; k < 5; k++)
                {
                    if(!Target_Pos[i,k])
                        candidate_pos.Add(new Tuple<int, int>(i, k));
                }
            }
                
            if(candidate_pos.Count == 0)
                return Vector2.zero;

            else
            {
                rand = UnityEngine.Random.Range(0, candidate_pos.Count);
                target = _Determine_Pos.Which_Pos(candidate_pos[rand].Item2, candidate_pos[rand].Item1);
            }
            return target;
        }

        private void Init_TargetPos()
        {
            Target_Pos = new bool[8, 7];
            for (int i = 0; i < boxTR.childCount; i++)
            {
                int row= 0;
                int col=0;
                _Determine_Pos.Calc_Which_Grid(ref row, ref col, boxTR.GetChild(i).position);
                if(row>=2 && row <=6)
                    Target_Pos[col, row] = true;
            }

            for (int i = 0; i < itemTR.childCount; i++)
            {
                int row= 0;
                int col=0;
                _Determine_Pos.Calc_Which_Grid(ref row, ref col, itemTR.GetChild(i).position);
                if(row>=2 && row <=6)
                    Target_Pos[col, row] = true;
                
            }
            
            for (int i = 0; i < 8; i++)
            {
                Target_Pos[i, 1] = true;
            }
        }
        #endregion
        
    }
}
