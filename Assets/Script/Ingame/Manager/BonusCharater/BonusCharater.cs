using System.Collections;
using System.Collections.Generic;
using Charater;
using Fire;
using Ingame;
using UnityEngine;
using Score;

namespace Manager
{
    public class BonusCharater : MonoBehaviour
    {
        [Header("Bonus")]
        private bool[] _Bonus_Check = new bool[6]; // 보너스 글자 모음 여부를 체크하는 변수 
        private bool[] _Bonus_Respawn = new bool[6];
        public Transform _Bonus_TR;
        public GameObject charater_obj;
        private bool bonus_check;

        [Header("UI")] public GameObject Bonus_Time;

        [SerializeField] private LocateBox _locateBox;
        
        /// <summary>
        /// 글자를 획득하면 호출되는 함수 
        /// </summary>
        /// <param name="type"></param>
        public void Set_Bonus_Charater(int type, Vector2 pos, bool is_destroy)
        {
            if (!_locateBox.is_pinata())
            {
                GameObject obj;
                Charater_Flight obj_script;
                if (is_destroy)
                {
                    obj = Instantiate(charater_obj, pos, Quaternion.identity);
                    obj_script = obj.GetComponent<Charater_Flight>();
                    obj_script.index = type;
                    obj_script.target_obj = _Bonus_TR.GetChild(type).gameObject;
                    _Bonus_Check[type] = true;
                }

                
                else
                {
                    _Bonus_Respawn[type] = true;
                }
                
            }
        }

        public void Set_Bonus_Charater_init()
        {
            for (int i = 0; i < _Bonus_Check.Length; i++)
            {
                _Bonus_Check[i] = false;
                _Bonus_Respawn[i] = false;
            }
        }
        
        public bool Get_Bonus_Charater(int type)
        {
            return _Bonus_Respawn[type];
        }
        
        /// <summary>
        /// 피냐타가 파괴되면 호출되는 함수 
        /// </summary>
        public void Bonus_Extinguish()
        {
            bonus_check = false;
            for (int i = 0; i < _Bonus_TR.childCount; i++)
                _Bonus_TR.GetChild(i).gameObject.GetComponent<Flame_Down>().Extinquish();
            
        }


        #region Get

        /// <summary>
        /// 피냐타 소환 여부를 리턴하는 함수 
        /// </summary>
        /// <returns></returns>
        public bool Get_BonusCheck()
        {
            if (bonus_check)
            {
                Bonus_Time.SetActive(true);
                Set_Bonus_Charater_init();
            }
            
            return bonus_check;
        }

        public void Do_BonusCheck()
        {            
            for (int i = 0; i < _Bonus_Check.Length; i ++ )
            {
                if (!_Bonus_Check[i])
                {
                    bonus_check = false;
                    return;
                }
            }
            bonus_check = true;
        }
        

        #endregion
        
    }
    
}
