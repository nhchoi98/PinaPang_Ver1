using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using Block;
using Ingame;

public class Momentum_calc : MonoBehaviour
{

    [SerializeField]
    private SpringJoint2D _SpringJoint;
    [SerializeField]
    private Transform pinata;
    private float Pre_momentum = 0f;
    private float Now_momentum = 0f;
    Rigidbody2D rigid_col;
    private bool is_first = true ;
    private IMediator _mediator;
    private int down_count = 0;

    private void Start()
    {
        _mediator = GameObject.FindWithTag("GameController").GetComponent<IMediator>();
       
    }
    
    private IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        
        switch (collision.gameObject.layer)
        {
            case 3:
                rigid_col = collision.gameObject.GetComponent<Rigidbody2D>();
                Vector2 vel = rigid_col.velocity;
                Now_momentum = vel.magnitude;
                
                if(vel.magnitude == 0f)
                    rigid_col.velocity = Vector2.zero;
                    
                else
                    rigid_col.velocity = ((1350f / Now_momentum) * rigid_col.velocity);
                break;
        }
        
        yield return null;
    }

    public void Set_SpingJoint_length()
    {

        if (pinata.position.y - 135f >= -500f)
            _SpringJoint.distance += 135f;

        else
        {
            float length = pinata.position.y + 505f;
            _SpringJoint.distance += length;

        }
        
        StartCoroutine(MoveDown());
    }

    IEnumerator MoveDown()
    {
        Vector2 Target_pos = pinata.position;
        Target_pos.y -= 135f;
        ++down_count;
        while(true)
        {
            pinata.position = Vector3.MoveTowards(pinata.position, Target_pos, Time.deltaTime * 500f);

            if (down_count == 8)
                _mediator.Event_Receive(Event_num.USER_DIE);
            

            if ((Vector2)pinata.position == Target_pos)
                break;

            yield return null;
        }
    }
    
}
