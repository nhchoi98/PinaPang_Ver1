
using UnityEngine.UI;
using UnityEngine;
using Data;

namespace Lobby
{
    
    public class Lobby_Playerinfo : MonoBehaviour
    {

        [Header("Commodity_UI")] [SerializeField]
        private Text Gem, Candy;
        
        public AudioSource Click;

        private void Awake()
        {
            Time.timeScale = 1f;
        }

        private void OnEnable()
        {
            this.Gem.text = string.Format("{0:#,0}", Playerdata_DAO.Player_Gem()); 
            this.Candy.text = string.Format("{0:#,0}", Playerdata_DAO.Player_Candy());
            
        }

        public void Set_Gem_Text()
        {
            this.Gem.text =string.Format("{0:#,0}", Playerdata_DAO.Player_Gem()); 
            
        }
        
    }
}
