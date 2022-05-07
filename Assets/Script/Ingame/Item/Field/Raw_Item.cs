
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Block;
using Ingame;
using UnityEngine.UI;
using DG.Tweening;
using Pinata;

namespace Item
{
    public class Raw_Item : MonoBehaviour, IItem_Data
    {
        public LocateBox locateBox;
        
        [Header("Field_Obj")]
        private List<IBox> AttackList = new List<IBox>(); // 원래는 private
        private Pinata_Down attack_pinata;
        private bool is_pinata = false;
        private Transform tr_pinata;
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

        private ItemType type;
        private void Start()
        {
            comboSequence = DOTween.Sequence()
                .SetAutoKill(false)
                .Append(DOTween.ToAlpha(() => hitAnimation.color, alpha => hitAnimation.color = alpha, 1f, 0.1f))
                .Append(DOTween.ToAlpha(() => hitAnimation.color, alpha => hitAnimation.color = alpha, 0f, 0.1f))
                .OnComplete(ItemDisable);
            Calc_HitAni_Position();
        }

        public void Update_List()
        {
            for (int i = 0; i < locateBox.boxGroup.childCount; i++)
            {
                Transform TR = locateBox.boxGroup.GetChild(i);
                if (Mathf.Abs(TR.position.y - this.transform.position.y) < 10)
                {
                    if (TR.gameObject.CompareTag("Box"))
                        AttackList.Add(TR.gameObject.GetComponent<IBox>());
                    
                }
                
            }
            
            // 피냐타를 공격 대상에 추가 
            if (locateBox.pinataGroup.childCount != 0)
            {
                Transform TR = locateBox.pinataGroup.GetChild(0).GetChild(0).GetChild(1);
                tr_pinata = TR;
                attack_pinata =  locateBox.pinataGroup.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Pinata_Down>();
                is_pinata = true;
            }
        }
        
        
        private void Calc_HitAni_Position()
        {
            hitAnimation.gameObject.transform.localPosition =
                    new Vector3(-transform.position.x, 0f, 0f);
        }

        private void ItemDisable()
        {
            itemImg.sprite = normalImg;
            flag = false;
        }
        public void StartAni_End()
        {
            animator.enabled = false;
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
                if(is_pinata)
                    Pinata_Attack();
                
                Sound();
            }
            yield return null;
        }
        
        private void Object_Pooling()
        {
            if (flag) return;
            flag = true;
            itemImg.sprite = hitSprite;
            comboSequence.Restart();

        }
        
        private void Pinata_Attack()
        {
            int target_index = locateBox.pinata_num;
            float target;
            switch (target_index)
            {
                default:
                    target = 135f + 15f;
                    break;

                case 0:
                    target = 111.5f;
                    break;
                
                case 2:
                    target = 111.5f + 15f;
                    break;

            }
            
            if (attack_pinata != null)
                if (Mathf.Abs(tr_pinata.position.y - this.transform.position.y) < target) attack_pinata.Item_Hit();


            else {
                is_pinata = false;
                    return; }
            
        }
        
        private void Sound()
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
        
        public void Set_Animation_Down()
        {
            comboSequence.Kill();
            Destroy(this.gameObject);
        }
        
        public void Set_Row(int value)
        {
            if (value == -1)
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
            for (int i = 0; i < locateBox.boxGroup.childCount; i++)
            {
                Transform TR = locateBox.boxGroup.GetChild(i);
                if (Mathf.Abs(TR.position.y - this.transform.position.y) < 10)
                {
                    if (TR.gameObject.CompareTag("Box"))
                        AttackList.Add(TR.gameObject.GetComponent<IBox>());
                    
                }
                
            }
            
            // 피냐타를 공격 대상에 추가 
            if (locateBox.pinataGroup.childCount != 0)
            {
                Transform TR = locateBox.pinataGroup.GetChild(0).GetChild(0).GetChild(1);
                tr_pinata = TR;
                attack_pinata =  locateBox.pinataGroup.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Pinata_Down>();
                is_pinata = true;
            }
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
