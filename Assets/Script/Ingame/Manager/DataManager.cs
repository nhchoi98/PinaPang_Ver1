
using UnityEngine;
using UnityEngine.UI;
using Badge;
using Challenge;
using Skin;

namespace Data {
    /// <summary>
    /// 이 매니저 클래스는 사용자의 데이터의 입출력을 관리하는 함수이다. 
    /// </summary>
    public class DataManager : MonoBehaviour
    {

        [SerializeField] private QuestManager _questManager;
        
        // 아이템 지속시간 데이터 변수 
        [Header("Commodity_UI")]
        [SerializeField]
        private Text Collection;
        private int Which_Theme = 1;
        public Text gem_text;
        public int item_duration_const { get; private set; } // item duration에 대한 코드 
        private bool is_extra_life;
        /// <summary>
        /// 시작시 모든 정보를 초기화 해오고, 읽어오고, 이걸 UI에 반영해줌 
        /// </summary>
        private void Awake()
        {
            gem_text.text = string.Format("{0:#,0}", Playerdata_DAO.Player_Gem());
            item_duration_const = AbilityDAO.Get_Duration(); // 나중에 vo로 읽어와서 상수값으로 여기서 바꾸어 주어야함 
            is_extra_life = AbilityDAO.Get_Extra_life();
            // 여기에 circle, box const도 들어가야함. 
            // life const도 들어가야함 
        }

        #region Get_Player_Info
        
        /// <summary>
        /// 추가 생명 여부를 리턴하는 함수
        /// </summary>
        /// <returns></returns>
        public bool Get_Extra_Life()
        {
            if (is_extra_life)
            {
                _questManager.Set_Revive();
                is_extra_life = false;
                return true;
            }
            return false;
        }
        
        #endregion
        
    }
}
