
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame
{
        public class GameOver_CandyResult : MonoBehaviour
        {
                private float candyConst;
                private int targetNum;
                private int candyNum= 0;
                private int candy_Target;
                private int realTarget;
                private int candyNum_Text;
                
                public Transform candyTR;
                private int arriveCount =0;
                public Text candyText;

                [Header("Button")] public GameObject homeBtn, retryBtn;
                [SerializeField] private GameOver_JarAnimation _jarAnimation;
                public GameObject jarObj;
                public void Set_CandyConst(float _const)
                {
                        candyConst = _const;
                }

                public void Set_CandyNum(int num)
                {
                        candy_Target = num;
                }

                public void Start_SecondPage()
                {
                        Transform tr;
                        if (candy_Target / candyConst == 0)
                        {
                                candyText.text = string.Format("{0:#,0}", candy_Target);
                                StartCoroutine(Open_Button());
                        }

                        else
                        {
                                if (candy_Target / candyConst > 10)
                                {
                                        realTarget = 10;
                                        targetNum = 10;
                                }

                                else
                                {
                                        targetNum = (int) (candy_Target / candyConst);
                                        realTarget = targetNum;
                                }

                                _jarAnimation.isCount = true;
                        }
                        
                        jarObj.SetActive(true);
                }
                
                private void set_text(object sender, EventArgs e)
                {
                        candyNum_Text += (int)candyConst;
                        candyText.text = string.Format("{0:#,0}", candyNum_Text);
                        ++arriveCount;
                        // 애니메이션 트리거 
                        if (arriveCount == realTarget)
                        {
                                arriveCount = 0;
                                if (candy_Target != candyNum)
                                {
                                        candyText.text = string.Format("{0:#,0}", candy_Target);
                                        candyNum = candy_Target;
                                }

                                StartCoroutine(Open_Button());
                        }
                }

                IEnumerator Open_Button()
                {
                        retryBtn.SetActive(true);
                        yield return new WaitForSeconds(1f);
                        homeBtn.SetActive(true);
                }

                public IEnumerator candyDown()
                {
                        Transform tr;
                        for (int i = 0; i <  targetNum; i++)
                        {
                                tr = candyTR.GetChild(i);
                                tr.GetComponent<CandyAnimation>().arrive += set_text;
                                candyTR.GetChild(i).gameObject.SetActive(true);
                        }

                        yield return null;
                }


        }
}
