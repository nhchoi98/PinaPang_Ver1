using System;
using System.Collections;
using System.Collections.Generic;
using Block;
using UnityEngine;
using UnityEngine.UI;
using Manager;
using Progetile;

public class PlusBall : MonoBehaviour, IBox
{
    [SerializeField]
    public Transform  DiePool;
    public AudioSource Ball_HIT;
    public GameObject Plusball_destroy;
    private bool Is_hit = false;
    private int row;
    private void Awake()
    {
        SoundManager SM = GameObject.FindWithTag("SM").GetComponent<SoundManager>();
        Ball_HIT = SM.PlusBall;
    }

    private void OnEnable()
    {
        row = 0;
        Is_hit = false;
    }

    IEnumerator OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 8)
            yield break;
        
        Attack(Vector2.zero);
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

    #region IBOX
    public void Attack(Vector2 pos, bool is_item = false)
    {
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
    }

    public void Item_Attack()
    {
        return; // 아이템이 공격해도 아무일도 일어나지 않음 
    }

    public void Set_HP(int HP)
    {
        return;
    }

    public void Set_Type(blocktype type)
    {
        return;
    }

    public void Set_Candle(int candle_type)
    {
        return;
    }

    public void Set_ColorType(int colorType, Transform particle)
    {
        return;
    }

    public void Set_Event(IBox.event_delegate _event, ref Progetile_Particle script)
    {
        return;
    }

    public int Get_Candle()
    {
        return -1;
    }

    public int Get_HP()
    {
        return 0;
    }

    public int whichRow()
    {
        return row;
    }

    public void Set_Row(int value)
    {
        if (value != -1)
            row += value;

        else
            ++row;
    }

    public blocktype Get_Type()
    {
        return blocktype.PLUSBALL;
    }

    public int Get_Row()
    {
        return row;
    }
    
    public Vector2 Get_Position()
    {
        return this.transform.position;
    }
    

    #endregion
}
