using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame
{
    public class Combo_Pooling : MonoBehaviour, ICombo_Pool
    {
        [SerializeField]
        public Transform Object_Pool, Activating_pool;
        public Canvas canvas;
        public Text upperCombo, lowerCombo;
        private Sequence comboSequence;
        private Color firstColor;

        private void Awake()
        {
            firstColor = new Color(1f, 1f, 1f, 1f);
            comboSequence = DOTween.Sequence()
                .SetAutoKill(false)
                .Append(DOTween.ToAlpha(() => upperCombo.color, alpha => upperCombo.color = alpha, 0f, 0.2f))
                .Join(DOTween.ToAlpha(() => lowerCombo.color, alpha => lowerCombo.color = alpha, 0f, 0.2f))
                .OnComplete(Set_Disable);
        }

        public void CanvasOn()
        {
            if (Activating_pool != null)
            {
                canvas.enabled = true;
                this.transform.SetParent(Activating_pool);
                transform.SetAsFirstSibling();
                StartCoroutine(Combo_ScoreAnimation());
            }
            
        }

        public void Set_Disable()
        {
            this.gameObject.transform.SetParent(Object_Pool);
            transform.SetAsLastSibling();
            upperCombo.color = firstColor;
            lowerCombo.color = firstColor;
            canvas.enabled = false;
        }

        IEnumerator Combo_ScoreAnimation()
        {
            Vector2 targetPos = new Vector2(0f + this.transform.position.x, 80f + this.transform.position.y);
            transform.DOMove(targetPos, 1f)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    comboSequence.Restart();
                    transform.DOKill();
                });
            
            // 좀 떠있다가 꺼지게 만들기
            // 색깔도 변하게 만들기
            yield return null;
        }
    }
}
