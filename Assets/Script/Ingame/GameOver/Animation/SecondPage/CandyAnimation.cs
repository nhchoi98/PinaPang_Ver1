
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Ingame
{
    public class CandyAnimation : MonoBehaviour
    {
        public Vector2 targetPos;
        public event EventHandler arrive;
        private Sequence openSequence;
        public SpriteRenderer candyImg;
        public float target_time_const;
        
        void Start()
        {
            object o = new object();
            EventArgs arge = new EventArgs();
            openSequence = DOTween.Sequence()
                .Append(DOTween.ToAlpha(() => candyImg.color, alpha => candyImg.color = alpha, 1f, 0.1f * target_time_const))
                .Append(transform.DOMove(targetPos, 1.2f*target_time_const)
                    .SetEase(Ease.InSine))
                .Insert(0f,transform.DORotate(new Vector3(0f,0f,180f),1.1f * target_time_const))
                .Insert(1f*target_time_const ,DOTween.ToAlpha(() => candyImg.color, alpha => candyImg.color = alpha, 0f, 0.5f*target_time_const)
                .OnComplete(() => {arrive.Invoke(o,arge);}));
        }
    }
}
