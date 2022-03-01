using System;
using System.Collections;
using Collection;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class Gem_Goods : MonoBehaviour
    {
        [Header("UI")] 
        [SerializeField] private Text gemText;
        [SerializeField] private Animator gem_animator;
        public GameObject gem_obj;
        public Transform discount_TR;
        private int gemconst;
        private int gem;
        public AudioSource gemFlying_Sound;

        
        private void OnEnable()
        {
            gem = Playerdata_DAO.Player_Gem();
            gemText.text = String.Format("{0:#,0}", gem);
        }

        public void Set_Gem()
        {
            gem = Playerdata_DAO.Player_Gem();
            gemText.text = String.Format("{0:#,0}", gem);
        }

        #region Merchandise_Gem

        public void Get_Merchandise_Gem(int index)
        {
            Target_gem(index);
        }
        
        #endregion

        private void Target_gem(int index)
        {
            int gem;
            gemFlying_Sound.Play();
            switch (index)
            {
                default:
                    gem = 0;
                    break;

                case 0:
                    gem  = Playerdata_DAO.Player_Gem();
                    gemconst = 1;
                    Playerdata_DAO.Set_Player_Gem(5);
                    gem = 5;
                    break;

                case 1:
                    gem  = Playerdata_DAO.Player_Gem();
                    gemconst = 1;
                    Playerdata_DAO.Set_Player_Gem(10);
                    gem = 10;
                    break;

                case 2:
                    gem  = Playerdata_DAO.Player_Gem();
                    gemconst = 1;
                    Playerdata_DAO.Set_Player_Gem(15);
                    gem = 15;
                    break;
                
                // 여기까지가 광고 시청에 의한 젬 획득 개수 
                case 10:
                    if (PlayerPrefs.GetInt("Gem_0", 0) == 0)
                    {
                        discount_TR.GetChild(0).GetChild(3).gameObject.SetActive(false);
                        gemconst = 1;
                        gem = 60;
                    }

                    else
                    {
                        gemconst = 2;
                        gem = 30;
                    }

                    break;

                case 11:
                    if (PlayerPrefs.GetInt("Gem_1", 0) == 0)
                    {
                        discount_TR.GetChild(1).GetChild(3).gameObject.SetActive(false);
                        gemconst = 2;
                        gem = 140;
                    }

                    else
                    {
                        gemconst = 2;
                        gem = 70;
                    }
                    break;

                case 12:
                    if (PlayerPrefs.GetInt("Gem_2", 0) == 0)
                    {
                        discount_TR.GetChild(2).GetChild(3).gameObject.SetActive(false);
                        gemconst = 15;
                        gem = 600;
                    }

                    else
                    {
                        gemconst = 20;
                        gem = 300;
                    }

                    break;

                case 13:
                    if (PlayerPrefs.GetInt("Gem_3", 0) == 0)
                    {
                        discount_TR.GetChild(3).GetChild(3).gameObject.SetActive(false);
                        gemconst = 40;
                        gem = 1400;
                    }

                    else
                    {
                        gemconst = 20;
                        gem = 700;
                    }
                    break;

                case 14:
                    if (PlayerPrefs.GetInt("Gem_4", 0) == 0)
                    {
                        discount_TR.GetChild(4).GetChild(3).gameObject.SetActive(false);
                        PlayerPrefs.SetInt("Gem_4", 1);
                        gemconst = 100;
                        gem = 3000;
                    }

                    else
                    {
                        gemconst = 50;
                        gem = 1500;
                    }

                    break;

                case 15:
                    if (PlayerPrefs.GetInt("Gem_5", 0) == 0)
                    {
                        discount_TR.GetChild(5).GetChild(3).gameObject.SetActive(false);
                        PlayerPrefs.SetInt("Gem_5", 1);
                        gemconst = 100;
                        gem = 6800;
                    }

                    else
                    {
                        gemconst = 100;
                        gem = 3400;
                    }
                    break;

                // 여기까지가 젬 상품에 의한 젬 획득 개수 
                
                case 100:
                    gemconst = 4;
                    gem = 80;
                    break;

                case 101:
                    gemconst = 8;
                    gem = 200;
                    break;

            }
            Playerdata_DAO.Set_Player_Gem(gem);
            StartCoroutine(Get_Gem(gem/gemconst));
            // 젬 획득 연출 
        }
        
        public IEnumerator Get_Gem(int num)
        {
            for (int i = 0; i < num; i++)
            {
                Gem_Flying script;
                Vector2 start_pos = Vector2.zero;
                GameObject obj = Instantiate(gem_obj, start_pos,Quaternion.identity); // 잼 획득 연출 넣기 
                obj.transform.localScale = new Vector2(70f, 70f);
                script = obj.GetComponent<Gem_Flying>();
                script.gem_animator = this.gem_animator;
                script.start_pos = start_pos;
                script.gem_text = this.gemText;
                script.arrive += set_text;
                script.Target_Pos = new Vector2(-439f, 861f);
            }
            yield return null;
        }
        
        
        private void set_text(object sender, EventArgs e)
        {
            gem += gemconst;
            gemText.text = string.Format("{0:#,0}", gem);
        }
        
    }
}
