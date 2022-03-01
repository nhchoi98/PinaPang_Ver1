
using System;
using Ingame;
using UnityEngine.UI;
using Manager;
using UnityEngine;

namespace NewBlock{
    public class New_Block_InfoUI : MonoBehaviour
    {
        public Transform img;
        private bool Is_NEXT;
        [SerializeField] private IMediator _mediator;

        private void Awake()
        {
            _mediator = GameObject.FindWithTag("GameController").GetComponent<IMediator>();
        }

        private void OnEnable() => img.gameObject.GetComponent<Image>().SetNativeSize();
        
        public void OnClick_Exit()
        {
            _mediator.Event_Receive(Event_num.SET_NEW_STAGE);
            this.gameObject.SetActive(false);
        }
    }
}
