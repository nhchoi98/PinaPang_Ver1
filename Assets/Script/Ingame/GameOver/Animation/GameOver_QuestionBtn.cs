
using UnityEngine;
using DG.Tweening;

namespace Ingame
{
    public class GameOver_QuestionBtn : MonoBehaviour
    {
        public GameObject panel;
        private bool isOpen = false;
        private bool activate = false;

        public void OnClick_BonusBtn()
        {
            if (activate)
                return;
            
            else
            {
                if (!isOpen)
                {
                    activate = true;
                    panel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.4f)
                        .SetEase(Ease.OutBounce)
                        .OnComplete(() => 
                            { isOpen = true; activate = false; });
                }

                else
                {
                    activate = true;
                    panel.transform.DOScale(new Vector3(0f, 0f, 1f), 0.4f)
                        .SetEase(Ease.OutExpo)
                        .OnComplete(() => 
                            { isOpen = false; activate = false; });
                    
                }
            }
        }
    }
}
