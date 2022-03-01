using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Ingame
{
    public class Result_Toy_New : MonoBehaviour
    {
        private Sequence openSequence;
        void Start()
        {
            openSequence = DOTween.Sequence()
                .SetAutoKill(false)
                .Append(transform.DOScale(0.8f, 0.5f)
                    .SetEase(Ease.InBounce))
                .Append(transform.DOScale(1.2f, 0.5f)
                    .SetEase(Ease.OutBounce))
                .Append(transform.DOScale(0.8f, 0.5f)
                    .SetEase(Ease.InBounce))
                .Append(transform.DOScale(1.2f, 0.5f)
                    .SetEase(Ease.OutBounce))
                .OnComplete(() => { StartCoroutine(Wait()); });
        }


        IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.5f);
            openSequence.Restart();
        }

        private void OnDisable()
        {
            openSequence.Kill();
        }
    
    }
}
