
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Skin
{
    public class SkillInfo_Set : MonoBehaviour
    {
        public Image skillImg;
        public VideoPlayer skill_video;
        public Text skillTitle;
        public Text skilldesc;
        private int target_index;
        public GameObject obj;
        public void Set_Panel(int index)
        {
            target_index = index;
            Set_Skill_Img();
            Set_Skill_Title();
            Set_Skill_desc();
            Set_Desc_Video();
            obj.SetActive(true);
        }

        private void Set_Desc_Video()
        {
            switch (Calc_Index.Get_Avatar_Num(target_index))
            {
                case 1006:
                    skill_video.clip =
                        Resources.Load<VideoClip>("Lobby/Skill/PartyGirl_Skill");
                    break;
                
                case 2000:
                    skill_video.clip =
                        Resources.Load<VideoClip>("Lobby/Skill/Astronaut_Skill");
                    break;
                
                case 2001:
                    skill_video.clip =
                        Resources.Load<VideoClip>("Lobby/Skill/Pajama_Skill");
                    break;
                
                case 2002:
                    skill_video.clip =
                        Resources.Load<VideoClip>("Lobby/Skill/TeddyBear_Skill");
                    break;
                
                
            }
            
        }
        private void Set_Skill_Img()
        {
            switch (Calc_Index.Get_Avatar_Num(target_index))
            {
                case 1006:
                    skillImg.sprite
                        = Resources.Load<Sprite>("Skin/Skill_icon/Skill_PartyAnimal");
                    break;
                
                case 2000:
                    skillImg.sprite
                        = Resources.Load<Sprite>("Skin/Skill_icon/Skill_Astronaut");
                    break;
                
                case 2001:
                    skillImg.sprite
                        = Resources.Load<Sprite>("Skin/Skill_icon/Skill_Pajama");
                    break;
                
                case 2002:
                    skillImg.sprite
                        = Resources.Load<Sprite>("Skin/Skill_icon/Skill_TeddyBear");
                    break;
                
            }
            
        }

        private void Set_Skill_Title()
        {
            switch (Calc_Index.Get_Avatar_Num(target_index))
            {
                case 1006:
                    skillTitle.text  = "Party Time";
                    break;
                
                case 2000:
                    skillTitle.text = "Guided rocket";
                    break;
                
                case 2001:
                    skillTitle.text = "Friend sheep";
                    break;
                
                case 2002:
                    skillTitle.text = "Bear Brylls";
                    break;
                
            }
            
        }

        private void Set_Skill_desc()
        {
            switch (Calc_Index.Get_Avatar_Num(target_index))
            {
                case 1006:
                    skilldesc.text = "Party gun has a 15% chance of shooting gem on the field every turn";
                    break;
                
                case 2000:
                    skilldesc.text = "Fire toy missiles in the 5x5 area to remove all boxes.";
                    break;
                
                case 2001:
                    skilldesc.text = "The sheep appearing in the pajamas' dreams appear in the field and break boxes.";
                    break;
                
                case 2002:
                    skilldesc.text = "The soles of the teddy bear's feet have the power to blow off half of all boxes.";
                    break;
                
            }
            
            
        }

        public void OnClick_Exit()
        {
            obj.SetActive(false);
        }
    }
}
