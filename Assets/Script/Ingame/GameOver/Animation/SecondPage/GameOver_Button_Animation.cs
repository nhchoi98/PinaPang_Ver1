
using System;
using UnityEngine;
using DG.Tweening;

namespace Ingame
{
    public class GameOver_Button_Animation : MonoBehaviour
    {
        private Sequence openSequence;
        public void Start()
        {
            openSequence = DOTween.Sequence()
                .Append(transform.DOScaleX(0.1f, 0.4f))
                .Insert(0.4f,transform.DOScaleX(1f,0.7f).SetEase(Ease.OutBounce))
                .Join(transform.DOScaleY(1f, 0.2f))
                .OnComplete(() => { });
        }


    }
}
