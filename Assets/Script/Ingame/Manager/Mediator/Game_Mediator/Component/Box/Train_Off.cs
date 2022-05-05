using System.Collections;
using System.Collections.Generic;
using Ingame;
using Ingame_Data;
using Item;
using UnityEngine;

public class Train_Off : MonoBehaviour
{
        private IMediator _mediator;
        public GameObject cloud;
        public Transform itemPool;
        [SerializeField] private SaveIngame_Subject save;
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
                for (int i = 0; i < itemPool.childCount; i++)
                { 
                        if(itemPool.GetChild(i).gameObject.layer == 11 )
                                itemPool.GetChild(i).gameObject.GetComponent<Col_Item>().Update_List();
                }
                save.notifyObserver(); // 부셔진 후 데이터를 다시 저장함 
                _mediator.Event_Receive(Event_num.Launch_Green);
                this.transform.position = new Vector2(1066f, -374f);
                this.gameObject.SetActive(false);
        }
}
