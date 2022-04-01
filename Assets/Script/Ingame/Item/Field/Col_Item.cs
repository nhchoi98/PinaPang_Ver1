using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Block;
using Ingame;
using DG.Tweening;
using UnityEngine.UI;
using Pinata;

namespace Item
{
    public class Col_Item : MonoBehaviour, IItem_Data
    {
        public LocateBox locateBox;
        private List<IBox> AttackList = new List<IBox>(); // 원래는 private
        private Transform tr_pinata;
        private Pinata_Down attack_pinata;
        private bool is_pinata = false;
        public Determine_Destroy _DetermineDestroy;
        public AudioSource[] sound;
        public Animator animator;
        public bool both;
        
        [Header("Hit")]
        public Image hitAnimation;
        private Sequence comboSequence;
        public Image itemImg;
        public Sprite hitSprite;
        public Sprite normalImg;
        private bool flag;
        public bool isCol;

        [Header("Pos_Data")] 
        public int row;
        public ItemType type;

        private void Start()
        {
            comboSequence = DOTween.Sequence()
                .SetAutoKill(false)
                .Append(DOTween.ToAlpha(() => hitAnimation.color, alpha => hitAnimation.color = alpha, 1f, 0.1f))
                .Append(DOTween.ToAlpha(() => hitAnimation.color, alpha => hitAnimation.color = alpha, 0f, 0.1f))
                .OnComplete(ItemDisable);
            Update_List();
        }

        public void Update_List()
        {
            if(AttackList.Count!=0)
                AttackList.Clear(); // 어택 리스트 초기화ㅣ 
            
            for (int i = 0; i < locateBox.boxGroup.childCount; i++)
            {
                Transform TR = locateBox.boxGroup.GetChild(i);
                if (Mathf.Abs(TR.position.x - this.transform.position.x) < 10)
                {
                    if (TR.gameObject.CompareTag("Box"))
                        AttackList.Add(TR.gameObject.GetComponent<IBox>());
                }
            }
            
            // 피냐타를 공격대상에 추가하기 
            if (locateBox.pinataGroup.childCount != 0)
            {
                tr_pinata = locateBox.pinataGroup.GetChild(0).GetChild(0).GetChild(1);
                attack_pinata = locateBox.pinataGroup.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Pinata_Down>();
                is_pinata = true;
            }

            Calc_HitAni_Position();
        }

        /// <summary>
        /// 아이템에 부딪힘이 감지되면, 리스트에 있는 대상들을 공격하는 스크립트 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        private IEnumerator OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == 3)
            {
                _DetermineDestroy.Activated();
                Object_Pooling();
                for (int i = 0; i < AttackList.Count; i++)
                {
                    if(AttackList[i]!=null)
                        AttackList[i].Item_Attack();
                    
                    else
                        AttackList.RemoveAt(i);
                }
                Object_Pooling();
                if(is_pinata)
                    Pinata_Attack();
                Sound();
            }
            yield return null;
        }
        
        public void StartAni_End()
        {
            animator.enabled = false;
        }
        
        /// <summary>
        /// 레이저에 맞음을 표시해주는 애니메이션 
        /// </summary>
        private void Object_Pooling()
        {
            if (flag) return;
            flag = true;
            itemImg.sprite = hitSprite;
            comboSequence.Restart();
        }

        private void Calc_HitAni_Position()
        {
            hitAnimation.gameObject.transform.localPosition =
                    new Vector3(0f, (-transform.position.y)+155f, 0f);
            
        }

        private void ItemDisable()
        {
            itemImg.sprite = normalImg;
            flag = false;
        }

        private void Pinata_Attack()
        { 
            float target_value = 0;
            if (attack_pinata == null)
            {
                is_pinata = false;
                return;
            }

            switch (locateBox.pinata_num)
            {
                default:
                    target_value = 135f + 15f;
                    break;

                case 1:
                    target_value = 225f+15f;
                    break;
                
                case 6:
                    target_value = 202.6f+15f;
                    break;

            }
            
            if (Mathf.Abs(tr_pinata.position.x - transform.position.x)<target_value)
                attack_pinata.Item_Hit();
            
        }

        private void Sound()
        {
            if (sound.Length == 0)
                return;
            
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (!sound[i].isPlaying)
                    {
                        sound[i].Play();
                        return;
                    }
                }
            }
        }

        public void Set_Animation_Down()
        {
            comboSequence.Kill();
            Destroy(this.gameObject);
        }

        public void Set_Row(int value)
        {
            if (row == -1)
                ++row;
            else
                row += value;
        }

        public int Get_Row()
        {
            return row;
        }

        public Vector2 Get_Pos()
        {
            return transform.position;
        }

        /// <summary>
        /// 공격 대상들을 초기화 시켜줌 
        /// </summary>
        public void Set_Load()
        {
            Update_List();
        }

        public void Set_Type(ItemType type)
        {
            this.type = type;
        }

        public ItemType Get_Type()
        {
            return type;
        }
    }
}
