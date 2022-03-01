
using Manager;
using UnityEngine;
using DG.Tweening;

namespace Ingame
{
    public class GameOver_RewardBg : MonoBehaviour
    {
        public GameOver GameOver;
        void Start()
        {
            transform.DOScale(new Vector3(1f, 1f, 1f), 1f)
                .SetEase(Ease.OutBounce)
                .OnComplete(() =>
                {
                    GameOver.StartCoroutine(GameOver.Set_Reward_List());
                });
        }
    }
}
