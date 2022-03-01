using System;
using System.Collections;
using System.Collections.Generic;
using Avatar;
using Progetile;
using Skin;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby{
    public class Charater_Info : MonoBehaviour
    {
        public Animator charater_ani; // 캐릭터 애니메이션 연결 var
        public Image ball_img;
        private void Awake()
        {
            int index;
            EquippedDAO data = new EquippedDAO();
            index = data.Get_Equipped_index();// index 정보 가져옴 
            Set_charater_UI(index);
            BallDAO data_ball = new BallDAO();
            index = data_ball.Get_BallEquipped_Data();
            Set_Ball(index);
            // 장착된 캐릭터 정보를 불러옴 
        }

        /// <summary>
        ///  장착된 번호를 기준으로 캐릭터 정보를 결정해주는 함수 
        /// </summary>
        /// <param name="index"></param>
        public void Set_charater_UI(int index)
        {
            charater_ani.runtimeAnimatorController = Set_Avatar_UI.Set_Charater_GameObject(index);
            SpriteRenderer shadow = charater_ani.gameObject.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
            switch (index)
            {
                default:
                    shadow.sprite = Resources.Load<Sprite>("Charater/Shadow/Lobby_Shadow");
                    break;
                
                case 1002:
                    shadow.sprite = Resources.Load<Sprite>("Charater/Shadow/Lobby_Shadow_M");
                    break;

                case 2003:
                    shadow.sprite = Resources.Load<Sprite>("Charater/Shadow/Lobby_Shadow_L");
                    break;
            }
        }

        public void Set_Ball(int index) => ball_img.sprite = Set_Avatar_UI.Set_Ball_Img(index);
        


    }
}
