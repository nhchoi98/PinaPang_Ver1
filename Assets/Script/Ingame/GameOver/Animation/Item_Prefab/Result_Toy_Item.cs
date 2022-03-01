
using UnityEngine;
using DG.Tweening;

namespace Ingame
{
    public class Result_Toy_Item : MonoBehaviour
    {
        private Sequence openSequence;
        void Start()
        {
            openSequence = DOTween.Sequence()
                .Append(transform.DOScale(1f, 0.2f)
                    .SetEase(Ease.InOutBounce))
                .OnComplete(() => { transform.DOKill(); });
        }

    }
}
