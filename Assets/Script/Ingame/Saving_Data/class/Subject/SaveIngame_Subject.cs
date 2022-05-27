using System;
using System.Collections;
using System.Collections.Generic;
using Ingame;
using UnityEngine;
using UnityEngine.UI;


namespace Ingame_Data
{
    public class SaveIngame_Subject : MonoBehaviour, ISubject_Ingame
    {
        private List<IObserver_Ingame> listObserver;
        [Header("Observer")]
        private IObserver_Ingame observer_Stage;

        [SerializeField] private GameManage _gameManage;

        [SerializeField] private Determine_BoxType _boxType;
        private void Awake()
        {
            ResisterObserver();
        }

        public void ResisterObserver()
        {
            listObserver = new List<IObserver_Ingame>();
            for(int i =0; i<this.transform.childCount; i++)
                listObserver.Add(this.gameObject.transform.GetChild(i).gameObject.GetComponent<IObserver_Ingame>());
        }

        public void RemoveObserver()
        {
            
        }

        public void Load_Game()
        {
            for(int i =0; i<this.transform.childCount; i++)
                listObserver[i].LoadData_ToIngame();
            
            _gameManage.Event_Receive(Event_num.Launch_Green);
        }

        public void notifyObserver()
        { 
            for (int i = 0; i < listObserver.Count; i++)
                listObserver[i].Update_Status();
           
            // 저장되었음을 알리는 ui 띄워주기 
            
        }
    }
}
