using DG.Tweening;
using UnityEngine;

namespace Ingame
{
    public class Lobby_BestScoreObj : MonoBehaviour
    {
        private Sequence openSequence;
        // Start is called before the first frame update
        void Start()
        {
            openSequence = DOTween.Sequence()
                .Append(transform.DOScaleX(0.1f, 0.3f)
                    .SetEase(Ease.InOutBounce))
                .Join(transform.DOScaleY(1f, 0.3f))
                .Insert(0.3f,transform.DOScaleX(1f,0.7f).SetEase(Ease.OutBounce))
                .OnComplete(() => { this.gameObject.GetComponent<Lobby_BestScoreObj>().enabled = false;});
        }


    }
}
