using DG.Tweening;
using UnityEngine;

namespace Ingame
{
    public class GameOver_BestScore : MonoBehaviour
    {
        public GameObject bestScore_Text;
        void Start()
        {
            transform.DOScale(new Vector3(1f, 1f, 1f), 0.8f)
                .OnComplete(() =>
                {
                    bestScore_Text.SetActive(true); transform.DOKill();});
        }
    }
}
