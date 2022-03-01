using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame
{
    public class Skill_1006 : MonoBehaviour, IAvatarSkill
    {
        private Transform boxTR;
        private IMediator _mediator;
        [SerializeField] private Text gemText;
        private int gem = 0;
        
        public GameObject gemObj;
        public Animator candyAnimator;
        private bool[,] Target_Pos;

        private Transform skillBtn;
        public AudioSource launchsound;
        private void Awake()
        {
            boxTR = GameObject.FindWithTag("BoxGroup").transform;
            _mediator = GameObject.FindWithTag("GameController").GetComponent<IMediator>();
            gem = Playerdata_DAO.Player_Gem();
            gemText.text = String.Format("{0:#,0}", gem);
            GameObject charater = GameObject.FindWithTag("Player");
            Destroy(charater.GetComponent<Charater_Contorller>());
            charater.AddComponent<Party_Charater_Controller>();
            Debug.Log("호출");
        }

        #region Notuse_Action
        public void OnClick_Skill_Activate()
        {
            
        }

        public void Set_Animation()
        {
            
        }

        public void Activate()
        {
            
        }

        public void Cancle()
        {
            
        }



        public void Set_SkillBtn(Transform tr)
        {
            skillBtn = tr;
            skillBtn.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skin/Skill_icon/Skill_PartyAnimal");
            skillBtn.gameObject.GetComponent<Button>().interactable = false;
            skillBtn.GetChild(0).gameObject.SetActive(false);
            tr.gameObject.SetActive(true);
        }
        

        #endregion

        public void Launch_Gem(Vector2 TargetPos, Vector2 startPos)
        {
            Candy_Flying script;
            GameObject obj = Instantiate(gemObj, startPos,Quaternion.identity); // 잼 획득 연출 넣기 
            script = obj.GetComponent<Candy_Flying>();
            obj.transform.SetParent(boxTR);
            script.candy_animator = this.candyAnimator;
            script.arrive += set_text;
            script.Target_Pos = TargetPos;
            launchsound.Play();
        }
        public void Set_count()
        {
            bool is_launch = Determine_Launch_Candy();
            Vector2 target;
            if (is_launch)
            {
                _mediator.Event_Receive(Event_num.AVATAR_SET);
                // 애니메이션 시작 
            }
            
        }
        private bool Determine_Launch_Candy()
        {
            int rand = UnityEngine.Random.Range(0, 100);
            if (rand < 15)
                return true;

            else
                return false;
        }
        
        private void set_text(object sender, EventArgs e)
        {
            gem += 1;
            gemText.text = String.Format("{0:#,0}", gem);
        }
        
       
    }
}

