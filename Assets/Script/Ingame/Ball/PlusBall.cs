using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Manager;

public class PlusBall : MonoBehaviour
{
    [SerializeField]
    private Transform  DiePool;
    public AudioSource Ball_HIT;
    public GameObject Plusball_destroy;
    private bool Is_hit = false;
    private void Awake()
    {
        SoundManager SM = GameObject.FindWithTag("SM").GetComponent<SoundManager>();
        Ball_HIT = SM.PlusBall;
    }

    private void OnEnable()
    {
        Is_hit = false;
    }

    IEnumerator OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 8)
            yield break;
        
        Ball_HIT.Play();
        if (!Is_hit)
        {
            Is_hit = true;
            transform.GetChild(0).gameObject.SetActive(false);
            gameObject.GetComponent<Animator>().enabled = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            transform.localScale = new Vector2(1f, 1f);
            StartCoroutine(Plus_Animation());
            Instantiate(Plusball_destroy, transform.position, Quaternion.identity);
        }
        
        yield return null;
    }
    IEnumerator Plus_Animation()
    {
        Vector2 TargetPos = new Vector2(transform.position.x, -701.02f);
        transform.SetParent(DiePool);
        while (true)
        {
            if (Mathf.Abs(TargetPos.y-transform.position.y) <0.1f)
                break;
            
            transform.position = Vector2.MoveTowards(transform.position, TargetPos, 3500f * Time.deltaTime);
            yield return null;
        }
    }
    
}
