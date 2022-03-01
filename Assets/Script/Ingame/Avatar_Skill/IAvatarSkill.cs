using System.Collections;
using camera;
using UnityEngine;
namespace Ingame
{
    public interface  IAvatarSkill
    {
        public abstract void OnClick_Skill_Activate(); // 버튼과 연결되는 함수 
        public abstract void Set_Animation(); // 실행하고 나서, 애니메이션을 실행시켜주는 함수 
        public abstract void Activate(); // 스킬 실행 알고리즘을 담고 있는 알고리즘 
        public abstract void Cancle();
        public abstract void Set_count(); // 스킬 사용 불가 상태에서 카운트를 세주는 함수 
        public abstract void Set_SkillBtn(Transform tr);
    }
}
