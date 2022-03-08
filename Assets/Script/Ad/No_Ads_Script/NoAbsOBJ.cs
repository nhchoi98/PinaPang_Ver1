using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class NoAbsOBJ : MonoBehaviour
{
    public Sequence hitSequence;
    public Image image;
    public Text text;
    void Start()
    {
        StartCoroutine(No_Ads_Info());
    }

    IEnumerator No_Ads_Info()
    {
        Vector2 Target_Pos = new Vector2(0f, 700f);
        hitSequence = DOTween.Sequence()
            .Append(transform.DOMove(Target_Pos, 1.8f)
                .SetEase(Ease.OutQuint)
                .OnComplete(() => Destroy(this.gameObject)))
            .Insert(1.5f, DOTween.ToAlpha(() => image.color, alpha => image.color = alpha, 0f, 0.25f))
            .Insert(1.5f, DOTween.ToAlpha(() => text.color, alpha => text.color = alpha, 0f, 0.25f));


        yield return null;

    }
}
