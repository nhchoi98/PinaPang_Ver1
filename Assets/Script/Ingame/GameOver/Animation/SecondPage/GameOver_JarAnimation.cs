using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame
{
    public class GameOver_JarAnimation : MonoBehaviour
    {
        [SerializeField] private GameOver_CandyResult _candyResult;
        public GameObject quantity;
        public bool isCount = false;
        public bool isPlaying;
        public bool doubleInput;
        private Sequence openSequence;
        void Start()
        {
            transform.DOScale(new Vector3(0.6f, 0.6f, 1f), 0.8f)
                .SetEase(Ease.OutBounce)
                .OnComplete(() => {
                    quantity.SetActive(true);
                    if (isCount)
                    {
                        _candyResult.StartCoroutine(_candyResult.candyDown());

                        openSequence = DOTween.Sequence()
                            .SetDelay(0.6f)
                            .Append(transform.DOScale(new Vector2(0.7f, 0.5f), 0.1f)
                                .SetEase(Ease.OutBounce))
                            .Append(transform.DOScale(new Vector2(0.5f, 0.7f), 0.1f)
                                .SetEase(Ease.OutBounce))
                            .Append(transform.DOScale(new Vector2(0.7f, 0.55f), 0.15f)
                                .SetEase(Ease.OutBounce))
                            .Append(transform.DOScale(new Vector2(0.55f, 0.65f), 0.15f)
                                .SetEase(Ease.OutBounce))
                            .Append(transform.DOScale(new Vector2(0.67f, 0.54f), 0.25f)
                                .SetEase(Ease.OutBounce))
                            .Append(transform.DOScale(new Vector2(0.6f, 0.6f), 0.35f)
                                .SetEase(Ease.OutBounce));
                        
                    }


                    transform.DOKill();
                });
            
            
        }
    }
}
