
using System;
using Fire;
using Ingame;
using UnityEngine;
using Manager;
public class Bonus_Time : MonoBehaviour
{
    [SerializeField] private IMediator _mediator;

    [SerializeField]

    public Transform _BONUS_TR;

    private void Awake()
    {
        _mediator = GameObject.FindWithTag("GameController").GetComponent<IMediator>();
    }

    public void End()
    {
        for (int i = 0; i < _BONUS_TR.childCount; i++)
            _BONUS_TR.GetChild(i).gameObject.GetComponent<Flame_Down>().Ignition();
        
        _mediator.Event_Receive(Event_num.PINATA_SPAWN);
        this.gameObject.SetActive(false);
    }

}
