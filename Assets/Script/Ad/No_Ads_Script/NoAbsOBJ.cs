using System.Collections;
using UnityEngine;
using DG.Tweening;

public class NoAbsOBJ : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(No_Ads_Info());
    }

    IEnumerator No_Ads_Info()
    {
        Vector2 Target_Pos = new Vector2(0f, 400f);
        transform.DOMove(Target_Pos, 2.5f)
            .SetEase(Ease.OutQuint);
        
        Destroy(this.gameObject);
        yield return null;

    }
}
