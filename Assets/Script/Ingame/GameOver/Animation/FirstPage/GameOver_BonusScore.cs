using UnityEngine;
using DG.Tweening;
using Manager;

namespace Ingame
{
    public class GameOver_BonusScore : MonoBehaviour
    {
        public GameObject reward;
        private Sequence openSequence;
        [SerializeField] private GameOver _gameOver;
        
        void Start()
        {
            openSequence = DOTween.Sequence()
                .Append(transform.DOScaleX(1f, 0.3f)
                    .SetEase(Ease.InOutBounce))
                .Join(transform.DOScaleY(1f, 0.3f))
                .OnComplete(() => { _gameOver.StartCoroutine(_gameOver.Set_Score_root()); });
        }

    }
}
