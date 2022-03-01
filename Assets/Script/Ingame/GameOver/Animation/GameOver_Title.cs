
using UnityEngine;
using DG.Tweening;

namespace Ingame
{
    public class GameOver_Title : MonoBehaviour
    {
        public GameObject mainScore;
        void Start()
        {
            transform.DOScale(new Vector3(4f, 4f, 1f), 1f)
                .SetEase(Ease.OutBounce)
                .OnComplete(() =>
                {
                    mainScore.SetActive(true);
                    transform.DOKill();
                    this.gameObject.GetComponent<GameOver_Title>().enabled = false; 
                    
                });
        }
    }
}
