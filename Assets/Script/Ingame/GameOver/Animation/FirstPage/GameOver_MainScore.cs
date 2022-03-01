using Manager;
using UnityEngine;
using DG.Tweening;

namespace Ingame
{
    public class GameOver_MainScore : MonoBehaviour
    {
        [SerializeField] private GameOver _gameOver;
        private Sequence openSequence;

        void Start()
        {
    
            openSequence = DOTween.Sequence()
                .Append(transform.DOScale(1f, 0.8f)
                    .SetEase(Ease.OutBounce))
                .OnComplete(() =>
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    _gameOver.StartCoroutine(_gameOver.Set_Score_root()); 
                });
        }


    }
}
