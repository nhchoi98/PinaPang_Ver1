using System.Collections;
using System.Collections.Generic;
using Ingame;
using UnityEngine;

public class Train_Off : MonoBehaviour
{
        private IMediator _mediator;
        public GameObject cloud;
        
        private void Awake()
        {
                _mediator = GameObject.FindWithTag("GameController").GetComponent<IMediator>();
        }
        
        public void On_Off()
        {
                GameObject obj;
                this.transform.GetChild(1).gameObject.transform.SetParent(this.gameObject.transform.parent);
                obj = Instantiate(cloud, new Vector2(-12f,0f),Quaternion.identity);
                obj.gameObject.transform.SetParent(this.transform);
                _mediator.Event_Receive(Event_num.Launch_Green);
                this.transform.position = new Vector2(1066f, -374f);
                this.gameObject.SetActive(false);
        }
}
