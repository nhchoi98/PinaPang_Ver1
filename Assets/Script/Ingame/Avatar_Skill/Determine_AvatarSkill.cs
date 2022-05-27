using System;
using Avatar;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame
{
    public class Determine_AvatarSkill : MonoBehaviour
    {
        private Avatar_3002_Skill skill_3002;
        private IAvatarSkill _avatarSkill;
        private IMediator _mediator;
        public GameObject skillBtn;
        public GameObject skill;
        private int index;  

        private bool is_active;
        
        private void Awake()
        {
            var equipDATA = new EquippedDAO();
            int index;
            index = equipDATA.Get_Equipped_index();
            _mediator = GameObject.FindWithTag("GameController").GetComponent<IMediator>();
            GameObject skill_obj;
            GameObject obj_instance;
            switch (index)
            {
                default:
                    is_active = false;
                    break;
                
                case 1006:
                    skill_obj = Resources.Load<GameObject>("Ingame/Skill/Party/Party_Skill");
                    obj_instance = Instantiate(skill_obj);
                    obj_instance.transform.SetParent(this.transform);
                    _avatarSkill = obj_instance.GetComponent<IAvatarSkill>();
                    _avatarSkill.Set_SkillBtn(skillBtn.transform);
                    is_active = true;
                    break;
                    
                
                case 2000:
                    skill_obj = Resources.Load<GameObject>("Ingame/Skill/3000/3000Skill");
                    obj_instance = Instantiate(skill_obj);
                    obj_instance.transform.SetParent(this.transform);
                    _avatarSkill = obj_instance.GetComponent<IAvatarSkill>();
                    is_active = true;
                    _avatarSkill.Set_SkillBtn(skillBtn.transform);
                    break;
                
                case 2001:
                   skill_obj = Resources.Load<GameObject>("Ingame/Skill/3001/3001Skill");
                   obj_instance = Instantiate(skill_obj);
                   obj_instance.transform.SetParent(this.transform);
                   _avatarSkill = obj_instance.GetComponent<IAvatarSkill>();
                    is_active = true;
                    _avatarSkill.Set_SkillBtn(skillBtn.transform);
                    break;
                
                case 2002:
                    skill_obj = Resources.Load<GameObject>("Ingame/Skill/3002/3002Skill");
                    obj_instance = Instantiate(skill_obj);
                    obj_instance.transform.SetParent(this.transform);
                    _avatarSkill = obj_instance.GetComponent<IAvatarSkill>();
                    skill_3002 = obj_instance.GetComponent<Avatar_3002_Skill>();
                    is_active = true;
                    _avatarSkill.Set_SkillBtn(skillBtn.transform);
                    break;
            }

        }

        /// <summary>
        /// 제일 마지막 구현. 스킬을 쓴 이후, 활성화 가능한지에 대해 panel을 킬 때 판단함. 
        /// </summary>
        public void Determine_Skill_Active()
        {
            if(is_active)
                _avatarSkill.Set_count();
        }


        public void OnClick_ItemClick()
        {
            _avatarSkill.OnClick_Skill_Activate();
        }

        private void Set_Skill_img()
        {
            
            
        }

        public void Bear_Skill()
        {
            skill_3002.check_land = true;

        }
        
    }
}

